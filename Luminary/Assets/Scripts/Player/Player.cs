using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject[] pBullet = null; //弾
    [SerializeField]
    private GroundChecker gCheck = null; //接地判定
    [SerializeField]
    private List<Renderer> rendererList = null; //編集する画像
    [SerializeField]
    private BulletManager bulletManager = null; //弾の管理オブジェクト
    [SerializeField]
    private bool canPunch = true; //パンチを打てるか
    [SerializeField]
    private bool canShot = true; //弾を撃てるか

    private Rigidbody2D rBody;
    private Collider2D pCollider;
    private Animator animator;
    private Coroutine moveCoroutine = null; //動作用のコルーチン
    private bool firstMoveFlag = true; //始めての動作か
    private bool moveFlag = false; //動かせるか
    private int hp; //HP
    private int healEnergy; //回復エネルギー
    private float xIn = 0.0f; //左右の入力
    private float yIn = 0.0f; //上下の入力
    private float xSpeed = 0.0f; //x方向の速度
    private float ySpeed = 0.0f; //y方向の速度
    private float accel = 0.0f; //加速度
    private bool jumpFlag = false; //ジャンプするか
    private float jumpTime = 0.0f; //ジャンプ時間
    public bool jumpKeyFlag = false; //ジャンプキーが押されたか
    public float jumpKeyTime = 0.0f; //ジャンプの入力時間
    private bool groundFlag = true; //着地しているか
    private bool attackFlag = false; //攻撃するか
    private float damageTime = 0.0f; //ダメージ後の無敵時間

    //入力情報
    private Vector2 stickInput;
    private float jumpInput = 0.0f;

    private Manager manager = null; //管理用のオブジェクト
    private SceneController sceneController = null; //シーン管理用のオブジェクト

    private SaveData saveData = null; //セーブ関連のオブジェクト
    private SoundEffectManager eManager = null; //効果音関連のオブジェクト
    private EffectManager effManager = null; //エフェクト関連のオブジェクト

    static private float speedMax = 8.0f; //速度の基準
    static private float accelBase = 0.2f; //加速度の基準
    static private float jumpSpeed = 12.0f; //ジャンプ速度の基準
    static private readonly float jumpTimeMax = 0.2f; //ジャンプ時間の最大値
    static private readonly float gScale = 2.0f; //重力の基準
    static private readonly float damageTimeBase = 2.0f; //ダメージ時の無敵時間
    static private readonly float damageAlphaBase = 0.5f; //ダメージ時のアルファ値の基
    static private readonly int standAnimHash = Animator.StringToHash("PlayerStand"); //立っているアニメーションのハッシュ
    static private readonly int runAnimHash = Animator.StringToHash("PlayerRun"); //走るアニメーションのハッシュ
    static private readonly int jumpAnimHash = Animator.StringToHash("PlayerJump"); //ジャンプするアニメーションのハッシュ
    static private readonly int punchAnimHash = Animator.StringToHash("PlayerPunch"); //パンチするアニメーションのハッシュ
    static private readonly int powPunchAnimHash = Animator.StringToHash("PlayerPowerPunch"); //パンチするアニメーションのハッシュ
    static private readonly int shotAnimHash = Animator.StringToHash("PlayerShot"); //攻撃するアニメーションのハッシュ
    static private readonly int damageAnimHash = Animator.StringToHash("PlayerDamage"); //ダメージアニメーションのハッシュ

    public int Hp { get { return hp; } set { hp = value; } } //HP
    public int HealEnergy { get { return healEnergy; } set { healEnergy = value; } } //回復エネルギー
    public Collider2D PCollider { get { return pCollider; } } //collider
    public bool MoveFlag { set { moveFlag = value; } } //動かせるか
    public Manager Manager { set { manager = value; } } //管理用のオブジェクト
    public SceneController SceneController { set { sceneController = value; } } //シーン管理用のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        rBody = gameObject.GetComponent<Rigidbody2D>(); //RigidBodyを取得
        pCollider = gameObject.GetComponent<BoxCollider2D>(); //Colliderを取得
        animator = GetComponent<Animator>(); //Animatorを取得
        saveData = sceneController.SaveData; //オブジェクトを取得
        eManager = sceneController.SoundEffectManager; //オブジェクトを取得
        effManager = sceneController.EffectManager; //オブジェクトを取得
        hp = saveData.Hp; //HPを設定
        healEnergy = saveData.HealEnergy; //回復エネルギーを設定

    }

    // Update is called once per frame
    void Update()
    {
        //開始直後のフレームでは動作しない
        if(firstMoveFlag)
        {
            firstMoveFlag = false; //始めの動作のフラグをfalseに
            moveFlag = true; //動作フラグをtrueに
            return; //終了
        }

        //HPが0なら
        if(hp <= 0)
        {
            xIn = 0.0f; //左右の入力を0に
            yIn = 0.0f; //上下の入力を0に
            return; //終了
        }

        CheckInput(); //入力の確認

        Attack(); //攻撃
    }

    private void FixedUpdate()
    {
        bool lastJumpFlag = jumpFlag; //直前のジャンプフラグ

        //HPが0なら
        if (hp <= 0)
        {
            rBody.velocity = new Vector2(0.0f, 0.0f); //速度の設定
            return; //終了
        }

        //xSpeed = rBody.velocity.x; //x方向の速度
        ySpeed = rBody.velocity.y; //y方向の速度

        if (!groundFlag && 0.01f < ySpeed)
        {
            groundFlag = false;
        }
        else
        {
            groundFlag = gCheck.GetGround(); //地面に接触しているか判定
        }

        //左右の入力があり、攻撃中でない場合
        if (0.0f < Mathf.Abs(xIn) && !attackFlag)
        {
            //右の入力が大きい場合
            if (0.0f < xIn)
            {
                transform.localScale = new Vector2(1.0f, 1.0f); //向きを設定
            }
            //左の入力が大きい場合
            else if (xIn < 0.0f)
            {
                transform.localScale = new Vector2(-1.0f, 1.0f); //向きを反転
            }

            accel = accelBase * transform.localScale.x; //加速度を設定
            //動き始めなら
            if (Mathf.Abs(xSpeed) <= 0.5f)
            {
                xSpeed = 0.5f * transform.localScale.x; //初速を設定
            }
            xSpeed += Mathf.Abs(xSpeed) * accel; //加減速
        }
        //ない場合
        else
        {
            accel = groundFlag ? 0.7f + gCheck.GetMapchipFriction() : 0.9f; //減速
            xSpeed *= accel;
        }

        //かなり遅くなったら
        if (Mathf.Abs(xSpeed) < 0.5f)
        {
            xSpeed = 0.0f; //速度を0に
        }

        //接地中でジャンプボタンが押されていない場合
        if (groundFlag)
        {
            jumpFlag = false; //ジャンプフラグをfalseに
            lastJumpFlag = jumpFlag;
            jumpTime = 0.0f; //ジャンプ時間を初期化
        }
        //接地中で攻撃中でなくジャンプするなら（事前入力は0.2秒まで有効）
        if (groundFlag && !jumpFlag && jumpKeyFlag && !attackFlag && jumpKeyTime <= 0.2f)
        {
            ySpeed = jumpSpeed; //ジャンプ速度を設定
            jumpFlag = true; //ジャンプフラグをtrueに
            jumpTime = 0.0f; //ジャンプ時間を初期化
            rBody.gravityScale = 0.0f; //重力を無効化
            eManager.PlaySe("jump.ogg"); //効果音を再生    
        }

        //接地していない場合
        if (!groundFlag)
        {
            //ジャンプボタンが押されている場合
            if (jumpKeyFlag)
            {
                jumpTime += Time.deltaTime; //ジャンプ時間を増加
            }
            //押されていない場合
            else
            {
                jumpFlag = true; //ジャンプフラグをtrueに
                jumpTime = jumpTimeMax; //ジャンプ時間を最大値に    
            }
        }

        //ジャンプ時間が限界に達した場合
        if (jumpTimeMax <= jumpTime)
        {
            rBody.gravityScale = gScale; //重力を設定しなおす
        }

        xSpeed = Mathf.Clamp(xSpeed, -speedMax, speedMax); //移動速度の限界を設定
        ySpeed = Mathf.Clamp(ySpeed, -jumpSpeed, jumpSpeed); //落下、上昇速度の限界を設定

        Vector2 pVelocity = new Vector2(xSpeed, ySpeed); //速度を設定

        //接地しているなら
        if (groundFlag)
        {
            Vector2 vec = gCheck.GetMapchipVelocity(); //マップチップの速度を取得
            pVelocity += vec; //速度を追加
        }

        rBody.velocity = pVelocity; //速度の設定

        if (moveCoroutine == null)
        {
            //左右の入力があるなら
            if(0.5f <= Mathf.Abs(xIn) && !jumpFlag)
            {
                animator.Play(runAnimHash); //アニメーションを再生
            }
            else if (!jumpFlag)
            {
                animator.Play(standAnimHash); //アニメーションを再生
            }
            //ジャンプした瞬間なら
            if (!lastJumpFlag && jumpFlag)
            {
                animator.Play(jumpAnimHash); //アニメーションを再生
            }
            //着地したなら
            if (lastJumpFlag && groundFlag)
            {
                animator.Play(standAnimHash); //アニメーションを再生
            }
        }

        Color col = rendererList[0].material.color;
        
        //ダメージ時の無敵中なら
        if (0.0f < damageTime)
        {
            foreach (Renderer ren in rendererList) {
                ren.material.color = new Color(col.r, col.g, col.b, damageAlphaBase + Mathf.PingPong(Time.time, 0.4f)); //キャラを点滅させる
            }
        }
        //そうでないなら
        else
        {
            foreach (Renderer ren in rendererList)
            {
                ren.material.color = new Color(col.r, col.g, col.b, 1.0f); //キャラをそのまま表示
            }
        }
    }

    //入力の確認
    private void CheckInput()
    {
        //動かせないなら終了
        if (!moveFlag)
        {
            xIn = 0.0f; //左右の入力確認
            yIn = 0.0f; //上下の入力確認
            jumpInput = 0.0f;
            jumpKeyFlag = false; //ジャンプキーフラグをfalseに
            jumpKeyTime = 0.0f; //入力時間を初期化
            return;
        }

        //入力の取得
        stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();
        jumpInput = sceneController.PlayerInput.actions["Jump"].ReadValue<float>();

        xIn = stickInput.x; //左右の入力確認
        yIn = stickInput.y; //上下の入力確認
        
        //ダメージ時の無敵中なら
        if (0.0f < damageTime)
        {
            damageTime -= Time.deltaTime; //無敵時間を減少
            damageTime = Mathf.Max(damageTime, 0.0f); //時間を調整
        }
        //立ち止まる動作中なら
        //if ((damageTimeBase - 0.5f) <= damageTime)
        if (moveCoroutine != null)
        {
            xIn = 0.0f; //左右の入力を0に
            yIn = 0.0f; //上下の入力を0に
            return; //終了
        }

        //ジャンプが入力された場合
        if (sceneController.InputActions["Jump"].triggered)
        {
            jumpKeyFlag = true; //ジャンプキーフラグをtrueに
        }
        //ジャンプが入力されている場合
        if (0.0f != jumpInput)
        {
            //jumpKeyFlag = true; //ジャンプキーフラグをtrueに
            jumpKeyTime += Time.deltaTime; //入力時間を増加
        }
        else
        {
            jumpKeyFlag = false; //ジャンプキーフラグをfalseに
            jumpKeyTime = 0.0f; //入力時間を初期化
        }
    }

    //攻撃
    private void Attack()
    {
        //入力の取得
        bool attackInput = sceneController.PlayerInput.actions["Fire"].triggered;
        bool attackInput2 = sceneController.PlayerInput.actions["Fire2"].triggered;

        //遠距離攻撃ボタンが入力された場合
        if (attackInput && canShot)
        {
            //攻撃中ではないなら
            if (!attackFlag)
            {
                attackFlag = true; //攻撃フラグをtrueに
                moveCoroutine = StartCoroutine(ShotAttack(stickInput.y)); //攻撃を開始
            }
        }

        //近距離攻撃ボタンが入力された場合
        if (attackInput2 && canPunch)
        {
            //攻撃中ではないなら
            if (!attackFlag)
            {
                attackFlag = true; //攻撃フラグをtrueに
                //入力方向によって攻撃を決定
                if(0.5f <= stickInput.y)
                {
                    moveCoroutine = StartCoroutine(PowerPunchAttack()); //強攻撃を開始
                }
                else
                {
                    moveCoroutine = StartCoroutine(PunchAttack()); //弱攻撃を開始
                }
            }
        }
    }

    //回復
    public void Healing(int heal)
    {
        hp += heal; //HPを回復
        hp = Math.Min(hp, Calculation.PLAYER_HP_MAX); //HPを調整
        eManager.PlaySe("heal.ogg"); //効果音を再生
        //エフェクトを発生
        effManager.CreateEffect(EffectNameList.EFFECT_HEAL, transform.position, transform.rotation, transform);
    }

    //回復エネルギーの増加
    public void GainHealEnergy(int gainEn)
    {
        healEnergy += gainEn; //回復エネルギーを増加
        healEnergy = Math.Min(healEnergy, Calculation.HEAL_EN_MAX); //エネルギーを調整
        eManager.PlaySe("heal.ogg"); //効果音を再生
    }

    //当たり判定と接触した場合
    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    //当たり判定と接触した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool damageFlag = false; //ダメージを受けるか
        int damageParam = 0; //ダメージ値

        //弾に当たった場合
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AbstructBullet ab = collision.gameObject.GetComponent<AbstructBullet>(); //オブジェクトを取得

            //自機に当たる場合
            if (ab.HitPlayer)
            {
                damageFlag = true; //ダメージフラグをtrueに
                damageParam = ab.Damage; //ダメージ値を取得
            }
        }

        //ダメージ判定に当たった場合
        if (collision.gameObject.CompareTag("Damage"))
        {
            damageFlag = true; //ダメージフラグをtrueに
            damageParam = 1; //ダメージ値を設定
        }
        //落ちた場合
        else if(collision.gameObject.CompareTag("DeadZone"))
        {
            damageFlag = true; //ダメージフラグをtrueに
            damageParam = Calculation.PLAYER_HP_MAX; //ダメージ値を設定
            damageTime = 0.0f; //ダメージ無敵を解除
        }

        //ダメージを受けるなら
        if(damageFlag && damageTime <= 0.0f)
        {
            hp -= damageParam; //HPを減少
            eManager.PlaySe(SeNameList.SE_HIT); //効果音を再生
            damageTime = damageTimeBase; //無敵時間を設定

            //HPが0になったら
            if (hp <= 0 && moveFlag)
            {
                effManager.CreateEffect(EffectNameList.EFFECT_DEFEAT, transform.position, transform.rotation); //エフェクトを生成
                moveFlag = false; //動作フラグをfalseに

                //自機を非表示にする
                foreach (Renderer ren in rendererList)
                {
                    ren.enabled = false;
                }
            }

            //攻撃中なら
            if (attackFlag)
            {
                attackFlag = false; //攻撃フラグをfalseに
                //コルーチンが実行中なら
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine); //コルーチンを停止
                }
            }
            moveCoroutine = StartCoroutine(DamageMotion()); //ダメージ動作を開始
        }
    }

    //遠距離攻撃のコルーチン
    private IEnumerator ShotAttack(float inputY)
    {
        eManager.PlaySe("pShoot.ogg"); //効果音を再生
        animator.Play(shotAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.2f);
        float zRotate = (1.0f <= inputY) ? 45.0f * transform.localScale.x : 0.0f ;//入力によって角度を設定
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, zRotate);
        bulletManager.CreateBullet(pBullet[0], transform.position, rotation, transform); //弾を生成
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        animator.Play(standAnimHash); //アニメーションを再生
        attackFlag = false; //攻撃フラグをfalseに
        moveCoroutine = null; //コルーチンを初期化
        yield return null;
    }

    //近距離攻撃（パンチ）のコルーチン
    private IEnumerator PunchAttack()
    {
        eManager.PlaySe("pPunch.ogg"); //効果音を再生
        animator.Play(punchAnimHash, 0, 0); //アニメーションを再生
        yield return new WaitForSeconds(0.1f);
        Vector3 pos = new Vector3(transform.position.x + (1.3f * transform.localScale.x), transform.position.y, transform.position.z);
        bulletManager.CreateBullet(pBullet[1], pos, transform.rotation, transform); //弾を生成
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        animator.Play(standAnimHash); //アニメーションを再生
        attackFlag = false; //攻撃フラグをfalseに
        moveCoroutine = null; //コルーチンを初期化
        yield return null;
    }

    //近距離攻撃（強パンチ）のコルーチン
    private IEnumerator PowerPunchAttack()
    {
        eManager.PlaySe("pPowerPunch.ogg"); //効果音を再生
        animator.Play(powPunchAnimHash, 0, 0); //アニメーションを再生
        yield return null;
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        animator.Play(standAnimHash); //アニメーションを再生
        attackFlag = false; //攻撃フラグをfalseに
        moveCoroutine = null; //コルーチンを初期化
        yield return null;
    }

    //ダメージのコルーチン
    private IEnumerator DamageMotion()
    {
        animator.Play(damageAnimHash, 0, 0); //アニメーションを再生
        yield return null;
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        animator.Play(standAnimHash); //アニメーションを再生
        attackFlag = false; //攻撃フラグをfalseに
        moveCoroutine = null; //コルーチンを初期化
        yield return null;
    }

}
