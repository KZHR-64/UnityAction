using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    static public readonly string SAVE_FILE_NAME = "SaveData.json"; //セーブデータ名

    static public readonly float FRAME_RATE = 60.0f; //フレームレート
    static public readonly float VOLUME_DEFAULT = 0.3f; //デフォルトの音量
    static public readonly int PLAYER_HP_MAX = 6; //自機のHPの最大値
    static public readonly int HEAL_EN_MAX = 5; //回復エネルギーの最大値

    //2点間の角度を取得
    static public float GetAngle(Vector3 pos1, Vector3 pos2, bool frontOnly = false, float xScare = 1.0f, float rotRange = 0.0f)
    {
        float disX = pos2.x - pos1.x; //x方向の距離
        float disY = pos2.y - pos1.y; //y方向の距離
        float rot = Mathf.Atan2(disY, disX); //角度を計算
        rot *= Mathf.Rad2Deg;

        //前のみに撃つなら
        if (frontOnly)
        {
            float limit = (xScare < 0) ? -180.0f : 0.0f; // 向きによって角度を設定
            rot = Mathf.Clamp(rot, limit - rotRange, limit + rotRange); //角度を調整
        }

        return rot; // 角度を返す
    }

    //少しずつ回転させる
    static public float GetSpin(float objAngle, float targetAngle, float spinSpeed)
    {
        float rot = Mathf.MoveTowardsAngle(objAngle, targetAngle, spinSpeed * Time.deltaTime); // 回転した角度を計算
        return rot; //角度を返す
    }

    //カメラを中心に位置を取得
    static public Vector3 GetPositionInWindow(Vector3 basePos, float targetX, float targetY)
    {
        Vector3 pos = new Vector3(basePos.x + targetX, basePos.y + targetY, basePos.z);
        return pos;
    }

    //カメラを中心に位置を移動
    static public Vector3 MovePositionInWindow(Vector3 targetPos, Vector3 startPos, float speed)
    {
        Vector3 nextPos = Vector3.Lerp(startPos, targetPos, speed * Time.deltaTime);
        return nextPos;
    }

    //カメラを中心に位置を移動したか取得
    static public bool CheckMovePositionInWindow(Vector3 targetPos, Vector3 presentPos)
    {
        return Mathf.Abs(Vector3.Distance(presentPos, targetPos)) < 0.01f;
    }
}
