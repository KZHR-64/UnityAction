using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private bool groundFlag = true; //接地しているか
    private bool triEnter = false; //地面に接触したか
    private bool triStay = false; //地面に接触しているか
    private bool triExit = false; //地面から離れたか
    private AbstructMapchip groundObj = null; //乗っているマップチップ

    //着地判定の取得
    public bool GetGround()
    {
        //地面に触れているなら
        if(triEnter || triStay)
        {
            groundFlag = true; //接地フラグをtrueに
        }
        //離れたなら
        else if(triExit)
        {
            groundFlag = false; //接地フラグをfalseに
        }

        //各トリガーを初期化
        triEnter = false;
        triStay = false;
        triExit = false;

        return groundFlag;
    }

    //乗っているマップチップの速度を取得
    public Vector2 GetMapchipVelocity()
    {
        //マップチップに乗っているなら
        if(groundObj)
        {
            return groundObj.PlusVelocity; //速度を返す
        }
        return new Vector2(0.0f, 0.0f); //ないなら0を返す
    }

    //乗っているマップチップの摩擦を取得
    public float GetMapchipFriction()
    {
        //マップチップに乗っているなら
        if (groundObj)
        {
            return (1.0f - groundObj.Friction) / 10.0f; //摩擦を返す
        }
        return 0.0f; //ないなら0を返す
    }

    //地面と接触した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            triEnter = true; //接触したフラグをtrueに
            //マップチップに乗ったなら
            if(collision.GetComponent<AbstructMapchip>())
            {
                groundObj = collision.GetComponent<AbstructMapchip>(); //マップチップを取得
            }
        }
    }

    //地面と接触している場合
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            triStay = true; //接触しているフラグをtrueに
        }
    }

    //地面から離れた場合
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            triExit = true; //離れたフラグをtrueに
            //マップチップに乗っていたら
            if (groundObj)
            {
                groundObj = null; //オブジェクトをnullに
            }
        }
    }
}
