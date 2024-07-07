using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W1�{�X�A�A���C�f�[����
public class AlloyDemon1 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyDemonStand"); //�ҋ@�A�j���[�V�����̃n�b�V��
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyDemonStraightShoot"); //�O�Ɍ��A�j���[�V�����̃n�b�V��
    static private readonly int straightShootEndAnimHash = Animator.StringToHash("AlloyDemonStraightShootEnd"); //�U������߂�A�j���[�V�����̃n�b�V��
    static private readonly int fallShootAnimHash = Animator.StringToHash("AlloyDemonFallShoot"); //��Ɍ��A�j���[�V�����̃n�b�V��
    static private readonly int fallShootEndAnimHash = Animator.StringToHash("AlloyDemonFallShootEnd"); //�U������߂�A�j���[�V�����̃n�b�V��
    static private readonly int jumpAnimHash = Animator.StringToHash("AlloyDemonJump"); //�W�����v����A�j���[�V�����̃n�b�V��

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
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
                moveCoroutine = StartCoroutine(StraightShoot()); //�R���[�`�����ĊJ
            }
            else if (status == 1)
            {
                moveCoroutine = StartCoroutine(FallShoot()); //�R���[�`�����ĊJ
            }
            else
            {
                moveCoroutine = StartCoroutine(JumpPress()); //�R���[�`�����ĊJ
            }
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //���j����Ă����Ȃ�
        if (status == 0)
        {
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_L, transform.position, transform.rotation); //�G�t�F�N�g�𔭐�
            status = 1; //��Ԃ�ύX
        }

        //��莞�Ԍo������
        if (5.0f <= time)
        {
            delFlag = true; //�G������
        }
    }

    //�O�Ɍ���
    private IEnumerator StraightShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(straightShootAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
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
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, 6.0f); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        animator.Play(straightShootEndAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
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

    //��Ɍ����A�e���~�点��
    private IEnumerator FallShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(fallShootAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
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
            bulletManager.CreateBullet(bulletList[1], boneList[0].position, transform.rotation); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        animator.Play(fallShootEndAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
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
        status = 2; //�p�^�[����ύX
        yield return null;
    }

    //�W�����v�������Ԃ�
    private IEnumerator JumpPress()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(jumpAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
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
        SetYSpeed(16.0f); //�㏸�J�n
        yMove = true;
        rBody.gravityScale = 0.0f; //�d�͂𖳌���
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        yMove = false;
        rBody.gravityScale = 1.0f; //�d�͂�L����
        float nextX = cameraManager.transform.position.x + (6.0f * transform.localScale.x); //�ʒu��ݒ�
        SetPosition(nextX, transform.position.y);
        //���n����܂őҋ@
        while (!groundFlag)
        {
            yield return null;
        }
        eManager.PlaySe(SeNameList.SE_ELAND_L); //���ʉ����Đ�
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }
}