using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPoint : MonoBehaviour
{
    [SerializeField]
    private List<AbstructEvent> eventList; //実行するイベント

    private Queue<AbstructEvent> events; //実行するイベント
    private bool hitFlag = false; //自機が触れたか

    private EventManager eveManager = null; //イベント関連のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        //イベントがないなら
        if(eventList.Count <= 0)
        {
            Destroy(gameObject); //オブジェクトを消す
            return;
        }
        //イベントをキューに挿入
        events = new Queue<AbstructEvent>();
        foreach (AbstructEvent item in eventList)
        {
            events.Enqueue(item);
        }
        eventList.Clear(); //Listは破棄
    }

    // Update is called once per frame
    void Update()
    {
        //イベントが終わっているなら
        if (events.Peek().DelFlag)
        {
            //時間を止めるイベントなら
            if (events.Peek().TimeStopFlag)
            {
                eveManager.RestartGame(); //時間停止を解除
            }
            //終わっているイベントを破棄
            Destroy(events.Peek().gameObject);
            events.Dequeue();

            //イベントがすべて終わっているなら
            if (events.Count == 0)
            {
                Destroy(gameObject); //オブジェクトを消す
            }
            //残っているなら
            else
            {
                //時間を止めるイベントなら
                if(events.Peek().TimeStopFlag)
                {
                    eveManager.StopGame(); //時間を停止
                }
                events.Peek().Activate(); //次のイベントを開始
            }
        }
    }

    //最初の設定
    public void FirstSetting(EventManager em)
    {
        eveManager = em;
    }

    //自機が重なった場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitFlag || events.Count == 0) return; //すでに触れているなら終了
        if (collision.CompareTag("Player"))
        {
            hitFlag = true; //触れたフラグをtrueに
            //時間を止めるイベントなら
            if (events.Peek().TimeStopFlag)
            {
                eveManager.StopGame(); //時間を停止
            }
            events.Peek().Activate(); //イベントを開始
        }
    }
}
