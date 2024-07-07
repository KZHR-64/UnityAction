using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour
{
    //BGM情報の構造体
    [Serializable]
    private struct BgmData
    {
        public string address; //アドレス名
        public bool loop; //ループするか
        public int loopStart; //ループ開始のサンプル数
        public int loopLength; //ループ開始地点からループ地点までのサンプル数
    }

    [SerializeField]
    private TextAsset bgmFile = null; //BGMの情報ファイル

    [SerializeField]
    private string bgmName = ""; //再生するBGM

    private Dictionary<string, BgmData> bgmList; //BGM情報

    private AudioSource aSource = null;
    private AsyncOperationHandle<AudioClip> handle; //BGMのハンドル

    private void Awake()
    {
        //オブジェクトがすでに存在するなら
        if (1 < FindObjectsOfType<SoundManager>().Length)
        {
            Destroy(gameObject); //オブジェクトを消す
        }

        bgmList = new Dictionary<string, BgmData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>(); //AudioSourceを取得
        LoadBgmFile(); //ファイルをロード

        SaveData saveData = FindObjectOfType<SaveData>(); //オブジェクトを取得
        aSource.volume = saveData.BgmVolume; //音量を設定
    }

    // Update is called once per frame
    void Update()
    {
        if (bgmName.Length == 0) return;
        //ループ地点に入ったら
        if(bgmList[bgmName].loopStart + bgmList[bgmName].loopLength <= aSource.timeSamples && bgmList[bgmName].loop)
        {
            aSource.timeSamples = bgmList[bgmName].loopStart + (aSource.timeSamples % (bgmList[bgmName].loopStart + bgmList[bgmName].loopLength)); //開始位置まで戻る
            //ループ地点=終端だったら
            if (!aSource.isPlaying)
            {
                aSource.Play(); //再生しなおす
            }
        }
    }

    //BGM情報の読み込み
    private void LoadBgmFile()
    {
        string[] row = bgmFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //カンマで区切る
            BgmData bd = new BgmData();
            bd.address = col[0]; //アドレスを設定
            bd.loop = col[1].Contains("1"); //ループするかを設定
            bd.loopStart = int.Parse(col[2]); //ループ開始位置を設定
            bd.loopLength = int.Parse(col[3]); //ループまでの長さを設定
            bgmList.Add(bd.address, bd); //情報をリストに追加
        }
    }

    //BGMを再生する
    public void PlayBgm(string playName)
    {
        if (!bgmList.ContainsKey(playName)) return; //存在しないなら戻る
        if (bgmName.Contains(playName)) return; //同じ曲なら戻る
        //再生中のBGMがあるなら
        if (aSource.clip)
        {
            aSource.Stop(); //BGMを停止
            Addressables.Release(handle); //BGMのハンドルを解放
        }
        handle = Addressables.LoadAssetAsync<AudioClip>(bgmList[playName].address); //BGMをロード
        aSource.clip = handle.WaitForCompletion(); //オーディオクリップを設定
        aSource.timeSamples = 0;
        aSource.Play(); //BGMを再生
        bgmName = playName; //再生中のBGMを設定
    }

    //BGMを停止する
    public void StopBgm()
    {
        aSource.Stop(); //BGMを停止
        bgmName = ""; //再生中のBGMを設定
    }
}
