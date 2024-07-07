using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : AbstructMapchip
{
    [SerializeField]
    private Vector2 moveDistance; //移動量
    [SerializeField]
    private float baseSpeed = 0.0f; //1秒間の移動量

    private bool backFlag = false; //引き返すか
    private Vector2 startPos; //開始位置
    private Vector2 backPos; //折り返し位置
    //private Vector2 calcVelocity; //前フレームからの移動量
    private Vector2 lastPos; //前フレームの位置

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startPos = transform.position; //開始位置を設定
        backPos = startPos + moveDistance; //折り返し位置を設定
        lastPos = rBody.position; //前の位置を設定
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        Vector2 destination = backFlag ? startPos : backPos; //目的地を設定

        //目的地に近づいたら
        if (Vector2.Distance(transform.position, destination) < 0.05f)
        {
            backFlag = !backFlag; //引き返すフラグを反転
            destination = backFlag ? startPos : backPos; //目的地を設定
        }
        //遠いなら
        else
        {
            Vector2 nextPos = Vector2.MoveTowards(transform.position, destination, baseSpeed * Time.deltaTime); //次の位置を設定
            rBody.MovePosition(nextPos); //次の位置に移動
        }

        plusVelocity = (rBody.position - lastPos) / Time.deltaTime; //移動量を設定
        lastPos = rBody.position; //前の位置を設定
    }

    //壊れた時の処理
    protected override void BrokenUpdate()
    {

    }
}
