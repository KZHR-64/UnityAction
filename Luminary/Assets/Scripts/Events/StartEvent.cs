using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : AbstructEvent
{
    private Animator startAnimator; //�J�n���̃A�j���[�V����

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startAnimator = GetComponentInChildren<Animator>();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //���ԂɒB������
        if (3.0f <= time)
        {
            delFlag = true; //�I�u�W�F�N�g������
        }
    }

    //�C�x���g�̋N��
    public override void Activate()
    {
        activeFlag = true; //�C�x���g���N��
        time = 0.0f; //���Ԃ�������
        startAnimator.Play("MoveStartEffect"); //�A�j���[�V�������Đ�
    }
}
