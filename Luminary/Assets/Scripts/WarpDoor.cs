using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDoor : MonoBehaviour
{
    //[SerializeField]
    //private string nextMapName = null; //移動先のマップ名
    //[SerializeField]
    //private bool checkBoss = false; //ボス戦中か参照するか

    //private bool activeFlag = true; //動作するか
    //private bool hitFlag = false; //自機が重なっているか

    //private Manager manager = null; //ゲーム管理のオブジェクト
    //private SoundEffectManager eManager = null; //効果音関連のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        //manager = FindObjectOfType<Manager>(); //オブジェクトを取得
        //eManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
    }

    // Update is called once per frame
    void Update()
    {
        //動作しないなら
        /*if(!activeFlag)
        {
            //ボス戦が終わっているなら
            if(checkBoss && !manager.BossBattleFlag)
            {
                activeFlag = true; //動作フラグをtrueに
                gameObject.GetComponent<Renderer>().enabled = true; //扉を表示
            }
            return; //戻る
        }

        //ボス戦中なら
        if(checkBoss && manager.BossBattleFlag && activeFlag)
        {
            activeFlag = false; //動作フラグをfalseに
            gameObject.GetComponent<Renderer>().enabled = false; //扉を非表示
            return; //戻る
        }

        //自機が重なっているなら
        if (hitFlag && nextMapName != null)
        {
            //上が押されたら
            if (Input.GetButtonDown("Vertical") && 0.0f < Input.GetAxisRaw("Vertical"))
            {
                eManager.PlaySe("warp.ogg"); //効果音を再生
                manager.SetNextMap(nextMapName); //次のマップの移動準備
            }
        }*/
    }

    //自機が重なった場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //hitFlag = true; //重なったフラグをtrueに
        }
    }

    //自機が離れた場合
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //hitFlag = false; //重なったフラグをfalseに
        }
    }
}
