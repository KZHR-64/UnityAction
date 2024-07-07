using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���@�_���̒e
public class LockOnBullet : AbstructBullet
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //�e�q�֌W������ꍇ
        if (transform.parent != null)
        {
            transform.parent = null; //�e�q�֌W������
        }
        float angle = Calculation.GetAngle(transform.position, player.transform.position); //���@�Ɍ������Ċp�x��ݒ�
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
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
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, 0.0f); //�p�x��ݒ�
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, qua); //�G�t�F�N�g�𔭐�
        delFlag = true; //�e������
    }
}
