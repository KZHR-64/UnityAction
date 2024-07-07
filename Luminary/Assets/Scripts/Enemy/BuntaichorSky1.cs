using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 中型ザコ、ブンタイチョー（空）
public class BuntaichorSky1 : AbstructEnemy
{
    static private readonly int shootAnimHash = Animator.StringToHash("BuntaichorShoot"); //撃つアニメーションのハッシュ
    static private readonly int standAnimHash = Animator.StringToHash("BuntaichorStand"); //待機時アニメーションのハッシュ
    static private readonly int DefeatAnimHash = Animator.StringToHash("BuntaichorDefeat"); //撃破時アニメーションのハッシュ

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

            moveCoroutine = StartCoroutine(ShootAttack()); //コルーチンを再開
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
            animator.Play(DefeatAnimHash); //アニメーションを再生
            rBody.gravityScale = 0.2f; //重力を受けるように
            xSpeed = 0.0f; //速度を設定
            status = 1; //状態を変更
        }

        //一定時間経ったら
        if (0.5f <= time)
        {
            //エフェクトを発生
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //敵を消す
        }
    }

    //射撃
    private IEnumerator ShootAttack()
    {
        yield return new WaitForSeconds(2.0f); //しばらく待機
        animator.Play(shootAnimHash); //アニメーションを再生
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
        yield return new WaitForSeconds(0.5f); //しばらく待機
        float zAngle = Calculation.GetAngle(boneList[0].position, player.transform.position, true, transform.localScale.x, 30.0f); //自機への角度を取得
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, zAngle); //角度を設定
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, 8.0f); //弾を生成
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //効果音を再生
        yield return new WaitForSeconds(2.0f); //しばらく待機
        animator.Play(standAnimHash); //アニメーションを再生
        status = 0; //状態を変更
        moveCoroutine = null; //コルーチンを初期化

        yield return null;
    }
}
