using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : AbstructBullet
{

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
        //�������Ă���Ȃ�
        if(hitFlag)
        {
            delFlag = true; //�e������
        }
    }
}
