using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigExplodeEffect : AbstructEffect
{
    private float effectTime = 0.0f; //エフェクトを呼ぶ時間

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //最初の更新
    protected override void FirstUpdate()
    {

    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //一定時間ごとに
        if(0.3f <= effectTime && time <= delTime - 0.5f)
        {
            //座標を設定
            float plusPosX = Random.Range(-2.0f, 2.0f);
            float plusPosY = Random.Range(-2.0f, 2.0f);
            Vector3 effPos = new Vector3(transform.position.x + plusPosX, transform.position.y - plusPosY, transform.position.z);

            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, effPos, transform.rotation);
            effectTime %= 0.3f; //時間を戻す
        }
        //消える時間なら
        if(delTime <= time)
        {
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation, null, 3.0f);
        }
        effectTime += Time.deltaTime; //時間を増加
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }
}
