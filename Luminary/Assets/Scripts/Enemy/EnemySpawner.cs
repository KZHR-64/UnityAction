using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵呼び出し、連戦用
public class EnemySpawner : MonoBehaviour
{
    private GameObject spawnEnemy = null; //呼び出す敵
    private AbstructEnemy abstructEnemy = null; //出した敵
    private Vector3 spawnPosition; //出す位置
    private bool spawnedFlag = false; //出したフラグ

    private EnemyManager enemyManager = null; //敵関連のオブジェクト
    private EffectManager effManager = null; //エフェクト関連のオブジェクト

    // Update is called once per frame
    void Update()
    {
        //出した敵がやられているなら
        if (spawnedFlag && !abstructEnemy)
        {
            Destroy(this); //オブジェクトを消す
        }
    }

    //最初の設定
    public void FirstSetting(GameObject ene, Vector3 pos, EnemyManager em, EffectManager eff)
    {
        spawnEnemy = ene;
        spawnPosition = pos;
        enemyManager = em;
        effManager = eff;
        StartCoroutine(CreateEnemy()); // 敵を出す
    }

    //敵を出すコルーチン
    private IEnumerator CreateEnemy()
    {
        effManager.CreateEffect(EffectNameList.EFFECT_SPAWN, spawnPosition, new Quaternion()); //出現エフェクトを出す

        yield return new WaitForSeconds(0.5f); //少し待機
        abstructEnemy = enemyManager.CreateEnemy(spawnEnemy, spawnPosition, new Quaternion()); //敵を出す
        spawnedFlag = true; //出したフラグをtrueに
        yield return null;
    }
}
