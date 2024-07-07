using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���E�ɕ��􂷂�e
public class SpreadBullet : AbstructBullet
{
    [SerializeField]
    private GameObject spawnBullet = null; //���\��̒e

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //�G�̌����ɉ����đ��x��ݒ�
        if (transform.parent != null)
        {
            causer = transform.parent; //�e��Transform���擾
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
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, 0.0f); //�p�x��ݒ�
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, qua); //�G�t�F�N�g�𔭐�
        //���E�ɒe�𐶐�
        bulManager.CreateBullet(spawnBullet, transform.position, qua);
        AbstructBullet ab = bulManager.CreateBullet(spawnBullet, transform.position, qua);
        ab.transform.localScale = new Vector2(-1.0f, 1.0f);
        delFlag = true; //�e������
    }
}