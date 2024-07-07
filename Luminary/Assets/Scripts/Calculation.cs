using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    static public readonly string SAVE_FILE_NAME = "SaveData.json"; //�Z�[�u�f�[�^��

    static public readonly float FRAME_RATE = 60.0f; //�t���[�����[�g
    static public readonly float VOLUME_DEFAULT = 0.3f; //�f�t�H���g�̉���
    static public readonly int PLAYER_HP_MAX = 6; //���@��HP�̍ő�l
    static public readonly int HEAL_EN_MAX = 5; //�񕜃G�l���M�[�̍ő�l

    //2�_�Ԃ̊p�x���擾
    static public float GetAngle(Vector3 pos1, Vector3 pos2, bool frontOnly = false, float xScare = 1.0f, float rotRange = 0.0f)
    {
        float disX = pos2.x - pos1.x; //x�����̋���
        float disY = pos2.y - pos1.y; //y�����̋���
        float rot = Mathf.Atan2(disY, disX); //�p�x���v�Z
        rot *= Mathf.Rad2Deg;

        //�O�݂̂Ɍ��Ȃ�
        if (frontOnly)
        {
            float limit = (xScare < 0) ? -180.0f : 0.0f; // �����ɂ���Ċp�x��ݒ�
            rot = Mathf.Clamp(rot, limit - rotRange, limit + rotRange); //�p�x�𒲐�
        }

        return rot; // �p�x��Ԃ�
    }

    //��������]������
    static public float GetSpin(float objAngle, float targetAngle, float spinSpeed)
    {
        float rot = Mathf.MoveTowardsAngle(objAngle, targetAngle, spinSpeed * Time.deltaTime); // ��]�����p�x���v�Z
        return rot; //�p�x��Ԃ�
    }

    //�J�����𒆐S�Ɉʒu���擾
    static public Vector3 GetPositionInWindow(Vector3 basePos, float targetX, float targetY)
    {
        Vector3 pos = new Vector3(basePos.x + targetX, basePos.y + targetY, basePos.z);
        return pos;
    }

    //�J�����𒆐S�Ɉʒu���ړ�
    static public Vector3 MovePositionInWindow(Vector3 targetPos, Vector3 startPos, float speed)
    {
        Vector3 nextPos = Vector3.Lerp(startPos, targetPos, speed * Time.deltaTime);
        return nextPos;
    }

    //�J�����𒆐S�Ɉʒu���ړ��������擾
    static public bool CheckMovePositionInWindow(Vector3 targetPos, Vector3 presentPos)
    {
        return Mathf.Abs(Vector3.Distance(presentPos, targetPos)) < 0.01f;
    }
}
