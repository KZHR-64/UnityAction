using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : AbstructItem
{
    [SerializeField]
    private int heal = 0; //回復量

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //アイテム獲得時の処理
    protected override void ActiveItem()
    {
        player.Healing(heal); //自機のHPを回復
    }
}
