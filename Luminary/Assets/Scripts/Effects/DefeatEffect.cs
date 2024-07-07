using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatEffect : AbstructEffect
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�ŏ��̍X�V
    protected override void FirstUpdate()
    {
        effManager.CreateEffect(EffectNameList.EFFECT_LUMINAL, transform.position, transform.rotation); //�G�t�F�N�g�𐶐�
        rBody.gravityScale = 0.0f; //���΂炭���d�͂�
        SetYSpeed(6.0f); //���x��ݒ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //���΂炭������
        if(0.5f <= time && rBody.gravityScale == 0.0f)
        {
            rBody.gravityScale = 2.0f; //�d�͂��󂯂�悤��
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        //�������x�𒲐�
        if(rBody.velocity.y < -6.0f)
        {
            SetYSpeed(-6.0f);
        }
    }
}
