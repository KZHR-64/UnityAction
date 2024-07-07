using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自機狙いの弾
public class LockOnBullet : AbstructBullet
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //親子関係がある場合
        if (transform.parent != null)
        {
            transform.parent = null; //親子関係を解除
        }
        float angle = Calculation.GetAngle(transform.position, player.transform.position); //自機に向かって角度を設定
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
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
