using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigExplodeEffect : AbstructEffect
{
    private float effectTime = 0.0f; //�G�t�F�N�g���ĂԎ���

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�ŏ��̍X�V
    protected override void FirstUpdate()
    {

    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //��莞�Ԃ��Ƃ�
        if(0.3f <= effectTime && time <= delTime - 0.5f)
        {
            //���W��ݒ�
            float plusPosX = Random.Range(-2.0f, 2.0f);
            float plusPosY = Random.Range(-2.0f, 2.0f);
            Vector3 effPos = new Vector3(transform.position.x + plusPosX, transform.position.y - plusPosY, transform.position.z);

            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, effPos, transform.rotation);
            effectTime %= 0.3f; //���Ԃ�߂�
        }
        //�����鎞�ԂȂ�
        if(delTime <= time)
        {
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation, null, 3.0f);
        }
        effectTime += Time.deltaTime; //���Ԃ𑝉�
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }
}
