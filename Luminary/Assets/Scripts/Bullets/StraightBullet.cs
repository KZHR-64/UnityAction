using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//まっすぐ飛ぶ弾
public class StraightBullet : AbstructBullet
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //敵の向きに応じて速度を設定
        if (transform.parent != null)
        {
            causer = transform.parent; //親のTransformを取得
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
    }

    //消えるときの更新
    protected override void EraseUpdate()
    {
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, 0.0f); //角度を設定
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, qua); //エフェクトを発生
        delFlag = true; //弾を消す
    }
}
