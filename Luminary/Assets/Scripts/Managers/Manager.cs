using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private PauseManager pauseManager = null; //ポーズ画面のオブジェクト
    [SerializeField]
    private BossLife lifeBar = null; //ボスの体力バーのオブジェクト
    [SerializeField]
    private ItemManager itemManager = null; //アイテム管理のオブジェクト
    [SerializeField]
    private Player player = null; //自機のオブジェクト
    [SerializeField]
    private EventManager eveManager = null; //イベント関連のオブジェクト
    [SerializeField]
    private MapchipManager mapchipManager = null; //マップチップ関連のオブジェクト
    [SerializeField]
    private EnemyManager enemyManager = null; //敵管理のオブジェクト
    [SerializeField]
    private string bgmName = ""; //再生するBGM

    private bool missFlag = false; //ミスしたか
    private bool warpFlag = false; //次のマップに移るか
    private string nextMapName = null; //次のマップの名前
    private bool nextFlag = false; //次のシーンに移るか

    private SceneController sceneController = null; //シーン管理用のオブジェクト

    public Player Player { get{ return player; } } //自機のオブジェクト
    public EventManager EveManager { get { return eveManager; } } //イベント関連のオブジェクト
    public ItemManager ItemManagerObj { get { return itemManager; } } //アイテム管理のオブジェクト

    private void Awake()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        pauseManager.gameObject.SetActive(false); //ポーズ画面を閉じておく
        lifeBar.gameObject.SetActive(false); //体力バーを閉じておく
    }

    // Start is called before the first frame update
    void Start()
    {
        mapchipManager.Manager = this; //オブジェクトを設定
        sceneController = FindObjectOfType<SceneController>(); //オブジェクトを取得
        sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
    }

    // Update is called once per frame
    void Update()
    {
        if (nextFlag) return; //次に移るなら飛ばす
        //ポーズ中なら
        if (pauseManager.gameObject.activeSelf)
        {
            //ステージセレクトに戻るなら
            if (pauseManager.BackFlag)
            {
                nextFlag = true; //次に移るフラグをtrueに
                var sc = FindObjectOfType<SceneController>();
                sc.SetNextScene("StageSelect"); //ステージセレクトに戻る
            }
            return; //飛ばす
        }

        //ゲームを止めるなら
        if (eveManager.GameStopFlag)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        //ポーズが押されたら
        if (sceneController.InputActions["Pause"].triggered && !eveManager.GameStopFlag)
        {
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //効果音を再生
            pauseManager.gameObject.SetActive(true); //ポーズ画面をActiveに
            pauseManager.PauseActivate(); //ポーズ画面を開く
        }

        GameObject[] iList = GameObject.FindGameObjectsWithTag("Item"); //アイテムのリストを取得
        //アイテムがあるなら
        if (0 < iList.Length)
        {
            //アイテムの数繰り返し
            foreach (var it in iList)
            {
                AbstructItem itCom = it.GetComponent<AbstructItem>(); //アイテムのスクリプトを取得
                //弾を消すなら
                if (itCom.DelFlag)
                {
                    Destroy(it); //オブジェクトを消す
                }
            }
        }

        //ボス戦が終わったら
        if (enemyManager.CheckBossBattleEnd())
        {
            //イベント待機中なら
            if (eveManager.WaitFlag)
            {
                eveManager.WaitFlag = false; //待機フラグをfalseに
            }
        }

        //ワープするなら
        if(warpFlag && !nextFlag)
        {
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SaveData.Hp = player.Hp; //自機のHPを引継ぎ
            sceneController.SaveData.HealEnergy = player.HealEnergy; //回復エネルギーを引継ぎ
            sceneController.SetNextScene(nextMapName); //次のマップに移動
        }

        //クリアしたなら
        if(eveManager.ClearFlag && !nextFlag)
        {
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SaveData.CheckClearStage(); //ステージ数更新の確認
            sceneController.SaveData.SetNovelFileName(false); //ファイル名を設定
            sceneController.SetNextScene("NovelPart"); //次のマップに移動
        }

        //ミスしているなら
        if (missFlag && eveManager.MissFlag)
        {
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SaveData.Hp = Calculation.PLAYER_HP_MAX; //自機のHPを初期化
            sceneController.SaveData.HealEnergy = player.HealEnergy; //回復エネルギーを引継ぎ
            sceneController.ReloadScene(); //シーンをリロード
        }

        //自機のHPが0なら
        if (player.Hp <= 0 && !missFlag)
        {
            missFlag = true; //ミスしたフラグをtrueに
            eveManager.ActiveMissEvent(); //ミス時のイベントを発生
        }

        //次に移るなら
        if (nextFlag)
        {
            //var sc = FindObjectOfType<SceneController>();
            //sc.ReloadScene(); //シーンをリロード
        }
    }

    //次のマップを設定
    public void SetNextMap(string nextName)
    {
        if (warpFlag) return; //ワープ準備済みなら終了

        nextMapName = nextName; //次のマップ名を取得
        warpFlag = true; //ワープフラグをtrueに
        //エフェクトを発生
    }
}
