using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ラスボス、アロイブレイン
public class AlloyBrain : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyBrainStand"); //待機アニメーションのハッシュ
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyBrainShoot"); //前に撃つアニメーションのハッシュ
    static private readonly int rightShootAnimHash = Animator.StringToHash("AlloyBrainRightShoot"); //右から撃つアニメーションのハッシュ
    static private readonly int leftShootAnimHash = Animator.StringToHash("AlloyBrainLeftShoot"); //左から撃つアニメーションのハッシュ
    static private readonly int chargeAnimHash = Animator.StringToHash("AlloyBrainCharge"); //溜めアニメーションのハッシュ
    static private readonly int strikeAnimHash = Animator.StringToHash("AlloyBrainStrike"); //溜め攻撃アニメーションのハッシュ

    private int justStatus = -1; //直前の行動

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
            while (status == justStatus)
            {
                status = Random.Range(0, 3); //パターンを変更
            }
            justStatus = status; //直前の行動を設定
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

    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //撃破されてすぐなら
        if (status == 0)
        {
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_L, transform.position, transform.rotation); //エフェクトを発生
            status = 1; //状態を変更
        }

        //一定時間経ったら
        if (5.0f <= time)
        {
            delFlag = true; //敵を消す
        }
    }

    //前に撃つ
    private IEnumerator StraightShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(straightShootAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 3.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        for (int i = 0; i < 4; i++)
        {
            bulletManager.CreateBullet(bulletList[0], transform.position, transform.rotation, 10.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        animator.Play(standAnimHash); //アニメーションを再生
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }

    //弾を降らせる
    private IEnumerator FallShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(rightShootAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, -8.0f, 3.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //少し待機
        for (int i = 1; i < 8; i++)
        {
            Vector3 shootPos = new Vector3(transform.position.x + 2.0f + (2.0f * i), transform.position.y + 2.0f, transform.position.z); // 弾の位置を設定
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, -90.0f); //角度を設定
            bulletManager.CreateBullet(bulletList[1], shootPos, qua, 20.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.2f); //少し待機
        }
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(leftShootAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 8.0f, 3.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //少し待機
        for (int i = 1; i < 8; i++)
        {
            Vector3 shootPos = new Vector3(transform.position.x - 2.0f - (2.0f * i), transform.position.y + 2.0f, transform.position.z); // 弾の位置を設定
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, -90.0f); //角度を設定
            bulletManager.CreateBullet(bulletList[1], shootPos, qua, 20.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.2f); //少し待機
        }
        yield return new WaitForSeconds(2.0f); //少し待機
        animator.Play(standAnimHash); //アニメーションを再生
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }

    //溜めてから撃つ
    private IEnumerator ChargeShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(chargeAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 4.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //少し待機
        animator.Play(strikeAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_ELAND_L); //効果音を再生
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
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, -90.0f); //角度を設定
        bulletManager.CreateBullet(bulletList[2], transform.position, qua, 16.0f); //弾を生成
        yield return new WaitForSeconds(2.0f); //少し待機
        animator.Play(standAnimHash); //アニメーションを再生
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, -2.0f); // 移動
        //移動するまで待機
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }
}