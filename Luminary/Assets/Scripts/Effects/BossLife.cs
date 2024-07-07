using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLife : MonoBehaviour
{
    [SerializeField]
    private Image bossLifeBar = null; //体力バーの画像

    private AbstructEnemy bossObject = null; //ボスのオブジェクト

    // Update is called once per frame
    void Update()
    {
        //ボスがいなければ
        if(!bossObject)
        {
            gameObject.SetActive(false); //体力バーを非表示
            return; //終了
        }

        bossLifeBar.rectTransform.sizeDelta = new Vector2(480.0f * bossObject.Hp / bossObject.MaxHp, 48.0f); //残りHPに応じて長さを変更
    }

    //ボスのオブジェクトを設定
    public void SetBossLife(AbstructEnemy enemy)
    {
        bossObject = enemy;
    }
}
