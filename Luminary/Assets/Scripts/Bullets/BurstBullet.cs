using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//炸裂する弾
public class BurstBullet : AbstructBullet
{
    [SerializeField]
    private GameObject spawnBullet = null; //撃つ予定の弾

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
    }

    //消えるときの更新
    protected override void EraseUpdate()
    {
        for (int i = 0; i < 8; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, 45.0f * i); //角度を設定
            bulManager.CreateBullet(spawnBullet, transform.position, qua, baseSpeed);//弾を生成
        }
        Quaternion effQua = Quaternion.Euler(0.0f, 0.0f, 0.0f); //角度を設定
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, effQua); //エフェクトを発生
        delFlag = true; //弾を消す
    }
}