using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W3�{�X�A�A���C�h���S��
public class AlloyDragon1 : AbstructEnemy
{
    static private readonly int standAnimHash = Animator.StringToHash("AlloyDragonStand"); //�ҋ@�A�j���[�V�����̃n�b�V��
    static private readonly int fireAnimHash = Animator.StringToHash("AlloyDragonFire"); //�Ή��U���A�j���[�V�����̃n�b�V��
    static private readonly int tailAnimHash = Animator.StringToHash("AlloyDragonTailShoot"); //�K���U���A�j���[�V�����̃n�b�V��
    static private readonly int wingAnimHash = Animator.StringToHash("AlloyDragonWingShoot"); //���U���A�j���[�V�����̃n�b�V��

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
                    moveCoroutine = StartCoroutine(FireShoot());
                    break;
                case 1:
                    moveCoroutine = StartCoroutine(TailShoot());
                    break;
                case 2:
                    moveCoroutine = StartCoroutine(WingShoot());
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

    //�Ή�����
    private IEnumerator FireShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        int count = 0;
        animator.Play(fireAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 8.0f, -1.0f); // �ړ�
        //�ړ�����܂őҋ@
        while(!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
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
        while (count < 50)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            //2�`4�b�̊ԁA1�b���ƂɃ~�T�C��������
            if(20 <= count && count <= 40 && count % 10 == 0)
            {
                enemyManager.CreateEnemy(enemyList[0], boneList[1].position, boneList[1].rotation); //�e�𐶐�
                eManager.PlaySe(SeNameList.SE_ESHOOT_2); //���ʉ����Đ�
            }
            count++; //�J�E���g�𑝉�
            yield return new WaitForSeconds(0.1f); //�����ҋ@
        }
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 12.0f, -2.0f); // �ړ�
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
        yield return new WaitForSeconds(3.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 1; //�p�^�[����ύX
        yield return null;
    }

    //�K���̏e
    private IEnumerator TailShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(tailAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, 3.0f); // �ړ�
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
        for (int i = 0; i < 2; i++)
        {
            bulletManager.CreateBullet(bulletList[1], boneList[2].position, boneList[2].rotation, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 12.0f, 3.0f); // �ړ�
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
        yield return new WaitForSeconds(3.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 2; //�p�^�[����ύX
        yield return null;
    }

    //���̏e
    private IEnumerator WingShoot()
    {
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        animator.Play(wingAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_L); //���ʉ����Đ�
        Vector3 nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 9.0f, 2.0f); // �ړ�
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
        xSpeed = -16.0f;
        for(int i = 0; i < 6; i++)
        {
            enemyManager.CreateEnemy(enemyList[0], boneList[1].position, boneList[1].rotation); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_2); //���ʉ����Đ�
            yield return new WaitForSeconds(0.2f); //�����ҋ@
        }
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        rBody.MovePosition(new Vector2(12.0f, -2.0f));
        xSpeed = 0.0f; //���x��ݒ�
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        nextPos = Calculation.GetPositionInWindow(cameraManager.transform.position, 7.0f, -2.0f); // �ړ�
        //�ړ�����܂őҋ@
        while (!Calculation.CheckMovePositionInWindow(transform.position, nextPos))
        {
            transform.position = Calculation.MovePositionInWindow(nextPos, transform.position, 4.0f);
            yield return null;
        }
        yield return new WaitForSeconds(3.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 0; //�p�^�[����ύX
        yield return null;
    }
}
