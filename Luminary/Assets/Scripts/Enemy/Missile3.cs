using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミサイル（追尾）
public class Missile3 : AbstructEnemy
{
    private float shootAngle = 0.0f; //進む角度

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
        //開始しばらく
        if (time <= 2.0f)
        {
            shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //自機への角度を取得
        }

        //向きに合わせ速度を設定
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //ラジアンを取得
        xSpeed = 6.0f * Mathf.Cos(rad) * transform.localScale.x; //速度を設定
        ySpeed = 6.0f * Mathf.Sin(rad) * transform.localScale.x; //速度を設定

        //何かに当たった場合
        if (hitFlag)
        {
            defeatFlag = true; //やられたことに
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        float angle = Calculation.GetSpin(transform.localEulerAngles.z, shootAngle, 360.0f); //回転
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle); //角度を設定
    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //エフェクトを発生
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_S, transform.position, transform.rotation);
        delFlag = true; //敵を消す
    }
}