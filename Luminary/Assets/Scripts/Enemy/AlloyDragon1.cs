using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ3ボス、アロイドラゴン
public class AlloyDragon1 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyDragonStand"); //待機アニメーションのハッシュ
    static private readonly int fireAnimHash = Animator.StringToHash("AlloyDragonFire"); //火炎攻撃アニメーションのハッシュ
    static private readonly int tailAnimHash = Animator.StringToHash("AlloyDragonTailShoot"); //尻尾攻撃アニメーションのハッシュ
    static private readonly int wingAnimHash = Animator.StringToHash("AlloyDragonWingShoot"); //翼攻撃アニメーションのハッシュ

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector2(-1.0f, 1.0f); //向きを設定
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //コルーチンが終わっているなら
        if (moveCoroutine == null)
        {
            //コルーチンを再開
            switch (status)
            {
                case 0:
                    moveCoroutine = StartCoroutine(FireShoot());
                    break;
                case 1:
                    moveCoroutine = StartCoroutine(TailShoot());
                    break;
                case 2:
                    moveCoroutine = StartCoroutine(WingShoot());
                    break;
                default:
                    break;
            }
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
            //エフェクトを発生
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_L, transform.position, transform.rotation); //エフェクトを発生
            status = 1; //状態を変更
        }

        //一定時間経ったら
        if (5.0f <= time)
        {
            delFlag = true; //敵を消す
        }
    }

    //火炎放射
    private IEnumerator FireShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        int count = 0;
        animator.Play(fireAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 8.0f, -1.0f); // 移動
        //移動するまで待機
        while(!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        while (count < 50)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            //2～4秒の間、1秒ごとにミサイルを撃つ
            if(20 <= count && count <= 40 && count % 10 == 0)
            {
                enemyManager.CreateEnemy(enemyList[0], boneList[1].position, boneList[1].rotation); //弾を生成
                eManager.PlaySe(SeNameList.SE_ESHOOT_2); //効果音を再生
            }
            count++; //カウントを増加
            yield return new WaitForSeconds(0.1f); //少し待機
        }
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 12.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.5f); //少し待機
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(3.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 1; //パターンを変更
        yield return null;
    }

    //尻尾の銃
    private IEnumerator TailShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(tailAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, 3.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        for (int i = 0; i < 2; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[2].position, boneList[2].rotation, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 12.0f, 3.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.5f); //少し待機
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(3.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 2; //パターンを変更
        yield return null;
    }

    //翼の銃
    private IEnumerator WingShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(wingAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 9.0f, 2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        xSpeed = -16.0f;
        for(int i = 0; i < 6; i++)
        {
            enemyManager.CreateEnemy(enemyList[0], boneList[1].position, boneList[1].rotation); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_2); //効果音を再生
            yield return new WaitForSeconds(0.2f); //少し待機
        }
        yield return new WaitForSeconds(1.0f); //しばらく待機
        rBody.MovePosition(new Vector2(12.0f, -2.0f));
        xSpeed = 0.0f; //速度を設定
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.5f); //少し待機
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(3.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }
}
