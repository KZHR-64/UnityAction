using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset eFile = null; //エフェクトの情報ファイル

    [SerializeField]
    private SoundEffectManager soundEffectManager = null; //効果音関連のオブジェクト

    private Dictionary<string, GameObject> eList; //エフェクト情報
    private List<AbstructEffect> effectList; //エフェクトのリスト

    private void Awake()
    {
        //オブジェクトがすでに存在するなら
        if (1 < FindObjectsOfType<EffectManager>().Length)
        {
            Destroy(gameObject); //オブジェクトを消す
        }

        eList = new Dictionary<string, GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadEffectFile(); //ファイルをロード
        effectList = new List<AbstructEffect>();
        GetComponentsInChildren<AbstructEffect>(effectList); //すでにある弾を取得
        //エフェクトがあるなら
        if (0 < effectList.Count)
        {
            foreach (AbstructEffect ae in effectList)
            {
                ae.FirstSetting(this, soundEffectManager, 1.0f); //最初の更新
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < effectList.Count)
        {
            foreach (AbstructEffect e in effectList)
            {
                //エフェクトを消すなら
                if (e.DelFlag)
                {
                    Destroy(e.gameObject); //オブジェクトを消す
                }
            }
            effectList.RemoveAll(eff => eff.DelFlag);
        }
    }

    //エフェクト情報の読み込み
    private void LoadEffectFile()
    {
        string[] row = eFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //カンマで区切る
            string name = col[0].TrimEnd(); //改行を削除
            
            //エフェクトをロード
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
            eList.Add(name, handle.WaitForCompletion());
        }
    }

    //エフェクトの生成
    public AbstructEffect CreateEffect(string effectName, Vector3 position, Quaternion rotation, Transform parent = null, float scale = 1.0f)
    {
        if (!eList.ContainsKey(effectName)) return null; //存在しないなら戻る

        GameObject eObj = Instantiate(eList[effectName], position, rotation, parent); //エフェクトを生成
        AbstructEffect ae = eObj.GetComponent<AbstructEffect>();
        ae.FirstSetting(this, soundEffectManager, scale); //最初の更新
        effectList.Add(ae); //弾をリストに追加
        return ae; //弾を返す
    }
}
