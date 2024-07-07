using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W1���{�X�A�f�J�G�[�w�C
public class DekaErhei : AbstructEnemy
{
    static private readonly int attackAnimHash = Animator.StringToHash("DekaErhei1ArmAttack"); //�U������A�j���[�V�����̃n�b�V��
    static private readonly int attackEndAnimHash = Animator.StringToHash("DekaErhei1ArmAttackEnd"); //�U������߂�A�j���[�V�����̃n�b�V��
    static private readonly int missileAttackAnimHash = Animator.StringToHash("DekaErhei1Missile"); //�~�T�C���U���̃A�j���[�V�����̃n�b�V��

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector2(-1.0f, 1.0f); //������ݒ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�R���[�`�����I����Ă���Ȃ�
        if (moveCoroutine == null)
        {
            //���@�̈ʒu�ɍ��킹�Č�����ݒ�
            float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
            transform.localScale = new Vector2(lScale, 1.0f);

            //�p�^�[���ɂ���čs����ύX
            if (status == 0)
            {
                moveCoroutine = StartCoroutine(RapidShoot()); //�R���[�`�����ĊJ
            }
            else
            {
                moveCoroutine = StartCoroutine(MissileShoot()); //�R���[�`�����ĊJ
            }
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
        if (status == 0)
        {
            status = 1; //��Ԃ�ύX
        }

        //��莞�Ԍo������
        if (0.1f <= time)
        {
            //�G�t�F�N�g�𔭐�
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //�G������
        }
    }

    //�e��A��
    private IEnumerator RapidShoot()
    {
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        animator.Play(attackAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        for (int i = 0; i < 2; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        animator.Play(attackEndAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 1; //�p�^�[����ύX
        yield return null;
    }

    //�~�T�C����A��
    private IEnumerator MissileShoot()
    {
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        animator.Play(missileAttackAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        for (int i = 0; i < 4; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, (30.0f - (10.0f * i)) * transform.localScale.x); //�p�x��ݒ�
            enemyManager.CreateEnemy(enemyList[0], boneList[1].position, qua, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_2); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }
}