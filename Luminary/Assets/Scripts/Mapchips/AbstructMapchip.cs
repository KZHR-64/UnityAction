using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructMapchip : MonoBehaviour
{
    [SerializeField]
    protected int hardness = -1; //硬度
    [SerializeField]
    protected float friction = 1.0f; //摩擦（止める力）
    [SerializeField]
    protected Vector2 plusVelocity; //追加する速度

    protected Rigidbody2D rBody;
    protected bool delFlag = false; //消去するか
    protected bool brokenFlag = false; //壊れているか
    protected float time = 0.0f; //経過時間

    protected SoundEffectManager eManager = null; //効果音関連のオブジェクト
    protected EffectManager effManager = null; //エフェクト関連のオブジェクト

    public bool DelFlag { get { return delFlag; } } //消去するか
    public float Friction { get { return friction; } } //摩擦（止める力）
    public Vector2 PlusVelocity { get { return plusVelocity; } } //追加する速度

    // Start is called before the first frame update
    protected void Start()
    {
        rBody = gameObject.GetComponent<Rigidbody2D>(); //RigidBodyを取得
        eManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
        effManager = FindObjectOfType<EffectManager>(); //オブジェクトを取得
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //消去するなら終了
        time += Time.deltaTime; //時間を増加
        //壊れているなら
        if (brokenFlag)
        {
            BrokenUpdate(); //壊れた時の処理
        }
        //壊れていないなら
        else
        {
            SubUpdate(); //継承側の更新
        }
    }
    
    protected void FixedUpdate()
    {
        SubFixUpdate(); //継承側の更新
    }

    //継承側の更新
    protected virtual void SubUpdate() { }

    //継承側の更新
    protected virtual void SubFixUpdate() { }

    //壊れた時の処理
    protected virtual void BrokenUpdate() { }

    //当たり判定と接触した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //弾に当たった場合
        if(collision.CompareTag("Bullet"))
        {
            AbstructBullet ab = collision.gameObject.GetComponent<AbstructBullet>(); //オブジェクトを取得

            //威力が硬度より上なら
            if(hardness < ab.BreakPower && 0 <= hardness)
            {
                brokenFlag = true; //壊れたフラグをtrueに
            }
        }
    }

}
