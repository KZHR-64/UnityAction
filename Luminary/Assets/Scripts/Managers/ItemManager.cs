using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset itemTableFile = null; //�A�C�e���e�[�u���̃t�@�C��

    [SerializeField]
    private List<AbstructItem> itemList; //�A�C�e���̃��X�g

    private List<int> itemTable = null; //�A�C�e���e�[�u��

    // Start is called before the first frame update
    void Start()
    {
        itemTable = new List<int>();
        LoadItemTable(); //�A�C�e���e�[�u���̃��[�h
    }

    //�A�C�e���e�[�u���̃��[�h
    private void LoadItemTable()
    {
        string[] row = itemTableFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            string num = col[1].TrimEnd(); //���s���폜

            itemTable.Add(int.Parse(num)); //���X�g�ɒǉ�
        }
    }

    //�A�C�e���̒��I
    public void LotteryItem(Transform itemTrans)
    {
        int itemNum = Random.Range(0, itemTable.Count - 1); //�A�C�e���𒊑I
        //�A�C�e�������݂���Ȃ�
        if(itemTable[itemNum] != -1)
        {
            Instantiate(itemList[itemTable[itemNum]], itemTrans.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)); //�A�C�e�����o��
        }
    }
}
