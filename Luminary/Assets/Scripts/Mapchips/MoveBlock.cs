using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : AbstructMapchip
{
    [SerializeField]
    private Vector2 moveDistance; //�ړ���
    [SerializeField]
    private float baseSpeed = 0.0f; //1�b�Ԃ̈ړ���

    private bool backFlag = false; //�����Ԃ���
    private Vector2 startPos; //�J�n�ʒu
    private Vector2 backPos; //�܂�Ԃ��ʒu
    //private Vector2 calcVelocity; //�O�t���[������̈ړ���
    private Vector2 lastPos; //�O�t���[���̈ʒu

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        startPos = transform.position; //�J�n�ʒu��ݒ�
        backPos = startPos + moveDistance; //�܂�Ԃ��ʒu��ݒ�
        lastPos = rBody.position; //�O�̈ʒu��ݒ�
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {
        Vector2 destination = backFlag ? startPos : backPos; //�ړI�n��ݒ�

        //�ړI�n�ɋ߂Â�����
        if (Vector2.Distance(transform.position, destination) < 0.05f)
        {
            backFlag = !backFlag; //�����Ԃ��t���O�𔽓]
            destination = backFlag ? startPos : backPos; //�ړI�n��ݒ�
        }
        //�����Ȃ�
        else
        {
            Vector2 nextPos = Vector2.MoveTowards(transform.position, destination, baseSpeed * Time.deltaTime); //���̈ʒu��ݒ�
            rBody.MovePosition(nextPos); //���̈ʒu�Ɉړ�
        }

        plusVelocity = (rBody.position - lastPos) / Time.deltaTime; //�ړ��ʂ�ݒ�
        lastPos = rBody.position; //�O�̈ʒu��ݒ�
    }

    //��ꂽ���̏���
    protected override void BrokenUpdate()
    {

    }
}
