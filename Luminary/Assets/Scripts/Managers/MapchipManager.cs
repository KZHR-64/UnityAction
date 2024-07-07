using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapchipManager : MonoBehaviour
{
    private List<AbstructMapchip> mapchipList; //マップチップのリスト
    private Manager manager = null; //管理用のオブジェクト

    public Manager Manager { set { manager = value; } } //管理用のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        mapchipList = new List<AbstructMapchip>();
        GetComponentsInChildren<AbstructMapchip>(mapchipList); //マップチップを取得
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AbstructMapchip m in mapchipList)
        {
            //マップチップを消すなら
            if (m.DelFlag)
            {
                Destroy(m.gameObject); //オブジェクトを消す
            }
        }
        mapchipList.RemoveAll(mchi => mchi.DelFlag);
    }

    //マップチップの生成
    public AbstructMapchip CreateMapchip(Transform mapchipPos)
    {
        return null;
    }
}
