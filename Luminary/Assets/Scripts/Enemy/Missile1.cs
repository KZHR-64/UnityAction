using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�~�T�C���i�����O���j
public class Missile1 : AbstructEnemy
{
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
        //�����ɍ��킹���x��ݒ�
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //���W�A�����擾
        xSpeed = 8.0f * Mathf.Cos(rad) * transform.localScale.x; //���x��ݒ�
        ySpeed = 8.0f * Mathf.Sin(rad) * transform.localScale.x; //���x��ݒ�

        //�����ɓ��������ꍇ
        if(hitFlag)
        {
            defeatFlag = true; //���ꂽ���Ƃ�
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_S, transform.position, transform.rotation);
        delFlag = true; //�G������
    }
}