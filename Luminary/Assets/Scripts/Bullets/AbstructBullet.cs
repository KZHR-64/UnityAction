using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructBullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 0; //ダメージ量
    [SerializeField]
    protected float baseSpeed = 0.0f; //基となる速度
    [SerializeField]
    private bool actOutCamera = false; //画面外でも動くか
    [SerializeField]
    private bool hitMap = true; //地形に当たるか
    [SerializeField]
    private bool hitPlayer = true; //自機に当たるか
    [SerializeField]
    private bool hitEnemy = true; //敵に当たるか
    [SerializeField]
    protected bool penetration = false; //貫通するか
    [SerializeField]
    protected int breakPower = 0; //地形を壊す力

    protected Rigidbody2D rBody = null;
    protected Transform causer = null; //弾を撃ったオブジェクトのTransform
    protected Coroutine moveCoroutine = null; //動作用のコルーチン
    protected bool delFlag = false; //消去するか
    protected bool hitFlag = false; //地形に当たったか
    protected bool outCameraFlag = false; //画面外に出たか
    protected float time = 0.0f; //経過時間
    protected float xSpeed = 0.0f; //x方向の速度
    protected float ySpeed = 0.0f; //y方向の速度

    protected Player player = null; //自機のオブジェクト
    protected BulletManager bulManager = null; //弾管理のオブジェクト
    protected EffectManager effManager = null; //エフェクト管理のオブジェクト
    protected SoundEffectManager eManager = null; //効果音関連のオブジェクト

    public int Damage { get { return damage; } } //ダメージ量
    public bool DelFlag { get { return delFlag; } } //消去するか
    public bool HitPlayer { get { return hitPlayer; } } //自機に当たるか
    public bool HitEnemy { get { return hitEnemy; } } //敵に当たるか
    public int BreakPower { get { return breakPower; } } //地形を壊す力

    // Start is called before the first frame update
    protected void Start()
    {
        rBody = GetComponent<Rigidbody2D>(); //RigidBodyを取得
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //消去するなら終了
        SubUpdate(); //継承側の更新
        time += Time.deltaTime; //時間を増加
    }

    protected void FixedUpdate()
    {
        SubFixUpdate(); //継承側の更新
        if (rBody)
        {
            rBody.velocity = new Vector2(xSpeed, ySpeed); //速度の設定
        }
        //当たっていて貫通しない場合
        if(hitFlag && !penetration)
        {
            EraseUpdate(); //弾を消す処理
        }
        hitFlag = false; //地形に当たっているフラグをfalseに
    }

    //継承側の更新
    protected virtual void SubUpdate() { }

    //継承側の更新
    protected virtual void SubFixUpdate() { }

    //消えるときの更新
    protected virtual void EraseUpdate()
    {
        delFlag = true; //弾を消す
    }

    //最初の設定
    public void FirstSetting(BulletManager bm, Player p, EffectManager eff, SoundEffectManager sem)
    {
        bulManager = bm;
        player = p;
        effManager = eff;
        eManager = sem;
    }

    //速度を設定
    protected void SetSpeed(bool checkScale = true)
    {
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //ラジアンを取得
        xSpeed = baseSpeed * Mathf.Cos(rad); //速度を設定
        ySpeed = baseSpeed * Mathf.Sin(rad); //速度を設定
        //向きを反映する場合
        if(checkScale)
        {
            xSpeed *= transform.localScale.x; //速度を設定
            ySpeed *= transform.localScale.x; //速度を設定
        }
    }
    
    //当たり判定と接触した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //地形に当たった場合
        if(collision.gameObject.CompareTag("Ground") && hitMap)
        {
            hitFlag = true; //当たったフラグをtrueに
        }
        //自機に当たった場合
        if (collision.gameObject.CompareTag("Player") && hitPlayer)
        {
            hitFlag = true; //当たったフラグをtrueに
        }
        //敵に当たった場合
        if (collision.gameObject.CompareTag("Enemy") && hitEnemy)
        {
            AbstructEnemy ae = collision.gameObject.GetComponent<AbstructEnemy>(); //オブジェクトを取得
            hitFlag = true; //当たったフラグをtrueに
        }
    }

    //カメラの範囲から出た場合
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("GeneratorActiveArea")) return;

        //画面外で動作しないなら
        if (!actOutCamera)
        {
            delFlag = true; //弾を消す
        }
        //動作するなら
        else
        {
            outCameraFlag = true; //出たフラグをtrueに
        }
    }

    //位置を設定
    public void SetPosition(float setX, float setY)
    {
        transform.position = new Vector3(setX, setY, transform.position.z);
    }

    //基となる速度を設定
    public void SetBaseSpeed(float setBaseSpeed)
    {
        baseSpeed = setBaseSpeed; //基の速度を設定
        SetSpeed(); //速度を再設定
    }
}
