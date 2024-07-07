using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_������
public class Cannon1 : AbstructEnemy
{
    [SerializeField]
    private Transform cannonTrans = null; //�C��

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
        if(moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Routine()); //�R���[�`�����ĊJ
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        float angle = Calculation.GetSpin(cannonTrans.localEulerAngles.z, shootAngle, 180.0f); //�C������]
        cannonTrans.localEulerAngles = new Vector3(0.0f, 0.0f, angle); //�C���̊p�x��ݒ�
    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
        delFlag = true; //�G������
    }

    //����p�̃R���[�`��
    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(3.0f); //���΂炭�ҋ@
        shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //���@�ւ̊p�x���擾
        shootAngle = Mathf.Clamp(shootAngle, 0.0f, 180.0f); //�p�x�𒲐�
        eManager.PlaySe(SeNameList.SE_EMOVE_M); //���ʉ����Đ�
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        bulletManager.CreateBullet(bulletList[0], boneList[0].position, cannonTrans.rotation, cannonTrans); //�e�𐶐�
        eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}
