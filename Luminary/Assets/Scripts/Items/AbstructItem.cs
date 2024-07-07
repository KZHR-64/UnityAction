using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructItem : MonoBehaviour
{
    protected bool delFlag = false; //消去するか
    protected float time = 0.0f; //経過時間

    protected Player player = null; //自機のオブジェクト
    protected SoundEffectManager eManager = null; //効果音関連のオブジェクト

    public bool DelFlag { get { return delFlag; } } //消去するか

    // Start is called before the first frame update
    protected void Start()
    {
        player = FindObjectOfType<Player>(); //オブジェクトを取得
        eManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //消去するなら終了
        time += Time.deltaTime; //時間を増加
    }

    //継承側の更新
    protected virtual void SubUpdate() { }

    //継承側の更新
    protected virtual void SubFixUpdate() { }

    //自機と接触した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ActiveItem(); //取得時の処理を実行
            delFlag = true; //アイテムを消す
        }
    }

    //アイテム獲得時の処理
    protected virtual void ActiveItem() { }
}
