using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���^�U�R�A�u���^�C�`���[�i��j
public class BuntaichorSky1 : AbstructEnemy
{
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

            moveCoroutine = StartCoroutine(ShootAttack()); //�R���[�`�����ĊJ
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
            animator.Play(DefeatAnimHash); //�A�j���[�V�������Đ�
            rBody.gravityScale = 0.2f; //�d�͂��󂯂�悤��
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

    //�ˌ�
    private IEnumerator ShootAttack()
    {
        yield return new WaitForSeconds(2.0f); //���΂炭�ҋ@
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
        float zAngle = Calculation.GetAngle(boneList[0].position, player.transform.position, true, transform.localScale.x, 30.0f); //���@�ւ̊p�x���擾
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, zAngle); //�p�x��ݒ�
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, 8.0f); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        yield return new WaitForSeconds(2.0f); //���΂炭�ҋ@
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        status = 0; //��Ԃ�ύX
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}
