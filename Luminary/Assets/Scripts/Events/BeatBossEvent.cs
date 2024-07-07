using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBossEvent : AbstructEvent
{
    private SpriteRenderer beatEffect = null; //ボス撃破時のエフェクト

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        beatEffect = GetComponent<SpriteRenderer>();
        beatEffect.enabled = false;
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //一定時間経過したら
        if (1.0f <= time)
        {
            eveManager.RestartGame(); //一時停止を解除
            beatEffect.enabled = false; //エフェクトを非表示
            delFlag = true; //オブジェクトを消す
            activeFlag = false;
        }
    }

    //イベントの起動
    public override void Activate()
    {
        activeFlag = true; //イベントを起動
        eveManager.StopGame(); //ゲームを一時停止
        time = 0.0f; //時間を初期化
        SoundEffectManager eManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
        eManager.PlaySe("smashHit.ogg"); //効果音を再生
        beatEffect.enabled = true; //エフェクトを表示
    }
}
