using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEvent : AbstructEvent
{
    private Animator clearAnimator = null; //�N���A���̃G�t�F�N�g

    private bool effectFlag = false; //�G�t�F�N�g���o������

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        clearAnimator = GetComponent<Animator>();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //��莞�Ԍo�߂�����
        if(1.0f <= time && !effectFlag)
        {
            soundEffectManager.PlaySe("shakin.ogg"); //���ʉ����Đ�
            clearAnimator.Play("MoveClearEffect"); //�N���A���̃A�j���[�V�������Đ�
            effectFlag = true; //�G�t�F�N�g���o�����t���O��true��
        }
        if (6.0f <= time)
        {
            eveManager.ClearFlag = true; //�N���A�����t���O��true��
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }

    //�C�x���g�̋N��
    public override void Activate()
    {
        activeFlag = true; //�C�x���g���N��
        soundManager.StopBgm(); //BGM���~
        //eveManager.GameStopFlag = true; //�Q�[�����ꎞ��~
        time = 0.0f; //���Ԃ�������
    }
}
