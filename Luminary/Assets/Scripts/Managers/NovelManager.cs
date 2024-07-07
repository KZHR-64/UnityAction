using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class NovelManager : MonoBehaviour
{
    //会話情報の構造体
    private struct RollDataStruct
    {
        public int type; //種類（0=会話、1=待機）
        public int[] imageNum; //立ち絵の番号
        public bool blackOut; //暗転させるか
        public string backName; //変更する背景
        public string soundEffect; //効果音
        public string name; //話者名
        public string talk; //会話内容
    }

    [SerializeField]
    private Image namePanel = null; //名前表示用のオブジェクト
    [SerializeField]
    private TextMeshProUGUI nameText = null; //表示する名前用のオブジェクト
    [SerializeField]
    private TextMeshProUGUI messageText = null; //表示する文章用のオブジェクト
    [SerializeField]
    private Image backImage = null; //背景の画像
    [SerializeField]
    private List<Image> charaImageList; //立ち絵の画像
    [SerializeField]
    private List<Sprite> charaSpriteList; //立ち絵の画像

    private Animator animator;
    private TextAsset txtFile = null; //表示する文章ファイル
    private string bgmName = null; //再生するBGM
    private string nextSceneName = null; //次のシーン名
    private List<RollDataStruct> rolls; //表示する文章
    private bool rollFlag = false; //文章を進めているか
    private bool blackOutFlag = false; //暗転しているか
    private int rollNum = 0; //表示する文章の番号
    private float time = 0.0f; //タイマー
    private AsyncOperationHandle<Sprite> handle; //BGMのハンドル

    private SceneController sceneController = null; //シーン管理用のオブジェクト

    private bool inputFlag = false; //決定が押されているか
    private bool skipInputFlag = false; //ポーズが押されているか

    private bool skipFlag = false; //シーンを移すか

    private readonly float talkSpeed = 0.05f; //文章の表示スピード

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        AsyncOperationHandle <TextAsset> ta = Addressables.LoadAssetAsync<TextAsset>(sceneController.SaveData.NovelFileName); //ファイルをロード
        txtFile = ta.WaitForCompletion(); //ファイルを取得
        LoadFile(); //ファイルの読み込み
        Addressables.Release(ta); //ハンドルを解放

        //名前を非表示
        namePanel.enabled = false;
        nameText.enabled = false;

        //立ち絵を非表示
        for (int i = 0; i < charaImageList.Count; i++)
        {
            charaImageList[i].enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        messageText.text = "";
        
        //BGMがあるなら
        if(0 < bgmName.Length)
        {
            sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
        }

        SetRoll(); //次の文章を設定
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFlag) return; //次に移るなら飛ばす

        //入力の確認
        inputFlag = sceneController.InputActions["Jump"].triggered;
        skipInputFlag = sceneController.InputActions["Pause"].triggered;

        //スキップするなら
        if(skipInputFlag)
        {
            skipFlag = true; //次に移るフラグをtrueに
        }

        //最後に達していなければ
        if (rollNum < rolls.Count && !rollFlag)
        {
            //決定が押されたら
            if (inputFlag)
            {
                sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //効果音を再生
                rollNum++; //次の会話を設定
                //最後に達したなら
                if (rollNum == rolls.Count)
                {
                    skipFlag = true; //次に移るフラグをtrueに
                }
                else
                {
                    SetRoll(); //次の文章を設定
                }
            }
        }

        //シーン移動するなら
        if (skipFlag)
        {
            sceneController.SetNextScene(nextSceneName); //タイトルに移動
        }
    }

    //次の文章を設定
    private void SetRoll()
    {
        //立ち絵を設定
        for(int i = 0; i < charaImageList.Count; i++)
        {
            //非表示設定なら
            if(rolls[rollNum].imageNum[i] == -2)
            {
                charaImageList[i].enabled = false; //画像を非表示
            }
            //そうではなく0以上なら
            else if(0 <= rolls[rollNum].imageNum[i])
            {
                charaImageList[i].enabled = true; //画像を表示
                charaImageList[i].sprite = charaSpriteList[rolls[rollNum].imageNum[i]]; //画像を設定
            }
        }

        //効果音があるなら
        if(0 < rolls[rollNum].soundEffect.Length)
        {
            sceneController.SoundEffectManager.PlaySe(rolls[rollNum].soundEffect); //効果音を再生
        }

        //名前があるなら
        if (0 < rolls[rollNum].name.Length)
        {
            //名前を表示
            nameText.text = rolls[rollNum].name;
            namePanel.enabled = true;
            nameText.enabled = true;
        }
        //ないなら
        else
        {
            //名前を非表示
            namePanel.enabled = false;
            nameText.enabled = false;
        }

        StartCoroutine(RollText(rolls[rollNum].talk, rolls[rollNum].blackOut, rolls[rollNum].backName)); //文章の表示開始
    }

    //ファイルのロード
    private void LoadFile()
    {
        rolls = new List<RollDataStruct>();
        string[] row = txtFile.text.Split('\n'); //行ごとに分割
        bgmName = row[0].TrimEnd(); //BGMを設定
        handle = Addressables.LoadAssetAsync<Sprite>(row[1].TrimEnd()); //背景をロード
        backImage.sprite = handle.WaitForCompletion(); //背景を設定
        nextSceneName = row[2].TrimEnd(); //次のシーンを設定
        //一行ずつ読み込み
        for (int i = 3; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //カンマで区切る
            RollDataStruct rollData = new RollDataStruct();
            rollData.imageNum = new int[3];
            rollData.type = int.Parse(col[0]); //種類を設定
            rollData.imageNum[0] = int.Parse(col[1]); //画像番号を設定
            rollData.imageNum[1] = int.Parse(col[2]); //画像番号を設定
            rollData.imageNum[2] = int.Parse(col[3]); //画像番号を設定
            rollData.blackOut = col[4].Contains("1"); //暗転を切り替えるか設定
            rollData.backName = col[5]; //切り替える背景を設定
            rollData.soundEffect = col[6]; //効果音を設定
            rollData.name = col[7].TrimEnd(); //内容を設定
            rollData.talk = col[8].TrimEnd(); //内容を設定
            rolls.Add(rollData); //情報をリストに追加
        }
    }

    //文章を少しずつ表示する
    private IEnumerator RollText(string roll, bool boFlag, string backName)
    {
        rollFlag = true; //進めているフラグをtrueに
        time = 0.0f; //時間を初期化
        messageText.text = ""; //テキストを初期化
        //背景を変えるなら
        if(0 < backName.Length)
        {
            backImage.sprite = null; //背景を削除
            Addressables.Release(handle); //背景のハンドルを解放
            handle = Addressables.LoadAssetAsync<Sprite>(backName.TrimEnd()); //背景をロード
            backImage.sprite = handle.WaitForCompletion(); //背景を設定
        }

        //暗転を切り替えるなら
        if(boFlag)
        {
            string animName = blackOutFlag ? "NovelFadeIn" : "NovelFadeOut"; //暗転状況によってアニメーションを設定
            animator.Play(animName); //表示アニメーションを再生
            yield return null;
            //アニメーションが終わるまで待機
            while (true)
            {
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                if (1.0f <= currentState.normalizedTime)
                {
                    break;
                }
                yield return null;
            }
            blackOutFlag = !blackOutFlag; //暗転フラグを切り替え
        }

        yield return new WaitForSeconds(0.1f); //少し待つ

        //文章がないなら
        if(roll.Length <= 0)
        {
            rollFlag = false; //進めているフラグをfalseに
            time = 0.0f; //時間を初期化
            yield break; //終了
        }

        while (true)
        {
            time += Time.deltaTime; //時間を増加

            if (inputFlag || skipInputFlag) break; //入力があったら終了

            int strLength = Mathf.FloorToInt(time / talkSpeed); //時間に応じて表示する文章量を設定
            if (roll.Length <= strLength) break;//最大に達したら終了
            messageText.text = roll.Substring(0, strLength); //表示する文を設定

            yield return null;
        }
        rollFlag = false; //進めているフラグをfalseに
        time = 0.0f; //時間を初期化
        messageText.text = roll; //全文を表示
        yield return null;
    }
}
