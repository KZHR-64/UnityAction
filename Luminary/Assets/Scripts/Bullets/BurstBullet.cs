using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�y�􂷂�e
public class BurstBullet : AbstructBullet
{
    [SerializeField]
    private GameObject spawnBullet = null; //���\��̒e

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //���@�̌����ɉ����đ��x��ݒ�
        if (transform.parent != null)
        {
            causer = transform.parent; //���@��Transform���擾
            transform.parent = null; //�e�q�֌W������
            transform.localScale = causer.localScale; //������ݒ�
        }
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {

    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        SetSpeed(); //���x��ݒ�
    }

    //������Ƃ��̍X�V
    protected override void EraseUpdate()
    {
        for (int i = 0; i < 8; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, 45.0f * i); //�p�x��ݒ�
            bulManager.CreateBullet(spawnBullet, transform.position, qua, baseSpeed);//�e�𐶐�
        }
        Quaternion effQua = Quaternion.Euler(0.0f, 0.0f, 0.0f); //�p�x��ݒ�
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, effQua); //�G�t�F�N�g�𔭐�
        delFlag = true; //�e������
    }
}