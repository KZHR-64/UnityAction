using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructEffect : MonoBehaviour
{
    [SerializeField]
    protected float delTime = -1.0f; //消える時間

    protected Rigidbody2D rBody = null;
    protected Animator animator = null;
    protected bool delFlag = false; //消去するか
    protected float time = 0; //経過時間
    protected float xSpeed = 0.0f; //x方向の速度
    protected float ySpeed = 0.0f; //y方向の速度
    private bool changeXSpeed = false; //x方向の速度を変えるか
    private bool changeYSpeed = false; //y方向の速度を変えるか

    protected EffectManager effManager = null; //エフェクト関連のオブジェクト
    protected SoundEffectManager eManager = null; //効果音関連のオブジェクト

    public bool DelFlag { get { return delFlag; } set { delFlag = value; } } //消去するか

    protected void Awake()
    {
        rBody = GetComponent<Rigidbody2D>(); //RigidBodyを取得
    }

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>(); //Animatorを取得
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //消去するなら終了
        time += Time.deltaTime; //時間を増加
        SubUpdate(); //継承側の更新
        
        //消える時間なら
        if(0.0f < delTime && delTime <= time)
        {
            delFlag = true; //エフェクトを消す
        }
    }

    protected void FixedUpdate()
    {
        SubFixUpdate(); //継承側の更新

        if (rBody)
        {
            float setX = changeXSpeed ? xSpeed : rBody.velocity.x; //設定する速度を決定
            float setY = changeYSpeed ? ySpeed : rBody.velocity.y;
            rBody.velocity = new Vector2(setX, setY); //速度の設定
        }
        changeXSpeed = false; //速度変更フラグをfalseに
        changeYSpeed = false;
    }

    //最初の設定
    public void FirstSetting(EffectManager em, SoundEffectManager sem, float scale)
    {
        effManager = em;
        eManager = sem;
        transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z * scale); //エフェクトの拡縮
        FirstUpdate();
    }

    //最初の更新
    protected virtual void FirstUpdate() { }

    //継承側の更新
    protected virtual void SubUpdate() { }

    //継承側の更新
    protected virtual void SubFixUpdate() { }

    //x方向の速度を設定
    protected void SetXSpeed(float setSpeed)
    {
        xSpeed = setSpeed; //速度を設定
        changeXSpeed = true; //速度変更フラグをtrueに
    }

    //y方向の速度を設定
    protected void SetYSpeed(float setSpeed)
    {
        ySpeed = setSpeed; //速度を設定
        changeYSpeed = true; //速度変更フラグをtrueに
    }
}
