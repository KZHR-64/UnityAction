using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private bool groundFlag = true; //�ڒn���Ă��邩
    private bool triEnter = false; //�n�ʂɐڐG������
    private bool triStay = false; //�n�ʂɐڐG���Ă��邩
    private bool triExit = false; //�n�ʂ��痣�ꂽ��
    private AbstructMapchip groundObj = null; //����Ă���}�b�v�`�b�v

    //���n����̎擾
    public bool GetGround()
    {
        //�n�ʂɐG��Ă���Ȃ�
        if(triEnter || triStay)
        {
            groundFlag = true; //�ڒn�t���O��true��
        }
        //���ꂽ�Ȃ�
        else if(triExit)
        {
            groundFlag = false; //�ڒn�t���O��false��
        }

        //�e�g���K�[��������
        triEnter = false;
        triStay = false;
        triExit = false;

        return groundFlag;
    }

    //����Ă���}�b�v�`�b�v�̑��x���擾
    public Vector2 GetMapchipVelocity()
    {
        //�}�b�v�`�b�v�ɏ���Ă���Ȃ�
        if(groundObj)
        {
            return groundObj.PlusVelocity; //���x��Ԃ�
        }
        return new Vector2(0.0f, 0.0f); //�Ȃ��Ȃ�0��Ԃ�
    }

    //����Ă���}�b�v�`�b�v�̖��C���擾
    public float GetMapchipFriction()
    {
        //�}�b�v�`�b�v�ɏ���Ă���Ȃ�
        if (groundObj)
        {
            return (1.0f - groundObj.Friction) / 10.0f; //���C��Ԃ�
        }
        return 0.0f; //�Ȃ��Ȃ�0��Ԃ�
    }

    //�n�ʂƐڐG�����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            triEnter = true; //�ڐG�����t���O��true��
            //�}�b�v�`�b�v�ɏ�����Ȃ�
            if(collision.GetComponent<AbstructMapchip>())
            {
                groundObj = collision.GetComponent<AbstructMapchip>(); //�}�b�v�`�b�v���擾
            }
        }
    }

    //�n�ʂƐڐG���Ă���ꍇ
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            triStay = true; //�ڐG���Ă���t���O��true��
        }
    }

    //�n�ʂ��痣�ꂽ�ꍇ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            triExit = true; //���ꂽ�t���O��true��
            //�}�b�v�`�b�v�ɏ���Ă�����
            if (groundObj)
            {
                groundObj = null; //�I�u�W�F�N�g��null��
            }
        }
    }
}
