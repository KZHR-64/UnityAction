using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset seFile = null; //効果音の情報ファイル

    private Dictionary<string, AudioClip> seList; //効果音情報

    private AudioSource aSource = null;

    private void Awake()
    {
        //オブジェクトがすでに存在するなら
        if (1 < FindObjectsOfType<SoundManager>().Length)
        {
            Destroy(gameObject); //オブジェクトを消す
        }

        seList = new Dictionary<string, AudioClip>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>(); //AudioSourceを取得
        LoadSeFile(); //ファイルをロード

        SaveData saveData = FindObjectOfType<SaveData>(); //オブジェクトを取得
        aSource.volume = saveData.SeVolume; //音量を設定
    }

    //効果音情報の読み込み
    private void LoadSeFile()
    {
        string[] row = seFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //カンマで区切る
            string name =  col[1].TrimEnd(); //改行を削除
            //効果音をロード
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(name);
            seList.Add(name, handle.WaitForCompletion());
        }
    }

    //効果音を再生する
    public void PlaySe(string seName)
    {
        if (!seList.ContainsKey(seName)) return; //存在しないなら戻る
        aSource.PlayOneShot(seList[seName]); //効果音を再生
    }
}
