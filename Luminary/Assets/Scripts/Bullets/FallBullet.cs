using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��x��ʊO�ɏo�Ă��玩�@�Ɍ������ė�����e
public class FallBullet : AbstructBullet
{
    float firstSpeed = 0.0f; //�ŏ��ɐݒ肵�����x
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //�G�̌����ɉ����đ��x��ݒ�
        if (transform.parent != null)
        {
            causer = transform.parent; //�e��Transform���擾
            transform.parent = null; //�e�q�֌W������
            transform.localScale = causer.localScale; //������ݒ�
        }
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f); //�p�x�͌Œ�
        firstSpeed = baseSpeed; //�ŏ��̑��x��ݒ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�R���[�`�����I����Ă���Ȃ�
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Routine()); //�R���[�`�����ĊJ
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        SetSpeed(); //���x��ݒ�
    }

    //������Ƃ��̍X�V
    protected override void EraseUpdate()
    {
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, 0.0f); //�p�x��ݒ�
        effManager.CreateEffect(EffectNameList.EFFECT_BULLET_HIT_1, transform.position, qua, null, 2.0f); //�G�t�F�N�g�𔭐�
        delFlag = true; //�e������
    }

    //����p�̃R���[�`��
    private IEnumerator Routine()
    {
        //��ʊO�ɏo��܂őҋ@
        while(!outCameraFlag)
        {
            yield return null;
        }
        SetBaseSpeed(0.0f); //��~
        yield return new WaitForSeconds(1.0f); //���΂炭�ҋ@
        SetPosition(player.transform.position.x, transform.position.y); //���@�̐^��Ɉړ�
        SetBaseSpeed(firstSpeed); //���x��߂�
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f); //�p�x����������
        yield return null;
    }
}