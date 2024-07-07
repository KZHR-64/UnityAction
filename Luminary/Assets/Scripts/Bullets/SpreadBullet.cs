using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//左右に分裂する弾
public class SpreadBullet : AbstructBullet
{
    [SerializeField]
    private GameObject spawnBullet = null; //撃つ予定の弾

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
        //左右に弾を生成
        bulManager.CreateBullet(spawnBullet, transform.position, qua);
        AbstructBullet ab = bulManager.CreateBullet(spawnBullet, transform.position, qua);
        ab.transform.localScale = new Vector2(-1.0f, 1.0f);
        delFlag = true; //弾を消す
    }
}