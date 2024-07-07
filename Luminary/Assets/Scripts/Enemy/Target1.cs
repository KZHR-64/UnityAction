using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target1 : AbstructEnemy
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

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //エフェクトを発生
        effManager.CreateEffect(EffectNameList.EFFECT_HIT, transform.position, transform.rotation);
        delFlag = true; //敵を消す
    }
}
