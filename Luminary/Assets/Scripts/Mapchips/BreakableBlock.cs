using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : AbstructMapchip
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {

    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }

    //��ꂽ���̏���
    protected override void BrokenUpdate()
    {
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_S, transform.position, transform.rotation);
        delFlag = true; //�I�u�W�F�N�g������
    }
}
