using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W5�{�X�A���V�i�Đ�j
public class Senten2 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("SentenStand"); //�ҋ@�A�j���[�V�����̃n�b�V��
    static private readonly int straightShootAnimHash = Animator.StringToHash("SentenShoot"); //�O�Ɍ��A�j���[�V�����̃n�b�V��
    static private readonly int dashAnimHash = Animator.StringToHash("SentenDash"); //�ːi����A�j���[�V�����̃n�b�V��
    static private readonly int backStepAnimHash = Animator.StringToHash("SentenBackStep"); //���ɉ�����A�j���[�V�����̃n�b�V��
    static private readonly int jumpAnimHash = Animator.StringToHash("SentenJump"); //�W�����v����A�j���[�V�����̃n�b�V��

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

            //�R���[�`�����ĊJ
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
        yield return new WaitForSeconds(0.1f); //�����ҋ@
        for (int i = 0; i < 3; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, 16.0f, transform); //�e�𐶐�   
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.1f); //�����ҋ@
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 3; //�p�^�[����ύX
        yield return null;
    }

    //�ːi
    private IEnumerator DashAttack()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(dashAnimHash); //�A�j���[�V�������Đ�
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
        xSpeed = 16.0f * transform.localScale.x; //���x��ݒ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        xSpeed = 0.0f; //���x��ݒ�
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 3; //�p�^�[����ύX
        yield return null;
    }

    //�W�����v
    private IEnumerator JumpPress()
    {
        animator.Play(jumpAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        SetYSpeed(24.0f); //�㏸�J�n
        yMove = true;
        rBody.gravityScale = 0.0f; //�d�͂𖳌���
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        yMove = false;
        rBody.gravityScale = 2.0f; //�d�͂�L����
        float nextX = player.transform.position.x; //���@�̏�Ɉʒu��ݒ�
        SetPosition(nextX, transform.position.y);
        //���n����܂őҋ@
        while (!groundFlag)
        {
            yield return null;
        }
        yield return null;
        eManager.PlaySe(SeNameList.SE_ELAND_L); //���ʉ����Đ�
        //���E�ɒe�𐶐�
        bulletManager.CreateBullet(bulletList[1], transform.position, transform.rotation, 12.0f);
        AbstructBullet ab = bulletManager.CreateBullet(bulletList[1], transform.position, transform.rotation, 12.0f);
        ab.transform.localScale = new Vector2(-1.0f, 1.0f);
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = Random.Range(0, 2); //�p�^�[����ύX
        yield return null;
    }

    //���ɉ�����
    private IEnumerator BackStep()
    {
        animator.Play(backStepAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        xSpeed = -8.0f * transform.localScale.x; //���x��ݒ�
        yield return new WaitForSeconds(0.2f); //�����ҋ@
        xSpeed = 0.0f; //���x��ݒ�
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 2; //�p�^�[����ύX
        yield return null;
    }
}
