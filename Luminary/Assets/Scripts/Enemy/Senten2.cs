using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ5ボス、穿天（再戦）
public class Senten2 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("SentenStand"); //待機アニメーションのハッシュ
    static private readonly int straightShootAnimHash = Animator.StringToHash("SentenShoot"); //前に撃つアニメーションのハッシュ
    static private readonly int dashAnimHash = Animator.StringToHash("SentenDash"); //突進するアニメーションのハッシュ
    static private readonly int backStepAnimHash = Animator.StringToHash("SentenBackStep"); //後ろに下がるアニメーションのハッシュ
    static private readonly int jumpAnimHash = Animator.StringToHash("SentenJump"); //ジャンプするアニメーションのハッシュ

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

            //コルーチンを再開
            switch (status)
            {
                case 0:
                    moveCoroutine = StartCoroutine(StraightShoot());
                    break;
                case 1:
                    moveCoroutine = StartCoroutine(DashAttack());
                    break;
                case 2:
                    moveCoroutine = StartCoroutine(JumpPress());
                    break;
                case 3:
                    moveCoroutine = StartCoroutine(BackStep());
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
        yield return new WaitForSeconds(0.1f); //少し待機
        for (int i = 0; i < 3; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, 16.0f, transform); //弾を生成   
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.1f); //少し待機
        }
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.5f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 3; //パターンを変更
        yield return null;
    }

    //突進
    private IEnumerator DashAttack()
    {
        yield return new WaitForSeconds(0.5f); //少し待機
        animator.Play(dashAnimHash); //アニメーションを再生
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
        xSpeed = 16.0f * transform.localScale.x; //速度を設定
        yield return new WaitForSeconds(0.5f); //少し待機
        xSpeed = 0.0f; //速度を設定
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.5f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 3; //パターンを変更
        yield return null;
    }

    //ジャンプ
    private IEnumerator JumpPress()
    {
        animator.Play(jumpAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        SetYSpeed(24.0f); //上昇開始
        yMove = true;
        rBody.gravityScale = 0.0f; //重力を無効化
        yield return new WaitForSeconds(0.5f); //少し待機
        yMove = false;
        rBody.gravityScale = 2.0f; //重力を有効化
        float nextX = player.transform.position.x; //自機の上に位置を設定
        SetPosition(nextX, transform.position.y);
        //着地するまで待機
        while (!groundFlag)
        {
            yield return null;
        }
        yield return null;
        eManager.PlaySe(SeNameList.SE_ELAND_L); //効果音を再生
        //左右に弾を生成
        bulletManager.CreateBullet(bulletList[1], transform.position, transform.rotation, 12.0f);
        AbstructBullet ab = bulletManager.CreateBullet(bulletList[1], transform.position, transform.rotation, 12.0f);
        ab.transform.localScale = new Vector2(-1.0f, 1.0f);
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = Random.Range(0, 2); //パターンを変更
        yield return null;
    }

    //後ろに下がる
    private IEnumerator BackStep()
    {
        animator.Play(backStepAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //効果音を再生
        xSpeed = -8.0f * transform.localScale.x; //速度を設定
        yield return new WaitForSeconds(0.2f); //少し待機
        xSpeed = 0.0f; //速度を設定
        animator.Play(standAnimHash); //アニメーションを再生
        yield return new WaitForSeconds(0.5f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 2; //パターンを変更
        yield return null;
    }
}
