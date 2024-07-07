using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミサイル（直線軌道）
public class Missile1 : AbstructEnemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        yMove = true;
        if (transform.parent != null)
        {
            Transform causer = transform.parent; //親のTransformを取得
            transform.parent = null; //親子関係を解除
            transform.localScale = causer.localScale; //向きを設定
        }
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //向きに合わせ速度を設定
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //ラジアンを取得
        xSpeed = 8.0f * Mathf.Cos(rad) * transform.localScale.x; //速度を設定
        ySpeed = 8.0f * Mathf.Sin(rad) * transform.localScale.x; //速度を設定

        //何かに当たった場合
        if(hitFlag)
        {
            defeatFlag = true; //やられたことに
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //エフェクトを発生
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_S, transform.position, transform.rotation);
        delFlag = true; //敵を消す
    }
}