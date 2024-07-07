using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEffect : AbstructEffect
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //最初の更新
    protected override void FirstUpdate()
    {
        effManager.CreateEffect(EffectNameList.EFFECT_LUMINAL, transform.position, transform.rotation); //エフェクトを生成
        rBody.gravityScale = 0.0f; //しばらく無重力に
        SetYSpeed(6.0f); //速度を設定
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //しばらくしたら
        if(0.5f <= time && rBody.gravityScale == 0.0f)
        {
            rBody.gravityScale = 2.0f; //重力を受けるように
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        //落下速度を調整
        if(rBody.velocity.y < -6.0f)
        {
            SetYSpeed(-6.0f);
        }
    }
}
