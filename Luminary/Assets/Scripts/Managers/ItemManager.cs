using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset itemTableFile = null; //アイテムテーブルのファイル

    [SerializeField]
    private List<AbstructItem> itemList; //アイテムのリスト

    private List<int> itemTable = null; //アイテムテーブル

    // Start is called before the first frame update
    void Start()
    {
        itemTable = new List<int>();
        LoadItemTable(); //アイテムテーブルのロード
    }

    //アイテムテーブルのロード
    private void LoadItemTable()
    {
        string[] row = itemTableFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //カンマで区切る
            string num = col[1].TrimEnd(); //改行を削除

            itemTable.Add(int.Parse(num)); //リストに追加
        }
    }

    //アイテムの抽選
    public void LotteryItem(Transform itemTrans)
    {
        int itemNum = Random.Range(0, itemTable.Count - 1); //アイテムを抽選
        //アイテムが存在するなら
        if(itemTable[itemNum] != -1)
        {
            Instantiate(itemList[itemTable[itemNum]], itemTrans.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)); //アイテムを出す
        }
    }
}
