using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapchipManager : MonoBehaviour
{
    private List<AbstructMapchip> mapchipList; //�}�b�v�`�b�v�̃��X�g
    private Manager manager = null; //�Ǘ��p�̃I�u�W�F�N�g

    public Manager Manager { set { manager = value; } } //�Ǘ��p�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        mapchipList = new List<AbstructMapchip>();
        GetComponentsInChildren<AbstructMapchip>(mapchipList); //�}�b�v�`�b�v���擾
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AbstructMapchip m in mapchipList)
        {
            //�}�b�v�`�b�v�������Ȃ�
            if (m.DelFlag)
            {
                Destroy(m.gameObject); //�I�u�W�F�N�g������
            }
        }
        mapchipList.RemoveAll(mchi => mchi.DelFlag);
    }

    //�}�b�v�`�b�v�̐���
    public AbstructMapchip CreateMapchip(Transform mapchipPos)
    {
        return null;
    }
}
