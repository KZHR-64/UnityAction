using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// その場で弾を撃つ
public class Erhei1 : AbstructEnemy
{
    static private readonly int attackAnimHash = Animator.StringToHash("Erhei1Attack"); //攻撃するアニメーションのハッシュ
    static private readonly int attackEndAnimHash = Animator.StringToHash("Erhei1AttackEnd"); //攻撃をやめるアニメーションのハッシュ
    static private readonly int defeatAnimHash = Animator.StringToHash("Erhei1Defeat"); //撃破時アニメーションのハッシュ

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector2(-1.0f, 1.0f); //向きを設定
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        //待機状態なら
        if(status == 0)
        {
            //自機の位置に合わせて向きを設定
            float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
            transform.localScale = new Vector2(lScale, 1.0f);
        }

        //コルーチンが終わっているなら
        if (moveCoroutine == null)
        {
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
        if(status == 0)
        {
            animator.Play(defeatAnimHash); //アニメーションを再生
            xSpeed = -12.0f * transform.localScale.x; //後ろに吹き飛ぶ
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
        yield return new WaitForSeconds(3.0f); //しばらく待機
        animator.Play(attackAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //効果音を再生
        status = 1; //状態を変更
        yield return new WaitForSeconds(1.0f); //しばらく待機
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //弾を生成
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
        yield return new WaitForSeconds(1.0f); //しばらく待機
        animator.Play(attackEndAnimHash); //アニメーションを再生
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //効果音を再生
        yield return new WaitForSeconds(1.0f); //しばらく待機
        status = 0; //状態を変更
        moveCoroutine = null; //コルーチンを初期化

        yield return null;
    }
}
