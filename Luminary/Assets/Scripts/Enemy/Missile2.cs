using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミサイル（狙ってから直進）
public class Missile2 : AbstructEnemy
{
    private float shootAngle = 0.0f; //進む角度
    private float baseSpeed = 0.0f; //基となる速度

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
        shootAngle = transform.localEulerAngles.z; //最初の角度を設定
        moveCoroutine = StartCoroutine(Routine()); //コルーチンを開始
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //向きに合わせ速度を設定
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //ラジアンを取得
        xSpeed = baseSpeed * Mathf.Cos(rad) * transform.localScale.x; //速度を設定
        ySpeed = baseSpeed * Mathf.Sin(rad) * transform.localScale.x; //速度を設定

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

    //動作用のコルーチン
    private IEnumerator Routine()
    {
        baseSpeed = 8.0f; //速度を設定
        yield return new WaitForSeconds(0.1f); //しばらく待機
        baseSpeed = 0.5f; //速度を設定
        shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //自機への角度を取得
        yield return new WaitForSeconds(0.5f); //しばらく待機
        baseSpeed = 8.0f; //速度を設定

        moveCoroutine = null; //コルーチンを初期化

        yield return null;
    }
}