using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : AbstructEvent
{
    private Animator startAnimator; //開始時のアニメーション

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startAnimator = GetComponentInChildren<Animator>();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //時間に達したら
        if (3.0f <= time)
        {
            delFlag = true; //オブジェクトを消す
        }
    }

    //イベントの起動
    public override void Activate()
    {
        activeFlag = true; //イベントを起動
        time = 0.0f; //時間を初期化
        startAnimator.Play("MoveStartEffect"); //アニメーションを再生
    }
}
