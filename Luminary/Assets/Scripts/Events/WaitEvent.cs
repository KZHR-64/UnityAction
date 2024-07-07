using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEvent : AbstructEvent
{
    [SerializeField]
    private float waitTimeMax = -1.0f; //�҂�����

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //���ԂɒB������
        if(0.0f <= waitTimeMax && waitTimeMax <= time)
        {
            delFlag = true; //�I�u�W�F�N�g������
        }

        //�{�X��I����҂��Ă���Ȃ�
        if(waitTimeMax < 0.0f)
        {
            //�{�X�킪�I����Ă�����
            if (!eveManager.WaitFlag)
            {
                delFlag = true; //�I�u�W�F�N�g������
            }
        }
    }

    //�C�x���g�̋N��
    public override void Activate()
    {
        activeFlag = true; //�C�x���g���N��
        time = 0.0f; //���Ԃ�������

        //�{�X��I����҂Ȃ�
        if (waitTimeMax < 0.0f)
        {
            eveManager.WaitFlag = true; //�ҋ@�t���O��true��
        }
    }
}
