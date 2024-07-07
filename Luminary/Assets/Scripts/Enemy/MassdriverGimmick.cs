using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassdriverGimmick : AbstructEnemy
{
    private float shootAngle = 0.0f; //���p�x

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
            time = 0.0f; //���Ԃ�������
            moveCoroutine = StartCoroutine(Routine()); //�R���[�`�����ĊJ
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        float angle = Calculation.GetSpin(transform.localEulerAngles.z, shootAngle, 180.0f); //�C������]
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle); //�C���̊p�x��ݒ�
    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_HIT, transform.position, transform.rotation);
        delFlag = true; //�G������
    }

    //����p�̃R���[�`��
    private IEnumerator Routine()
    {
        //���΂炭�͏Ə����킹
        while (time < 4.0f)
        {
            shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //���@�ւ̊p�x���擾
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        bulletManager.CreateBullet(bulletList[0], transform.position, transform.rotation, transform); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}