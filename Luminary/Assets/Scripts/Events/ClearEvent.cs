using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEvent : AbstructEvent
{
    private Animator clearAnimator = null; //クリア時のエフェクト

    private bool effectFlag = false; //エフェクトを出したか

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        clearAnimator = GetComponent<Animator>();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //一定時間経過したら
        if(1.0f <= time && !effectFlag)
        {
            soundEffectManager.PlaySe("shakin.ogg"); //効果音を再生
            clearAnimator.Play("MoveClearEffect"); //クリア時のアニメーションを再生
            effectFlag = true; //エフェクトを出したフラグをtrueに
        }
        if (6.0f <= time)
        {
            eveManager.ClearFlag = true; //クリアしたフラグをtrueに
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }

    //イベントの起動
    public override void Activate()
    {
        activeFlag = true; //イベントを起動
        soundManager.StopBgm(); //BGMを停止
        //eveManager.GameStopFlag = true; //ゲームを一時停止
        time = 0.0f; //時間を初期化
    }
}
