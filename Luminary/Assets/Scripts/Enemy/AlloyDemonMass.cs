using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージ4中ボス、量産型アロイデーモン
public class AlloyDemonMass : AbstructEnemy
{
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyDemonStraightShoot"); //前に撃つアニメーションのハッシュ
    static private readonly int straightShootEndAnimHash = Animator.StringToHash("AlloyDemonStraightShootEnd"); //攻撃をやめるアニメーションのハッシュ
    
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

            moveCoroutine = StartCoroutine(StraightShoot()); //コルーチンを再開
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
        for (int i = 0; i < 3; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, 8.0f); //弾を生成
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
            yield return new WaitForSeconds(0.5f); //少し待機
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
        yield return new WaitForSeconds(1.0f); //少し待機
        moveCoroutine = null; //コルーチンを初期化
        status = 1; //パターンを変更
        yield return null;
    }
}
