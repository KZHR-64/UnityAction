using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ4ボス、アロイウィザード
public class AlloyWizard1 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyWizardStand"); //待機アニメーションのハッシュ
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyWizardStraightShoot"); ///前に撃つアニメーションのハッシュ
    static private readonly int fallShootAnimHash = Animator.StringToHash("AlloyWizardFallShoot"); ///上に撃つアニメーションのハッシュ
    static private readonly int chargeAnimHash = Animator.StringToHash("AlloyWizardCharge"); //溜めるアニメーションのハッシュ
    static private readonly int strikeAnimHash = Animator.StringToHash("AlloyWizardStrike"); //溜め攻撃アニメーションのハッシュ

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
                    moveCoroutine = StartCoroutine(StraightShoot());
                    break;
                case 1:
                    moveCoroutine = StartCoroutine(FallShoot());
                    break;
                case 2:
                    moveCoroutine = StartCoroutine(ChargeShoot());
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

    //前に弾を撃つ
    private IEnumerator StraightShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, 1.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        animator.Play(straightShootAnimHash); //アニメーションを再生
        yield return null;
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
        for (int i = 0; i < 5; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z - 60.0f + (30.0f * i)); //角度を設定
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, 12.0f, transform); //弾を生成
        }
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
        yield return new WaitForSeconds(1.0f); //少し待機
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, 9.0f); // 移動
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
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 1; //パターンを変更
        yield return null;
    }

    //上から弾を降らせる
    private IEnumerator FallShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(fallShootAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 2.0f); // 移動
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
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[i % 2].position, transform.rotation, 12.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.5f); //少し待機
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[2], boneList[i % 2].position, transform.rotation); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.5f); //少し待機
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[i % 2].position, transform.rotation, 12.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.5f); //少し待機
        }
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 9.0f); // 移動
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
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 2; //パターンを変更
        yield return null;
    }

    //溜めてから弾を撃つ
    private IEnumerator ChargeShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(chargeAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 8.0f, 3.0f); // 移動
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
        animator.Play(strikeAnimHash); //アニメーションを再生
        yield return null;
        bulletManager.CreateBullet(bulletList[3], boneList[0].position, transform.rotation, 16.0f); //弾を生成
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
        yield return new WaitForSeconds(1.0f); //少し待機
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
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 13.0f, 3.0f); // 移動
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
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }
}