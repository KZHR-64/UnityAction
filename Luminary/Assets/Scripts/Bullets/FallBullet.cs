using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一度画面外に出てから自機に向かって落ちる弾
public class FallBullet : AbstructBullet
{
    float firstSpeed = 0.0f; //最初に設定した速度
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
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f); //角度は固定
        firstSpeed = baseSpeed; //最初の速度を設定
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //コルーチンが終わっているなら
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Routine()); //コルーチンを再開
        }
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
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, qua, null, 2.0f); //エフェクトを発生
        delFlag = true; //弾を消す
    }

    //動作用のコルーチン
    private IEnumerator Routine()
    {
        //画面外に出るまで待機
        while(!outCameraFlag)
        {
            yield return null;
        }
        SetBaseSpeed(0.0f); //停止
        yield return new WaitForSeconds(1.0f); //しばらく待機
        SetPosition(player.transform.position.x, transform.position.y); //自機の真上に移動
        SetBaseSpeed(firstSpeed); //速度を戻す
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f); //角度を下向きに
        yield return null;
    }
}