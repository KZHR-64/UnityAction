using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private MessageWindow msgWindow = null; //メッセージウインドウ
    [SerializeField]
    private TextMeshProUGUI talkText = null; //表示する文章用のオブジェクト
    [SerializeField]
    private MissEvent missEvent = null; //ミスした時のイベント
    [SerializeField]
    private BeatBossEvent beatBossEvent = null; //ボス撃破時のイベント
    [SerializeField]
    private Manager manager = null; //ゲーム管理のオブジェクト
    [SerializeField]
    private Player player = null; //自機のオブジェクト

    private SceneController sceneController = null; //シーン管理用のオブジェクト
    private SoundManager soundManager = null; //BGM関連のオブジェクト
    private SoundEffectManager soundEffectManager = null; //効果音関連のオブジェクト
    private EffectManager effManager = null; //エフェクト関連のオブジェクト

    private bool clearFlag = false; //クリアしたか
    private bool missFlag = false; //ミスしたか
    private bool waitFlag = false; //待機するか
    private bool gameStopFlag = false; //ゲーム進行を止めるか
    private int gameStopNum = 0; //ゲームを止めているイベント数

    public bool ClearFlag { get { return clearFlag; } set { clearFlag = value; } } //クリアしたか
    public bool MissFlag { get { return missFlag; } set { missFlag = value; } } //ミスしたか
    public bool WaitFlag { get { return waitFlag; } set { waitFlag = value; } } //待機するか
    public bool GameStopFlag { get { return gameStopFlag; }} //待機するか
    public MessageWindow MsgWindow { get { return msgWindow; } } //メッセージウインドウ
    public TextMeshProUGUI TalkText { get { return talkText; } } //表示する文章用のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>(); //オブジェクトを取得
        soundManager = sceneController.SoundManager; //オブジェクトを取得
        soundEffectManager = sceneController.SoundEffectManager; //オブジェクトを取得
        effManager = sceneController.EffectManager; //オブジェクトを取得

        AbstructEvent[] eventList = GetComponentsInChildren<AbstructEvent>(); //すでにあるイベントを取得
        //イベントがあるなら
        if(0 < eventList.Length)
        {
            foreach(AbstructEvent ae in eventList)
            {
                ae.FirstSetting(this, player, soundManager, soundEffectManager); //最初の更新
            }
        }
        EventPoint[] eventPoint = GetComponentsInChildren<EventPoint>();
        //イベントがあるなら
        if (0 < eventPoint.Length)
        {
            foreach (EventPoint ep in eventPoint)
            {
                ep.FirstSetting(this); //最初の更新
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ミスしたイベントを開始
    public void ActiveMissEvent()
    {
        missEvent.ActiveFlag = true; //イベントを開始
    }

    //ボス撃破イベントを開始
    public void ActiveBeatBossEvent()
    {
        beatBossEvent.Activate();
    }

    //入力を確認
    public bool CheckButtonInput()
    {
        //ジャンプか攻撃ボタンが押されたことを確認
        bool jumpInput = sceneController.PlayerInput.actions["Jump"].triggered;
        bool fireInput = sceneController.PlayerInput.actions["Fire"].triggered;
        bool fire2Input = sceneController.PlayerInput.actions["Fire2"].triggered;
        return jumpInput || fireInput || fire2Input;
    }

    //次のステージに移動
    public void WarpNextMap(string mapName)
    {
        soundEffectManager.PlaySe("warp.ogg"); //効果音を再生
        manager.SetNextMap(mapName); //次のマップの移動準備
    }

    //ゲームの進行を停止
    public void StopGame()
    {
        gameStopNum++; //停止しているイベント数を増加
        gameStopFlag = true; //停止フラグをtrueに
    }

    //ゲームを再開
    public void RestartGame()
    {
        gameStopNum--; //停止数を減少
        gameStopNum = Mathf.Max(0, gameStopNum);

        //停止数が0になったら
        if(gameStopNum <= 0)
        {
            gameStopFlag = false; //停止フラグをfalseに
        }
    }
}
