using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveData : MonoBehaviour
{
    //セーブデータの構造体
    [System.Serializable]
    private struct SaveDataStruct
    {
        public int playableStage; //遊べるステージ数
        public float bgmVolume; //BGMの音量
        public float seVolume; //効果音の音量
    }
    [SerializeField]
    private List<string> StartNovelFileNameList = null; //ステージ開始時のノベルファイル名
    [SerializeField]
    private List<string> EndNovelFileNameList = null; //ステージ終了時のノベルファイル名

    private string filePath = ""; //セーブデータのパス
    private SaveDataStruct saveData; //セーブデータ
    private string novelFileName = "stage1Start.csv"; //ノベルパート用のファイル名
    private int selestStage = 0; //選択中のステージ
    private int hp = Calculation.PLAYER_HP_MAX; //自機のHP
    private int healEnergy = 0; //回復エネルギー

    public string NovelFileName { get { return novelFileName; } } //ノベルパート用のファイル名
    public int SelestStage { get { return selestStage; } set { selestStage = value; } } //選択中のステージ
    public int Hp { get { return hp; } set { hp = value; } } //HP
    public int HealEnergy { get { return healEnergy; } set { healEnergy = value; } } //回復エネルギー
    public int PlayableStage { get { return saveData.playableStage; } set { saveData.playableStage = value; } } //遊べるステージ数
    public float BgmVolume { get { return saveData.bgmVolume; } set { saveData.bgmVolume = value; } } //BGMの音量
    public float SeVolume { get { return saveData.seVolume; } set { saveData.seVolume = value; } } //効果音の音量

    private void Awake()
    {
        //オブジェクトがすでに存在するなら
        if (1 < FindObjectsOfType<SaveData>().Length)
        {
            Destroy(gameObject); //オブジェクトを消す
        }
        //ファイルパスの設定
#if UNITY_EDITOR
        filePath = Path.Combine(Directory.GetCurrentDirectory(), Calculation.SAVE_FILE_NAME);
#else
        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), Calculation.SAVE_FILE_NAME);
#endif
        LoadData(); //セーブデータの読み込み
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDestroy()
    {
        WriteSaveData();
    }

    //セーブデータの読み込み
    private void LoadData()
    {
        //セーブデータがあるなら
        if (File.Exists(filePath))
        {
            //ファイルから読み込み
            StreamReader reader = new StreamReader(filePath);
            string strData = reader.ReadToEnd();
            saveData = JsonUtility.FromJson<SaveDataStruct>(strData);
        }
        //ないなら
        else
        {
            //デフォルト値を設定
            saveData.playableStage = 0;
            saveData.bgmVolume = Calculation.VOLUME_DEFAULT;
            saveData.seVolume = Calculation.VOLUME_DEFAULT;

            WriteSaveData(); //セーブデータを生成
        }
    }

    //セーブデータの更新
    public void WriteSaveData()
    {
        string strData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, strData); //ファイルに保存
    }

    //クリア情報を更新するか確認
    public void CheckClearStage()
    {
        //今選べる最後のステージをクリアしていたら
        if(saveData.playableStage <= selestStage)
        {
            saveData.playableStage++; //遊べるステージ数を更新
        }
    }

    //自機の初期化
    public void ResetStatus()
    {
        hp = Calculation.PLAYER_HP_MAX; //HPを初期化
        healEnergy = 0; //回復エネルギーを初期化
    }

    //ノベルパートファイル名の設定
    public void SetNovelFileName(bool startStage)
    {
        //開始時なら
        if(startStage)
        {
            novelFileName = StartNovelFileNameList[selestStage]; //ファイル名を設定
        }
        //終了時なら
        else
        {
            novelFileName = EndNovelFileNameList[selestStage]; //ファイル名を設定
        }
    }
}
