using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ1ボス、アロイデーモン
public class AlloyDemon1 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyDemonStand"); //待機アニメーションのハッシュ
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyDemonStraightShoot"); //前に撃つアニメーションのハッシュ
    static private readonly int straightShootEndAnimHash = Animator.StringToHash("AlloyDemonStraightShootEnd"); //攻撃をやめるアニメーションのハッシュ
    static private readonly int fallShootAnimHash = Animator.StringToHash("AlloyDemonFallShoot"); //上に撃つアニメーションのハッシュ
    static private readonly int fallShootEndAnimHash = Animator.StringToHash("AlloyDemonFallShootEnd"); //攻撃をやめるアニメーションのハッシュ
    static private readonly int jumpAnimHash = Animator.StringToHash("AlloyDemonJump"); //ジャンプするアニメーションのハッシュ

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
                moveCoroutine = StartCoroutine(StraightShoot()); //コルーチンを再開
            }
            else if (status == 1)
            {
                moveCoroutine = StartCoroutine(FallShoot()); //コルーチンを再開
            }
            else
            {
                moveCoroutine = StartCoroutine(JumpPress()); //コルーチンを再開
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
        yield return new WaitForSeconds(1.0f); //少し待機
        for (int i = 0; i < 4; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, 6.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        animator.Play(straightShootEndAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
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
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 1; //パターンを変更
        yield return null;
    }

    //上に撃ち、弾を降らせる
    private IEnumerator FallShoot()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(fallShootAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
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
        yield return new WaitForSeconds(1.0f); //少し待機
        for (int i = 0; i < 4; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[0].position, transform.rotation); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        animator.Play(fallShootEndAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
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
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 2; //パターンを変更
        yield return null;
    }

    //ジャンプ→押しつぶし
    private IEnumerator JumpPress()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(jumpAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
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
        SetYSpeed(16.0f); //上昇開始
        yMove = true;
        rBody.gravityScale = 0.0f; //重力を無効化
        yield return new WaitForSeconds(1.0f); //少し待機
        yMove = false;
        rBody.gravityScale = 1.0f; //重力を有効化
        float nextX = cameraManager.transform.position.x + (6.0f * transform.localScale.x); //位置を設定
        SetPosition(nextX, transform.position.y);
        //着地するまで待機
        while (!groundFlag)
        {
            yield return null;
        }
        eManager.PlaySe(SeNameList.SE_ELAND_L); //効果音を再生
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(1.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }
}