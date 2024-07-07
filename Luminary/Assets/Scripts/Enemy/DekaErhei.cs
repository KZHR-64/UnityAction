using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ1中ボス、デカエーヘイ
public class DekaErhei : AbstructEnemy
{
    static private readonly int attackAnimHash = Animator.StringToHash("DekaErhei1ArmAttack"); //攻撃するアニメーションのハッシュ
    static private readonly int attackEndAnimHash = Animator.StringToHash("DekaErhei1ArmAttackEnd"); //攻撃をやめるアニメーションのハッシュ
    static private readonly int missileAttackAnimHash = Animator.StringToHash("DekaErhei1Missile"); //ミサイル攻撃のアニメーションのハッシュ

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
            //自機の位置に合わせて向きを設定
            float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
            transform.localScale = new Vector2(lScale, 1.0f);

            //パターンによって行動を変更
            if (status == 0)
            {
                moveCoroutine = StartCoroutine(RapidShoot()); //コルーチンを再開
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
        ySpeed = Mathf.Clamp(ySpeed, -12.0f, 12.0f); //落下、上昇速度の限界を設定
    }

    //やられた時の動作
    protected override void DefeatUpdate()
    {
        //撃破されてすぐなら
        if (status == 0)
        {
            status = 1; //状態を変更
        }

        //一定時間経ったら
        if (0.1f <= time)
        {
            //エフェクトを発生
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //敵を消す
        }
    }

    //弾を連射
    private IEnumerator RapidShoot()
    {
        yield return new WaitForSeconds(1.0f); //少し待機
        animator.Play(attackAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //効果音を再生
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
        for (int i = 0; i < 2; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        animator.Play(attackEndAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //効果音を再生
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

    //ミサイルを連射
    private IEnumerator MissileShoot()
    {
        yield return new WaitForSeconds(1.0f); //少し待機
        animator.Play(missileAttackAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //効果音を再生
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
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, (30.0f - (10.0f * i)) * transform.localScale.x); //角度を設定
            enemyManager.CreateEnemy(enemyList[0], boneList[1].position, qua, transform); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_2); //効果音を再生
            yield return new WaitForSeconds(1.0f); //少し待機
        }
        yield return new WaitForSeconds(2.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 0; //パターンを変更
        yield return null;
    }
}