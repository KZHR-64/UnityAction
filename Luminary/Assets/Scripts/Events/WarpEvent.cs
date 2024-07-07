using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEvent : AbstructEvent
{
    [SerializeField]
    private string nextMapName = ""; //次のステージ名

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //時間に達したら
        if (0.5f <= time)
        {
            eveManager.WarpNextMap(nextMapName); //移動するステージを設定
            delFlag = true; //オブジェクトを消す
        }
    }

    //イベントの起動
    public override void Activate()
    {
        activeFlag = true; //イベントを起動
        time = 0.0f; //時間を初期化
        player.MoveFlag = false; //自機を一時停止
    }
}
