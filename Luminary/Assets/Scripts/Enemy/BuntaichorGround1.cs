using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���^�U�R�A�u���^�C�`���[�i���j
public class BuntaichorGround1 : AbstructEnemy
{
    static private readonly int chargeAnimHash = Animator.StringToHash("BuntaichorStingCharge"); //�ːi�O�̗��߃A�j���[�V�����̃n�b�V��
    static private readonly int stingAnimHash = Animator.StringToHash("BuntaichorSting"); //�ːi����A�j���[�V�����̃n�b�V��
    static private readonly int shootAnimHash = Animator.StringToHash("BuntaichorShoot"); //���A�j���[�V�����̃n�b�V��
    static private readonly int standAnimHash = Animator.StringToHash("BuntaichorStand"); //�ҋ@���A�j���[�V�����̃n�b�V��
    static private readonly int DefeatAnimHash = Animator.StringToHash("BuntaichorDefeat"); //���j���A�j���[�V�����̃n�b�V��

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
                moveCoroutine = StartCoroutine(StingAttack()); //�R���[�`�����ĊJ
            }
            else
            {
                moveCoroutine = StartCoroutine(ShootAttack()); //�R���[�`�����ĊJ
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
            animator.Play(DefeatAnimHash); //�A�j���[�V�������Đ�
            xSpeed = 0.0f; //���x��ݒ�
            status = 1; //��Ԃ�ύX
        }

        //��莞�Ԍo������
        if (0.5f <= time)
        {
            //�G�t�F�N�g�𔭐�
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //�G������
        }
    }

    //�ːi
    private IEnumerator StingAttack()
    {
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        animator.Play(chargeAnimHash); //�A�j���[�V�������Đ�
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
        yield return new WaitForSeconds(0.5f); //���΂炭�ҋ@
        animator.Play(stingAnimHash); //�A�j���[�V�������Đ�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        yield return new WaitForSeconds(0.1f); //���΂炭�ҋ@
        xSpeed = 6.0f * transform.localScale.x; //���x��ݒ�
        yield return new WaitForSeconds(0.5f); //���΂炭�ҋ@
        xSpeed = 0.0f; //���x��ݒ�
        yield return new WaitForSeconds(0.5f); //���΂炭�ҋ@
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        status = 1; //��Ԃ�ύX
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }

    //�ˌ�
    private IEnumerator ShootAttack()
    {
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        animator.Play(shootAnimHash); //�A�j���[�V�������Đ�
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
        yield return new WaitForSeconds(0.5f); //���΂炭�ҋ@
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, transform.rotation, 8.0f, transform); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        status = 0; //��Ԃ�ύX
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}
