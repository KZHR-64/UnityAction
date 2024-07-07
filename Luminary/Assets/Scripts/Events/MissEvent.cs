using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissEvent : AbstructEvent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //一定時間経過したら
        if(5.0 <= time)
        {
            eveManager.MissFlag = true; //ミスしたフラグをtrueに
            Destroy(this); //オブジェクトを消す
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }

    //イベントの起動
    public override void Activate()
    {

    }
}
