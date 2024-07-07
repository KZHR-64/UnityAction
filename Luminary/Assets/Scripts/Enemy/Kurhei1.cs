using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クーヘイ、その場で弾を撃つ
public class Kurhei1 : AbstructEnemy
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector2(-1.0f, 1.0f); //向きを設定
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //自機の位置に合わせて向きを設定
        float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
        transform.localScale = new Vector2(lScale, 1.0f);

        //コルーチンが終わっているなら
        if (moveCoroutine == null)
        {
            time = 0.0f; //時間を初期化
            moveCoroutine = StartCoroutine(Routine()); //コルーチンを再開
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        ySpeed = Mathf.Clamp(ySpeed, -12.0f, 12.0f); //落下、上昇速度の限界を設定
    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //撃破されてすぐなら
        if (status == 0)
        {
            rBody.gravityScale = 0.5f; //重力を受けるように
            status = 1; //状態を変更
        }

        //一定時間経ったら
        if (0.2f <= time)
        {
            //エフェクトを発生
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //敵を消す
        }
    }

    //動作用のコルーチン
    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(2.0f); //しばらく待機
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //弾を生成
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
        moveCoroutine = null; //コルーチンを初期化

        yield return null;
    }
}
