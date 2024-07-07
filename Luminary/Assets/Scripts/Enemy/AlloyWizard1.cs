using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W4�{�X�A�A���C�E�B�U�[�h
public class AlloyWizard1 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyWizardStand"); //�ҋ@�A�j���[�V�����̃n�b�V��
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyWizardStraightShoot"); ///�O�Ɍ��A�j���[�V�����̃n�b�V��
    static private readonly int fallShootAnimHash = Animator.StringToHash("AlloyWizardFallShoot"); ///��Ɍ��A�j���[�V�����̃n�b�V��
    static private readonly int chargeAnimHash = Animator.StringToHash("AlloyWizardCharge"); //���߂�A�j���[�V�����̃n�b�V��
    static private readonly int strikeAnimHash = Animator.StringToHash("AlloyWizardStrike"); //���ߍU���A�j���[�V�����̃n�b�V��

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
            //�R���[�`�����ĊJ
            switch (status)
            {
                case 0:
                    moveCoroutine = StartCoroutine(StraightShoot());
                    break;
                case 1:
                    moveCoroutine = StartCoroutine(FallShoot());
                    break;
                case 2:
                    moveCoroutine = StartCoroutine(ChargeShoot());
                    break;
                default:
                    break;
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
            //�G�t�F�N�g�𔭐�
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_L, transform.position, transform.rotation); //�G�t�F�N�g�𔭐�
            status = 1; //��Ԃ�ύX
        }

        //��莞�Ԍo������
        if (5.0f <= time)
        {
            delFlag = true; //�G������
        }
    }

    //�O�ɒe������
    private IEnumerator StraightShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, 1.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        animator.Play(straightShootAnimHash); //�A�j���[�V�������Đ�
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
        for (int i = 0; i < 5; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z - 60.0f + (30.0f * i)); //�p�x��ݒ�
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, 12.0f, transform); //�e�𐶐�
        }
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, 9.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 1; //�p�^�[����ύX
        yield return null;
    }

    //�ォ��e���~�点��
    private IEnumerator FallShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(fallShootAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
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
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[i % 2].position, transform.rotation, 12.0f); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.5f); //�����ҋ@
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[2], boneList[i % 2].position, transform.rotation); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.5f); //�����ҋ@
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[i % 2].position, transform.rotation, 12.0f); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.5f); //�����ҋ@
        }
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 9.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 2; //�p�^�[����ύX
        yield return null;
    }

    //���߂Ă���e������
    private IEnumerator ChargeShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(chargeAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 8.0f, 3.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
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
        animator.Play(strikeAnimHash); //�A�j���[�V�������Đ�
        yield return null;
        bulletManager.CreateBullet(bulletList[3], boneList[0].position, transform.rotation, 16.0f); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //�����ҋ@
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
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 13.0f, 3.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }
}