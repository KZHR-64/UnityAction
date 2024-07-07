using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�~�T�C���i�ǔ��j
public class Missile3 : AbstructEnemy
{
    private float shootAngle = 0.0f; //�i�ފp�x

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
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�J�n���΂炭
        if (time <= 2.0f)
        {
            shootAngle = Calculation.GetAngle(transform.position, player.transform.position); //���@�ւ̊p�x���擾
        }

        //�����ɍ��킹���x��ݒ�
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //���W�A�����擾
        xSpeed = 6.0f * Mathf.Cos(rad) * transform.localScale.x; //���x��ݒ�
        ySpeed = 6.0f * Mathf.Sin(rad) * transform.localScale.x; //���x��ݒ�

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
}