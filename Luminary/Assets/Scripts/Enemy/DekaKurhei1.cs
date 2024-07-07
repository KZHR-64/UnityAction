using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ2中ボス、デカクーヘイ
public class DekaKurhei1 : AbstructEnemy
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //コルーチンが終わっているなら
        if (moveCoroutine == null)
        {
            //自機の位置に合わせて向きを設定
            float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
            transform.localScale = new Vector2(lScale, 1.0f);

            //パターンによって行動を変更
            if (status == 0)
            {
                moveCoroutine = StartCoroutine(RapidShoot1()); //コルーチンを再開
            }
            else if (status == 2)
            {
                moveCoroutine = StartCoroutine(RapidShoot2()); //コルーチンを再開
            }
            else
            {
                moveCoroutine = StartCoroutine(MissileShoot()); //コルーチンを再開
            }
        }
    }

    //継承側の更新
    protected override void SubFixUpdate()
    {

    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //撃破されてすぐなら
        if (status == 0)
        {
            rBody.gravityScale = 1.0f; //重力を受けるように
            yMove = false;
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

    //弾を連射1
    private IEnumerator RapidShoot1()
    {
        yield return new WaitForSeconds(1.0f); //少し待機
        SetYSpeed(-8.0f); //下に降りる
        yMove = true;
        yield return new WaitForSeconds(0.5f); //少し待機
        SetYSpeed(0.0f); //停止
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[1].position, transform.rotation, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.1f); //少し待機
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        float zAngle = Calculation.GetAngle(boneList[0].position, player.transform.position, true, transform.localScale.x, 30.0f); //自機への角度を取得
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, zAngle); //角度を設定
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.1f); //少し待機
        }
        yield return new WaitForSeconds(3.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 1; //パターンを変更
        yield return null;
    }

    //弾を連射2
    private IEnumerator RapidShoot2()
    {
        yield return new WaitForSeconds(1.0f); //少し待機
        SetYSpeed(-8.0f); //下に降りる
        yMove = true;
        yield return new WaitForSeconds(0.5f); //少し待機
        SetYSpeed(0.0f); //停止
        for (int i = 0; i < 5; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z + 30.0f); //角度を設定
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, transform); //弾を生成
            qua = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z - 30.0f); //角度を設定
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.5f); //少し待機
            bulletManager.CreateBullet(bulletList[0], boneList[1].position, transform.rotation, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.5f); //少し待機
        }
        yield return new WaitForSeconds(3.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 3; //パターンを変更
        yield return null;
    }

    //追尾ミサイル
    private IEnumerator MissileShoot()
    {
        yield return new WaitForSeconds(1.0f); //少し待機
        SetYSpeed(8.0f); //上に上がる
        yMove = true;
        yield return new WaitForSeconds(0.5f); //少し待機
        SetYSpeed(0.0f); //停止
        for (int i = 0; i < 3; i++)
        {
            float zAngle = (transform.localScale.x < 0) ? 0.0f : 180.0f; // 向きによって角度を設定
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, zAngle); //角度を設定
            enemyManager.CreateEnemy(enemyList[0], boneList[2].position, qua); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_2); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        yield return new WaitForSeconds(3.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = (status + 1) % 4; //パターンを変更
        yield return null;
    }
}
