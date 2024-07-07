using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�y�􂷂�e
public class SparkBullet : AbstructBullet
{
    private Animator animator = null;
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
        animator = GetComponent<Animator>(); //Animator���擾
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {

    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        //�A�j���[�V�������I�������
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (1.0f <= currentState.normalizedTime)
        {
            delFlag = true; //�G�t�F�N�g������
        }
    }
}