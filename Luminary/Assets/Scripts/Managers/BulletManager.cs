using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private Player player; //���@�̃I�u�W�F�N�g

    private List<AbstructBullet> bulletList; //�e�̃��X�g
    private SoundEffectManager soundEffectManager = null; //���ʉ��֘A�̃I�u�W�F�N�g
    private EffectManager effectManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        soundEffectManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
        effectManager = FindObjectOfType<EffectManager>(); //�I�u�W�F�N�g���擾

        bulletList = new List<AbstructBullet>();
        GetComponentsInChildren<AbstructBullet>(bulletList); //���łɂ���e���擾
        //�e������Ȃ�
        if (0 < bulletList.Count)
        {
            foreach (AbstructBullet ab in bulletList)
            {
                ab.FirstSetting(this, player, effectManager, soundEffectManager); //�ŏ��̍X�V
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�e������Ȃ�
        if (0 < bulletList.Count)
        {
            //�e�̐��J��Ԃ�
            foreach (AbstructBullet b in bulletList)
            {
                //�e�������Ȃ�
                if (b.DelFlag)
                {
                    Destroy(b.gameObject); //�I�u�W�F�N�g������
                }
            }
            bulletList.RemoveAll(bul => bul.DelFlag);
        }
    }

    //�e�̐���
    public AbstructBullet CreateBullet(GameObject bullet, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject bObj = Instantiate(bullet, position, rotation, parent); //�e�𐶐�
        AbstructBullet ab = bObj.GetComponent<AbstructBullet>();
        ab.FirstSetting(this, player, effectManager, soundEffectManager); ; //�ŏ��̍X�V
        bulletList.Add(ab); //�e�����X�g�ɒǉ�
        return ab; //�e��Ԃ�
    }

    //�e�̐���
    public AbstructBullet CreateBullet(GameObject bullet, Vector3 position, Quaternion rotation, float baseSpeed, Transform parent = null)
    {
        GameObject bObj = Instantiate(bullet, position, rotation, parent); //�e�𐶐�
        AbstructBullet ab = bObj.GetComponent<AbstructBullet>();
        ab.FirstSetting(this, player, effectManager, soundEffectManager); //�ŏ��̍X�V
        ab.SetBaseSpeed(baseSpeed); //��ƂȂ鑬�x��ݒ�
        bulletList.Add(ab); //�e�����X�g�ɒǉ�
        return ab; //�e��Ԃ�
    }
}
