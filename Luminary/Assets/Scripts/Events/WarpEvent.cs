using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEvent : AbstructEvent
{
    [SerializeField]
    private string nextMapName = ""; //���̃X�e�[�W��

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //���ԂɒB������
        if (0.5f <= time)
        {
            eveManager.WarpNextMap(nextMapName); //�ړ�����X�e�[�W��ݒ�
            delFlag = true; //�I�u�W�F�N�g������
        }
    }

    //�C�x���g�̋N��
    public override void Activate()
    {
        activeFlag = true; //�C�x���g���N��
        time = 0.0f; //���Ԃ�������
        player.MoveFlag = false; //���@���ꎞ��~
    }
}
