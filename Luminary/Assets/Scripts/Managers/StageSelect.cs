using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSelect : MonoBehaviour
{
    //ステージ情報の構造体
    private struct StageDataStruct
    {
        public bool playable; //選べるか
        public string subTitle; //サブタイトル
        public string novelName; //読み込むファイル名
    }

    [SerializeField]
    private TextAsset txtFile = null; //サブタイトルファイル
    [SerializeField]
    private List<TextMeshProUGUI> titleText = null; //表示する文章用のオブジェクト
    [SerializeField]
    private string bgmName = "tukito_search_for_ideals.ogg"; //再生するBGM

    private List<StageDataStruct> stagedata; //ステージ情報

    private SceneController sceneController = null; //シーン管理用のオブジェクト

    private bool nextFlag = false; //次のシーンに移るか
    private int cursor = 0; //カーソルの位置

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        sceneController = FindObjectOfType<SceneController>(); //オブジェクトを取得
        stagedata = new List<StageDataStruct>();
        LoadFile(); //ファイルをロード
        sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
    }

    // Update is called once per frame
    void Update()
    {
        if (nextFlag) return; //次に移るなら飛ばす

        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();
        //上下の入力があったら
        if (sceneController.InputActions["Move"].triggered)
        {
            //上が押されたら
            if (0.0f < stickInput.y)
            {
                cursor += (stagedata.Count - 1); //カーソルを上に
            }
            //下が押されたら
            else if (stickInput.y < 0.0f)
            {
                cursor++; //カーソルを下に
            }
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //効果音を再生
        }

        cursor %= stagedata.Count; //カーソルを調整

        //上下のサブタイトルを設定
        int cursor0 = (cursor + (stagedata.Count - 1)) % stagedata.Count;
        int cursor2 = (cursor + 1) % stagedata.Count;

        //カーソルに合わせサブタイトルを表示
        titleText[0].text = stagedata[cursor0].subTitle;
        titleText[1].text = stagedata[cursor].subTitle;
        titleText[2].text = stagedata[cursor2].subTitle;

        //決定が押されたら
        if (sceneController.InputActions["Jump"].triggered && !nextFlag && stagedata[cursor].playable)
        {
            sceneController.SoundManager.StopBgm(); //BGMを停止
            sceneController.SaveData.SelestStage = cursor; //選んだステージ番号を設定
            sceneController.SaveData.SetNovelFileName(true); //ファイル名を設定
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //効果音を再生
            var sc = FindObjectOfType<SceneController>();
            sc.SetNextScene("NovelPart"); //ノベルパートに移動
        }

        //戻るなら
        if (sceneController.InputActions["Fire"].triggered && !nextFlag)
        {
            sceneController.SoundManager.StopBgm(); //BGMを停止
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CANCEL); //効果音を再生
            var sc = FindObjectOfType<SceneController>();
            sc.SetNextScene("Title"); //タイトルに戻る
        }
    }
    
    //ファイルの読み込み
    private void LoadFile()
    {
        string[] row = txtFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 0; i < row.Length; i++)
        {
            StageDataStruct sdata = new StageDataStruct();
            string[] col = row[i].Split(','); //カンマで区切る
            //選択可能なステージなら
            if (i <= sceneController.SaveData.PlayableStage)
            {
                sdata.playable = true; //選べるフラグをtrueに
                sdata.subTitle = col[0].Replace(';', '\n'); //セミコロンを改行に変換
                sdata.novelName = col[1].TrimEnd(); //ファイル名を設定
            }
            //選択不可能なら
            else
            {
                sdata.playable = false; //選べるフラグをfalseに
                sdata.subTitle = "？？？"; //サブタイトルを非表示
                sdata.novelName = ""; //ファイル名を設定
            }
            stagedata.Add(sdata); //リストに追加
        }
    }
}
