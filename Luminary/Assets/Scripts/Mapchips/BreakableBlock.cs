using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : AbstructMapchip
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //継承側の更新
    protected override void SubUpdate()
    {

    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }

    //壊れた時の処理
    protected override void BrokenUpdate()
    {
        //エフェクトを発生
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_S, transform.position, transform.rotation);
        delFlag = true; //オブジェクトを消す
    }
}
