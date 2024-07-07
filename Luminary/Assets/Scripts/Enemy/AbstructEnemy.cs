using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AbstructEnemy : MonoBehaviour
{
    [SerializeField]
    protected int maxHp = 0; //HPの最大値
    [SerializeField]
    protected bool bossFlag = false; //ボスか
    [SerializeField]
    protected bool actOutCamera = false; //画面外でも動くか
    [SerializeField]
    protected bool itemDropFlag = true; //アイテムを落とすか
    [SerializeField]
    protected GroundChecker groundChecker = null; //着地判定
    [SerializeField]
    protected Collider2D[] damageCollider = null; //ダメージ判定
    [SerializeField]
    protected Transform[] boneList = null; //参照するボーン
    [SerializeField]
    protected GameObject[] bulletList = null; //撃つ予定の弾
    [SerializeField]
    protected GameObject[] enemyList = null; //出す予定の敵

    protected Rigidbody2D rBody = null;
    protected Animator animator = null;
    protected Renderer rend = null;
    protected Renderer[] rendererList = null;
    protected Coroutine moveCoroutine = null; //動作用のコルーチン
    protected int hp; //HP
    protected bool firstMoveFlag = true; //始めての動作か
    protected bool delFlag = false; //消去するか
    protected bool defeatFlag = false; //やられたか
    protected bool hitFlag = false; //何かに当たったか
    protected bool groundFlag = true; //着地しているか
    protected bool landingFlag = false; //着地直後か
    protected float time = 0.0f; //経過時間
    protected float xSpeed = 0.0f; //x方向の速度
    protected float ySpeed = 0.0f; //y方向の速度
    protected bool xStop = false; //横方向の移動をやめるか
    protected bool yMove = false; //縦方向に移動するか
    protected int status = 0; //行動パターン
    protected bool timerStop = false; //時間を止めるか
    protected float damageTime = 0.0f; //ダメージ後の無敵時間

    protected Player player = null; //自機のオブジェクト
    protected EnemyManager enemyManager = null; //敵関連のオブジェクト
    protected BulletManager bulletManager = null; //弾の管理オブジェクト
    protected SoundEffectManager eManager = null; //効果音関連のオブジェクト
    protected EffectManager effManager = null; //エフェクト関連のオブジェクト
    protected CameraManager cameraManager = null; //カメラ関連のオブジェクト

    static protected readonly float damageAlphaBase = 0.5f; //ダメージ時のアルファ値の基

    public int MaxHp { get { return maxHp; } } //最大HP
    public int Hp { get { return hp; } } //HP
    public bool DelFlag { get { return delFlag; } } //消去するか
    public bool DefeatFlag { get { return defeatFlag; } } //やられたか
    public bool BossFlag { get { return bossFlag; } } //ボスか
    public bool ItemDropFlag { get { return itemDropFlag; } } //アイテムを落とすか

    // Start is called before the first frame update
    protected void Start()
    {
        rBody = GetComponent<Rigidbody2D>(); //RigidBodyを取得
        animator = GetComponent<Animator>(); //Animatorを取得
        rend = GetComponent<Renderer>(); //Rendererを取得
        rendererList = GetComponentsInChildren<Renderer>();
        hp = maxHp; //HPを設定
    }

    // Update is called once per frame
    protected void Update()
    {
        //開始直後のフレームでは動作しない
        if (firstMoveFlag)
        {
            firstMoveFlag = false; //始めの動作のフラグをfalseに
            return; //終了
        }

        if (delFlag) return; //消去するなら終了
        //ダメージ時の無敵中なら
        if(0.0f < damageTime)
        {
            damageTime -= Time.deltaTime; //無敵時間を減少
            damageTime = Mathf.Max(damageTime, 0.0f); //時間を調整
        }
        //やられているなら
        if (defeatFlag)
        {
            DefeatUpdate(); //やられた時の動作
        }
        //やられていないなら
        else
        {
            SubUpdate(); //継承側の更新
        }
        //時間を止めないなら
        if (!timerStop)
        {
            time += Time.deltaTime; //時間を増加
        }
        hitFlag = false; //当たった判定を初期化
    }

    protected void FixedUpdate()
    {
        float xs = xSpeed; //x方向の速度
        float ys = rBody.velocity.y; //y方向の速度

        bool justGroundFlag = groundFlag; //直前の接地判定

        //接触判定があるなら
        if(groundChecker != null)
        {
            groundFlag = groundChecker.GetGround(); //地面に接触しているか判定
        }

        //着地直後なら
        if(!justGroundFlag && groundFlag)
        {
            landingFlag = true; //着地フラグをtrueに
        }
        //そうでないなら
        else
        {
            landingFlag = false; //着地フラグをfalseに
        }

        //止まるなら
        if (xStop)
        {
            xs = 0.0f; //速度を0に
            xStop = false; //止まるフラグをfalseに
        }
        //縦方向に移動するなら
        if(yMove)
        {
            ys = ySpeed; //y方向の速度を設定
        }

        SubFixUpdate(); //継承側の更新

        rBody.velocity = new Vector2(xs, ys); //速度の設定

        //Rendererがあるなら
        if(rend)
        {
            Color col = rend.material.color;
            //ダメージ時の無敵中なら
            if (0.0f < damageTime)
            {
                rend.material.color = new Color(col.r, col.g, col.b, damageAlphaBase + Mathf.PingPong(time, 0.4f)); //キャラを点滅させる
            }
            //そうでないなら
            else
            {
                rend.material.color = new Color(col.r, col.g, col.b, 1.0f); //キャラをそのまま表示
            }
        }

        //子オブジェクトも変更
        foreach(Renderer r in rendererList)
        {
            Color col = r.material.color;
            //ダメージ時の無敵中なら
            if (0.0f < damageTime)
            {
                r.material.color = new Color(col.r, col.g, col.b, damageAlphaBase + Mathf.PingPong(time, 0.4f)); //キャラを点滅させる
            }
            //そうでないなら
            else
            {
                r.material.color = new Color(col.r, col.g, col.b, 1.0f); //キャラをそのまま表示
            }
        }
    }

    //継承側の更新
    protected virtual void SubUpdate() { }

    //継承側の更新
    protected virtual void SubFixUpdate() { }

    //やられた時の動作
    protected virtual void DefeatUpdate()
    {
        delFlag = true; //敵を消す
    }

    //最初の設定
    public void FirstSetting(EnemyManager em, Player p, BulletManager bm, SoundEffectManager sem, EffectManager eff, CameraManager cam)
    {
        player = p;
        enemyManager = em;
        bulletManager = bm;
        eManager = sem;
        effManager = eff;
        cameraManager = cam;
    }

    //当たり判定と接触した場合
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //マップチップに当たった場合
        if (collision.gameObject.CompareTag("Mapchip") || collision.gameObject.CompareTag("Ground"))
        {
            hitFlag = true; //当たった判定をtrueに
        }
    }

    //当たり判定と接触した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //自機に当たった場合
        if (collision.gameObject.CompareTag("Player"))
        {
            hitFlag = true; //当たった判定をtrueに
        }
        //弾に当たった場合
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (defeatFlag) return; //やられているなら終了
            AbstructBullet ab = collision.gameObject.GetComponent<AbstructBullet>(); //オブジェクトを取得
            if (!ab.HitEnemy) return; //敵に当たらないなら飛ばす
            //無敵時間が発生していないなら
            if (damageTime <= 0.0f)
            {
                hp -= ab.Damage; //ダメージ分HPを減らす
                damageTime = 1.0f; //無敵時間を設定
            }

            //HPが0になったら
            if (hp <= 0)
            {
                defeatFlag = true; //やられたフラグをtrueに
                time = 0.0f; //時間を初期化
                status = 0; //状態を初期化
                foreach(Collider2D col in damageCollider)
                {
                    col.enabled = false; //当たり判定を消す
                }
                //コルーチンが実行中なら
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine); //コルーチンを停止
                }
                //ボスなら
                if (bossFlag)
                {
                    enemyManager.BeatBoss(); //ボス撃破時のイベントを開始
                }
            }
            //0でなければ
            else
            {
                //エフェクトを発生
                effManager.CreateEffect(EffectNameList.EFFECT_HIT, transform.position, transform.rotation, transform);
            }

            eManager.PlaySe(SeNameList.SE_HIT_ENEMY); //効果音を再生
        }
    }

    //位置を設定
    public void SetPosition(float setX, float setY)
    {
        transform.position = new Vector3(setX, setY, transform.position.z);
    }

    //速度を設定
    public void SetXSpeed(float setSpeed)
    {
        xSpeed = setSpeed * transform.localScale.x; //速度を設定
    }

    //速度を設定
    public void SetYSpeed(float setSpeed)
    {
        ySpeed = setSpeed; //速度を設定
    }
}
