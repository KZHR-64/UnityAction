using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target1 : AbstructEnemy
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

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_HIT, transform.position, transform.rotation);
        delFlag = true; //�G������
    }
}
