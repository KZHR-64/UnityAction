using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEnergyItem : AbstructItem
{
    [SerializeField]
    private int heal = 0; //�񕜗�

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�A�C�e���l�����̏���
    protected override void ActiveItem()
    {
        player.GainHealEnergy(heal); //���@�̉񕜃G�l���M�[�𑝂₷
    }
}
