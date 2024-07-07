using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//炸裂する弾
public class SparkBullet : AbstructBullet
{
    private Animator animator = null;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //自機の向きに応じて速度を設定
        if (transform.parent != null)
        {
            causer = transform.parent; //自機のTransformを取得
            transform.parent = null; //親子関係を解除
            transform.localScale = causer.localScale; //向きを設定
        }
        animator = GetComponent<Animator>(); //Animatorを取得
    }

    //継承側の更新
    protected override void SubUpdate()
    {

    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        //アニメーションが終わったら
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (1.0f <= currentState.normalizedTime)
        {
            delFlag = true; //エフェクトを消す
        }
    }
}