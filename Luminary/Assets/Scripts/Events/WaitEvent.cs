using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEvent : AbstructEvent
{
    [SerializeField]
    private float waitTimeMax = -1.0f; //待ち時間

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //時間に達したら
        if(0.0f <= waitTimeMax && waitTimeMax <= time)
        {
            delFlag = true; //オブジェクトを消す
        }

        //ボス戦終了を待っているなら
        if(waitTimeMax < 0.0f)
        {
            //ボス戦が終わっていたら
            if (!eveManager.WaitFlag)
            {
                delFlag = true; //オブジェクトを消す
            }
        }
    }

    //イベントの起動
    public override void Activate()
    {
        activeFlag = true; //イベントを起動
        time = 0.0f; //時間を初期化

        //ボス戦終了を待つなら
        if (waitTimeMax < 0.0f)
        {
            eveManager.WaitFlag = true; //待機フラグをtrueに
        }
    }
}
