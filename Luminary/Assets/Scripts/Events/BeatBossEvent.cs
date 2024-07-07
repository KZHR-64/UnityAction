using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBossEvent : AbstructEvent
{
    private SpriteRenderer beatEffect = null; //�{�X���j���̃G�t�F�N�g

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        beatEffect = GetComponent<SpriteRenderer>();
        beatEffect.enabled = false;
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //��莞�Ԍo�߂�����
        if (1.0f <= time)
        {
            eveManager.RestartGame(); //�ꎞ��~������
            beatEffect.enabled = false; //�G�t�F�N�g���\��
            delFlag = true; //�I�u�W�F�N�g������
            activeFlag = false;
        }
    }

    //�C�x���g�̋N��
    public override void Activate()
    {
        activeFlag = true; //�C�x���g���N��
        eveManager.StopGame(); //�Q�[�����ꎞ��~
        time = 0.0f; //���Ԃ�������
        SoundEffectManager eManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
        eManager.PlaySe("smashHit.ogg"); //���ʉ����Đ�
        beatEffect.enabled = true; //�G�t�F�N�g��\��
    }
}
