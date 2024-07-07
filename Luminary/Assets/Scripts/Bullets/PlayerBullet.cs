using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : AbstructBullet
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //自機の向きに応じて速度を設定
        if (transform.parent != null)
        {
            causer = transform.parent; //自機のTransformを取得
            transform.parent = null; //親子関係を解除
            transform.localScale = causer.localScale; //向きを設定
        }
    }

    //継承側の更新
    protected override void SubUpdate()
    {

    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        SetSpeed(); //速度を設定
        //当たっているなら
        if(hitFlag)
        {
            delFlag = true; //弾を消す
        }
    }
}
