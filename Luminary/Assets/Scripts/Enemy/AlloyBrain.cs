using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���X�{�X�A�A���C�u���C��
public class AlloyBrain : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyBrainStand"); //�ҋ@�A�j���[�V�����̃n�b�V��
    static private readonly int straightShootAnimHash = Animator.StringToHash("AlloyBrainShoot"); //�O�Ɍ��A�j���[�V�����̃n�b�V��
    static private readonly int rightShootAnimHash = Animator.StringToHash("AlloyBrainRightShoot"); //�E���猂�A�j���[�V�����̃n�b�V��
    static private readonly int leftShootAnimHash = Animator.StringToHash("AlloyBrainLeftShoot"); //�����猂�A�j���[�V�����̃n�b�V��
    static private readonly int chargeAnimHash = Animator.StringToHash("AlloyBrainCharge"); //���߃A�j���[�V�����̃n�b�V��
    static private readonly int strikeAnimHash = Animator.StringToHash("AlloyBrainStrike"); //���ߍU���A�j���[�V�����̃n�b�V��

    private int justStatus = -1; //���O�̍s��

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
            while (status == justStatus)
            {
                status = Random.Range(0, 3); //�p�^�[����ύX
            }
            justStatus = status; //���O�̍s����ݒ�
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
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 3.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        for (int i = 0; i < 4; i++)
        {
            bulletManager.CreateBullet(bulletList[0], transform.position, transform.rotation, 10.0f); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }

    //�e���~�点��
    private IEnumerator FallShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(rightShootAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, -8.0f, 3.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        for (int i = 1; i < 8; i++)
        {
            Vector3 shootPos = new Vector3(transform.position.x + 2.0f + (2.0f * i), transform.position.y + 2.0f, transform.position.z); // �e�̈ʒu��ݒ�
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, -90.0f); //�p�x��ݒ�
            bulletManager.CreateBullet(bulletList[1], shootPos, qua, 20.0f); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.2f); //�����ҋ@
        }
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(leftShootAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 8.0f, 3.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        for (int i = 1; i < 8; i++)
        {
            Vector3 shootPos = new Vector3(transform.position.x - 2.0f - (2.0f * i), transform.position.y + 2.0f, transform.position.z); // �e�̈ʒu��ݒ�
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, -90.0f); //�p�x��ݒ�
            bulletManager.CreateBullet(bulletList[1], shootPos, qua, 20.0f); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.2f); //�����ҋ@
        }
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }

    //���߂Ă��猂��
    private IEnumerator ChargeShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(chargeAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, 4.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        animator.Play(strikeAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_ELAND_L); //���ʉ����Đ�
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
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, -90.0f); //�p�x��ݒ�
        bulletManager.CreateBullet(bulletList[2], transform.position, qua, 16.0f); //�e�𐶐�
        yield return new WaitForSeconds(2.0f); //�����ҋ@
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 0.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 6.0f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }
}