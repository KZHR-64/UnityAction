using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private Image[] hpImage; //HPの画像
    [SerializeField]
    private Image[] healEnImage; //回復エネルギーの画像

    private Player player = null; //自機のオブジェクト

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>(); //オブジェクトを取得
        HpUpdate(); //HP関連の更新
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HpUpdate(); //HP関連の更新
    }

    private void FixedUpdate()
    {

    }

    //HP関連の更新
    private void HpUpdate()
    {
        //HP残量を表示
        for (int i = 0; i < hpImage.Length; i++)
        {
            //自機のHP未満なら
            if(i < player.Hp)
            {
                hpImage[i].enabled = true; //画像を表示
            }
            //HP以上なら
            else
            {
                hpImage[i].enabled = false; //画像を非表示
            }
        }

        //回復エネルギーを表示
        for (int i = 0; i < healEnImage.Length; i++)
        {
            //自機の回復エネルギー未満なら
            if (i < player.HealEnergy)
            {
                healEnImage[i].enabled = true; //画像を表示
            }
            //回復エネルギー以上なら
            else
            {
                healEnImage[i].enabled = false; //画像を非表示
            }
        }

    }
}
