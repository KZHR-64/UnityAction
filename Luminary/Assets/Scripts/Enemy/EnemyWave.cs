using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵との連戦
public class EnemyWave : MonoBehaviour
{
    //敵情報の構造体
    [System.Serializable]
    private struct SpawnStruct
    {
        public GameObject enemy; //出す敵
        public Vector3 position; //出す位置
    }

    [SerializeField]
    EnemySpawner enemySpawner = null; // 敵を出すためのオブジェクト
    [SerializeField]
    private int spawnMax = 1; //同時に画面に出す敵の数
    [SerializeField]
    private bool checkZero = false; //一通り倒されてから補充するか
    [SerializeField]
    private List<SpawnStruct> spawnList = null; //出す敵の情報リスト

    private bool activeFlag = false; //開始したか
    private int enemyCount = 0; //出した敵の数
    private bool replenishFlag = true; //敵を補充するフラグ
    private EnemySpawner[] spawnedEnemy = new EnemySpawner[4]; //すでに出ている敵

    private EnemyManager enemyManager = null; //敵関連のオブジェクト
    protected EffectManager effManager = null; //エフェクト関連のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        spawnMax = Mathf.Clamp(spawnMax, 1, 4); //同時に出す敵の最大数を調整
        StartCoroutine(FirstWait()); //少し待機
    }

    // Update is called once per frame
    void Update()
    {
        int enemyNum = 0; //敵の数
        //開始していないなら
        if (!activeFlag)
        {
            return; //終了
        }

        //同時出現の数繰り返し
        for(int i = 0; i < spawnMax; i++)
        {
            //出ていない敵がいるなら
            if (!spawnedEnemy[i])
            {
                //敵を補充するなら
                if (replenishFlag)
                {
                    CreateEnemy1(i); //敵を出す
                    enemyNum++; //カウントを増やす
                }
            }
            //出ているなら
            else
            {
                enemyNum++; //カウントを増やす
            }
        }
        //最大まで敵がいる、出す予定なら
        if (spawnMax <= enemyNum && checkZero)
        {
            replenishFlag = false; //しばらく補充しない
        }
        //敵がいないなら
        if(enemyNum <= 0 && checkZero)
        {
            replenishFlag = true; //敵を補充する
        }
        //最後まで出したら
        if (spawnList.Count <= enemyCount)
        {
            activeFlag = false; //開始フラグをfalseに
        }
    }

    //最初の設定
    public void FirstSetting(EnemyManager em, Player p, EffectManager eff)
    {
        //player = p;
        enemyManager = em;
        effManager = eff;
    }

    //敵を出す
    private void CreateEnemy1(int num)
    {
        //最後まで出していたら戻る
        if (spawnList.Count <= enemyCount) return;
        spawnedEnemy[num] = Instantiate<EnemySpawner>(enemySpawner, transform.position, transform.rotation); //敵の呼び出しオブジェクトを生成
        spawnedEnemy[num].FirstSetting(spawnList[enemyCount].enemy, spawnList[enemyCount].position, enemyManager, effManager); //初期設定
        enemyCount++; //出した敵の合計を増加
    }

    //少し待機
    private IEnumerator FirstWait()
    {
        yield return new WaitForSeconds(0.2f);
        activeFlag = true; //開始フラグをtrueに

        yield return null;
    }

    //終了したか確認
    public bool CheckWaveEnd()
    {
        bool beatFlag = true; //全員倒したか

        //同時出現の数繰り返し
        for (int i = 0; i < spawnedEnemy.Length; i++)
        {
            //出ている敵がいるなら
            if (spawnedEnemy[i])
            {
                beatFlag = false; //倒したフラグをfalseに
                break;
            }
        }
        return beatFlag && (spawnList.Count <= enemyCount);
    }
}
