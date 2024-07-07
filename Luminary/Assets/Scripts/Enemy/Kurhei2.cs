using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �N�[�w�C�A���̏�Ń~�T�C��������
public class Kurhei2 : AbstructEnemy
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector2(-1.0f, 1.0f); //������ݒ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //���@�̈ʒu�ɍ��킹�Č�����ݒ�
        float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
        transform.localScale = new Vector2(lScale, 1.0f);

        //�R���[�`�����I����Ă���Ȃ�
        if (moveCoroutine == null)
        {
            time = 0.0f; //���Ԃ�������
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
        if (status == 0)
        {
            rBody.gravityScale = 0.5f; //�d�͂��󂯂�悤��
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
        yield return new WaitForSeconds(5.0f); //���΂炭�ҋ@
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, -90.0f); //���p�x��ݒ�
        enemyManager.CreateEnemy(enemyList[0], boneList[0].position, rot); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_2); //���ʉ����Đ�
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}
