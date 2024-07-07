using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//狙い撃ち
public class Cannon1 : AbstructEnemy
{
    [SerializeField]
    private Transform cannonTrans = null; //砲塔

    private float shootAngle = 0.0f; //撃つ角度

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //コルーチンが終わっているなら
        if(moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Routine()); //コルーチンを再開
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {
        float angle = Calculation.GetSpin(cannonTrans.localEulerAngles.z, shootAngle, 180.0f); //砲塔を回転
        cannonTrans.localEulerAngles = new Vector3(0.0f, 0.0f, angle); //砲塔の角度を設定
    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //エフェクトを発生
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
        delFlag = true; //敵を消す
    }

    //動作用のコルーチン
    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(3.0f); //しばらく待機
        shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //自機への角度を取得
        shootAngle = Mathf.Clamp(shootAngle, 0.0f, 180.0f); //角度を調整
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //効果音を再生
        yield return new WaitForSeconds(1.0f); //しばらく待機
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, cannonTrans.rotation, cannonTrans); //弾を生成
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
        moveCoroutine = null; //コルーチンを初期化

        yield return null;
    }
}
