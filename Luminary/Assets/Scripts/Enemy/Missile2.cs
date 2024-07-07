using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�~�T�C���i�_���Ă��璼�i�j
public class Missile2 : AbstructEnemy
{
    private float shootAngle = 0.0f; //�i�ފp�x
    private float baseSpeed = 0.0f; //��ƂȂ鑬�x

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        yMove = true;
        if (transform.parent != null)
        {
            Transform causer = transform.parent; //�e��Transform���擾
            transform.parent = null; //�e�q�֌W������
            transform.localScale = causer.localScale; //������ݒ�
        }
        shootAngle = transform.localEulerAngles.z; //�ŏ��̊p�x��ݒ�
        moveCoroutine = StartCoroutine(Routine()); //�R���[�`�����J�n
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�����ɍ��킹���x��ݒ�
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //���W�A�����擾
        xSpeed = baseSpeed * Mathf.Cos(rad) * transform.localScale.x; //���x��ݒ�
        ySpeed = baseSpeed * Mathf.Sin(rad) * transform.localScale.x; //���x��ݒ�

        //�����ɓ��������ꍇ
        if (hitFlag)
        {
            defeatFlag = true; //���ꂽ���Ƃ�
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        float angle = Calculation.GetSpin(transform.localEulerAngles.z, shootAngle, 360.0f); //��]
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle); //�p�x��ݒ�
    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_S, transform.position, transform.rotation);
        delFlag = true; //�G������
    }

    //����p�̃R���[�`��
    private IEnumerator Routine()
    {
        baseSpeed = 8.0f; //���x��ݒ�
        yield return new WaitForSeconds(0.1f); //���΂炭�ҋ@
        baseSpeed = 0.5f; //���x��ݒ�
        shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //���@�ւ̊p�x���擾
        yield return new WaitForSeconds(0.5f); //���΂炭�ҋ@
        baseSpeed = 8.0f; //���x��ݒ�

        moveCoroutine = null; //�R���[�`����������

        yield return null;
    }
}