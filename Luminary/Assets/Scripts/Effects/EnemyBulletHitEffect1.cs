using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHitEffect1 : AbstructEffect
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�ŏ��̍X�V
    protected override void FirstUpdate()
    {
        eManager.PlaySe(SeNameList.SE_EXPLODE_M); //���ʉ����Đ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�A�j���[�V�������I�������
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (1.0f <= currentState.normalizedTime)
        {
            delFlag = true; //�G�t�F�N�g������
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }
}
