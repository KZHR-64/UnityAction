using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private Player player; //自機のオブジェクト

    private List<AbstructBullet> bulletList; //弾のリスト
    private SoundEffectManager soundEffectManager = null; //効果音関連のオブジェクト
    private EffectManager effectManager = null; //エフェクト関連のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        soundEffectManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
        effectManager = FindObjectOfType<EffectManager>(); //オブジェクトを取得

        bulletList = new List<AbstructBullet>();
        GetComponentsInChildren<AbstructBullet>(bulletList); //すでにある弾を取得
        //弾があるなら
        if (0 < bulletList.Count)
        {
            foreach (AbstructBullet ab in bulletList)
            {
                ab.FirstSetting(this, player, effectManager, soundEffectManager); //最初の更新
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //弾があるなら
        if (0 < bulletList.Count)
        {
            //弾の数繰り返し
            foreach (AbstructBullet b in bulletList)
            {
                //弾を消すなら
                if (b.DelFlag)
                {
                    Destroy(b.gameObject); //オブジェクトを消す
                }
            }
            bulletList.RemoveAll(bul => bul.DelFlag);
        }
    }

    //弾の生成
    public AbstructBullet CreateBullet(GameObject bullet, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject bObj = Instantiate(bullet, position, rotation, parent); //弾を生成
        AbstructBullet ab = bObj.GetComponent<AbstructBullet>();
        ab.FirstSetting(this, player, effectManager, soundEffectManager); ; //最初の更新
        bulletList.Add(ab); //弾をリストに追加
        return ab; //弾を返す
    }

    //弾の生成
    public AbstructBullet CreateBullet(GameObject bullet, Vector3 position, Quaternion rotation, float baseSpeed, Transform parent = null)
    {
        GameObject bObj = Instantiate(bullet, position, rotation, parent); //弾を生成
        AbstructBullet ab = bObj.GetComponent<AbstructBullet>();
        ab.FirstSetting(this, player, effectManager, soundEffectManager); //最初の更新
        ab.SetBaseSpeed(baseSpeed); //基となる速度を設定
        bulletList.Add(ab); //弾をリストに追加
        return ab; //弾を返す
    }
}
