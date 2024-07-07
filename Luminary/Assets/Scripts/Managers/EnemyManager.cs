using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Player player; //自機のオブジェクト
    [SerializeField]
    private EventManager eveManager = null; //イベント関連のオブジェクト
    [SerializeField]
    private ItemManager itemManager = null; //アイテム管理のオブジェクト
    [SerializeField]
    private BulletManager bulletManager = null; //弾の管理オブジェクト

    private List<AbstructEnemy> enemyList; //弾のリスト
    private EnemyWave enemyWave = null; //敵ウェーブ
    private bool bossBattleFlag = false; //ボス戦中か
    private bool bossLiveFlag = false; //ボスがいるか

    private CameraManager cameraManager = null; //カメラ関連のオブジェクト
    private SoundEffectManager soundEffectManager = null; //効果音関連のオブジェクト
    private EffectManager effManager = null; //エフェクト関連のオブジェクト

    public bool BossBattleFlag { get { return bossBattleFlag; } } //ボス戦中か

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>(); //オブジェクトを取得
        soundEffectManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
        effManager = FindObjectOfType<EffectManager>(); //オブジェクトを取得

        enemyList = new List<AbstructEnemy>();
        GetComponentsInChildren<AbstructEnemy>(enemyList); //すでにいる敵を取得
        //敵がいるなら
        if (0 < enemyList.Count)
        {
            foreach (AbstructEnemy ae in enemyList)
            {
                ae.FirstSetting(this, player, bulletManager, soundEffectManager, effManager, cameraManager); //最初の更新
            }
        }
        enemyWave = GetComponentInChildren<EnemyWave>(); //オブジェクトを取得
        //敵ウェーブがあるなら
        if(enemyWave)
        {
            enemyWave.FirstSetting(this, player, effManager); //最初の更新
        }
    }

    // Update is called once per frame
    void Update()
    {
        bossLiveFlag = false;
        //敵がいるなら
        if (0 < enemyList.Count)
        {
            //敵の数繰り返し
            foreach (AbstructEnemy e in enemyList)
            {
                //ボスがいるなら
                if(e.BossFlag)
                {
                    bossLiveFlag = true; //ボスがいるフラグをtrueに
                    //ボス戦中でないなら
                    if(!bossBattleFlag)
                    {
                        bossBattleFlag = true; //ボス戦フラグをtrueに
                    }
                }
                //敵を消すなら
                if (e.DelFlag)
                {
                    //アイテムを落とすなら
                    if (e.ItemDropFlag)
                    {
                        itemManager.LotteryItem(e.transform); //アイテムの抽選
                    }
                    Destroy(e.gameObject); //オブジェクトを消す
                }
            }
            enemyList.RemoveAll(ene => ene.DelFlag);
        }
    }

    //敵の生成
    public AbstructEnemy CreateEnemy(GameObject enemy, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject eObj = Instantiate(enemy, position, rotation, parent); //敵を生成
        AbstructEnemy ae = eObj.GetComponent<AbstructEnemy>();
        ae.FirstSetting(this, player, bulletManager, soundEffectManager, effManager, cameraManager); //最初の更新
        enemyList.Add(ae); //敵をリストに追加
        return ae; //敵を返す*/
    }

    //ボス戦が終わったか取得
    public bool CheckBossBattleEnd()
    {
        if(enemyWave)
        {
            return enemyWave.CheckWaveEnd(); //ウェーブが終わったか確認
        }
        //ボスがやられていたら
        if (!bossLiveFlag && bossBattleFlag)
        {
            bossBattleFlag = false; //ボス戦フラグをfalseに
            return true;
        }
        return false;
    }

    //ボス撃破時の処理
    public void BeatBoss()
    {
        eveManager.ActiveBeatBossEvent(); //ボス撃破イベントを開始
    }
}
