using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : AbstructEffect
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //最初の更新
    protected override void FirstUpdate()
    {

    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //アニメーションが終わったら
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (1.0f <= currentState.normalizedTime)
        {
            delFlag = true; //エフェクトを消す
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }
}
