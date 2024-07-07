using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̏�Œe������
public class Erhei1 : AbstructEnemy
{
    static private readonly int attackAnimHash = Animator.StringToHash("Erhei1Attack"); //�U������A�j���[�V�����̃n�b�V��
    static private readonly int attackEndAnimHash = Animator.StringToHash("Erhei1AttackEnd"); //�U������߂�A�j���[�V�����̃n�b�V��
    static private readonly int defeatAnimHash = Animator.StringToHash("Erhei1Defeat"); //���j���A�j���[�V�����̃n�b�V��

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector2(-1.0f, 1.0f); //������ݒ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�ҋ@��ԂȂ�
        if(status == 0)
        {
            //���@�̈ʒu�ɍ��킹�Č�����ݒ�
            float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
            transform.localScale = new Vector2(lScale, 1.0f);
        }

        //�R���[�`�����I����Ă���Ȃ�
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Routine()); //�R���[�`�����ĊJ
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        ySpeed = Mathf.Clamp(ySpeed, -12.0f, 12.0f); //�����A�㏸���x�̌��E��ݒ�
    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //���j����Ă����Ȃ�
        if(status == 0)
        {
            animator.Play(defeatAnimHash); //�A�j���[�V�������Đ�
            xSpeed = -12.0f * transform.localScale.x; //���ɐ������
            status = 1; //��Ԃ�ύX
        }

        //��莞�Ԍo������
        if (0.2f <= time)
        {
            //�G�t�F�N�g�𔭐�
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //�G������
        }
    }

    //����p�̃R���[�`��
    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(3.0f); //���΂炭�ҋ@
        animator.Play(attackAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        status = 1; //��Ԃ�ύX
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        animator.Play(attackEndAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        status = 0; //��Ԃ�ύX
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}
