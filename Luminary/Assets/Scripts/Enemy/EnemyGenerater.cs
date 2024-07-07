using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerater : MonoBehaviour
{
    [SerializeField]
    private GameObject generateEnemy; //呼び出す敵

    private bool generatedFrag = false; //敵を出したか

    //起動判定に触れた場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (generatedFrag) return; //呼び出し済みなら終了

        //起動判定なら
        if(collision.CompareTag("GeneratorActiveArea")) {
            Instantiate(generateEnemy, transform.position, transform.rotation); //敵を生成
            generatedFrag = true; //呼び出しフラグをtrueに
        }
    }

    //再起動不可判定から離れた場合
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!generatedFrag) return; //呼び出し済みでなければ終了

        //再起動不可判定なら
        //起動判定なら
        if (collision.CompareTag("GeneratorStopArea"))
        {
            generatedFrag = false; //呼び出しフラグをfalseに
        }
    }
}
