using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissEvent : AbstructEvent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //��莞�Ԍo�߂�����
        if(5.0 <= time)
        {
            eveManager.MissFlag = true; //�~�X�����t���O��true��
            Destroy(this); //�I�u�W�F�N�g������
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }

    //�C�x���g�̋N��
    public override void Activate()
    {

    }
}
