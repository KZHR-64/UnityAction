using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEvent : AbstructEvent
{
    //イベント情報の構造体
    private struct TalkData
    {
        public int type; //種類（0=会話、1=待機）
        public string talk; //会話内容
    }

    [SerializeField]
    private TextAsset talkFile = null; //会話の情報ファイル

    [SerializeField]
    private List<TalkData> talkList; //会話情報
    private int talkNum = 0; //現在の会話
    private bool startFlag = false; //会話を開始するか

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        talkList = new List<TalkData>();
        LoadTalkFile();
    }

    //継承側の更新
    protected override void SubUpdate()
    {
        if (eveManager.WaitFlag) return; //待機中なら終了
        
        //始まっていなければ
        if (!startFlag)
        {
            //しばらくしたら
            if (0.8f < time)
            {
                eveManager.TalkText.enabled = true; //文章を表示
                SetText(); //次の文章を設定
                startFlag = true; //会話を開始
            }
            return;
        }

        //最後に達していなければ
        if (talkNum < talkList.Count)
        {
            //決定が押されたら
            if (eveManager.CheckButtonInput())
            {
                talkNum++; //次の会話を設定
                SetText(); //次の文章を設定
            }
            //最後に達したなら
            if (talkNum == talkList.Count)
            {
                eveManager.TalkText.enabled = false; //文章を非表示に
                eveManager.MsgWindow.CloseWindow(); //会話ウィンドウを非表示に
                time = 0.0f; //時間を初期化
            }
        }
        //最後に達したなら
        else
        {
            //しばらくしたら
            if (0.8f < time)
            {
                //最後に達しているなら
                if (talkList.Count <= talkNum)
                {
                    delFlag = true; //オブジェクトを消す
                }
                //最後ではないなら
                else
                {
                    eveManager.WaitFlag = true; //待機フラグをtrueに
                }
            }
        }
    }

    //イベントの起動
    public override void Activate()
    {
        eveManager.MsgWindow.OpenWindow(); //会話ウィンドウを表示
        activeFlag = true; //イベントを起動
        time = 0.0f; //時間を初期化
    }

    //会話情報の読み込み
    private void LoadTalkFile()
    {
        string[] row = talkFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 0; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //カンマで区切る
            string talkStr = ""; //会話内容
            TalkData td = new TalkData();
            td.type = int.Parse(col[0]); //種類を設定
            if(0 < col[1].Length)
            {
                talkStr = string.Concat(talkStr, col[1].TrimEnd(), '\n'); //内容を設定
            }
            td.talk = string.Concat(talkStr, col[2].TrimEnd()); //内容を設定
            talkList.Add(td); //情報をリストに追加
        }
    }

    //次の文章を設定
    private void SetText()
    {
        if(talkNum == talkList.Count) return; //最後に達していたら終了
        eveManager.TalkText.text = talkList[talkNum].talk; //文章を設定

    }
}
