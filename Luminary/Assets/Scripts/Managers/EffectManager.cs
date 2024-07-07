using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset eFile = null; //�G�t�F�N�g�̏��t�@�C��

    [SerializeField]
    private SoundEffectManager soundEffectManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    private Dictionary<string, GameObject> eList; //�G�t�F�N�g���
    private List<AbstructEffect> effectList; //�G�t�F�N�g�̃��X�g

    private void Awake()
    {
        //�I�u�W�F�N�g�����łɑ��݂���Ȃ�
        if (1 < FindObjectsOfType<EffectManager>().Length)
        {
            Destroy(gameObject); //�I�u�W�F�N�g������
        }

        eList = new Dictionary<string, GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadEffectFile(); //�t�@�C�������[�h
        effectList = new List<AbstructEffect>();
        GetComponentsInChildren<AbstructEffect>(effectList); //���łɂ���e���擾
        //�G�t�F�N�g������Ȃ�
        if (0 < effectList.Count)
        {
            foreach (AbstructEffect ae in effectList)
            {
                ae.FirstSetting(this, soundEffectManager, 1.0f); //�ŏ��̍X�V
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < effectList.Count)
        {
            foreach (AbstructEffect e in effectList)
            {
                //�G�t�F�N�g�������Ȃ�
                if (e.DelFlag)
                {
                    Destroy(e.gameObject); //�I�u�W�F�N�g������
                }
            }
            effectList.RemoveAll(eff => eff.DelFlag);
        }
    }

    //�G�t�F�N�g���̓ǂݍ���
    private void LoadEffectFile()
    {
        string[] row = eFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            string name = col[0].TrimEnd(); //���s���폜
            
            //�G�t�F�N�g�����[�h
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
            eList.Add(name, handle.WaitForCompletion());
        }
    }

    //�G�t�F�N�g�̐���
    public AbstructEffect CreateEffect(string effectName, Vector3 position, Quaternion rotation, Transform parent = null, float scale = 1.0f)
    {
        if (!eList.ContainsKey(effectName)) return null; //���݂��Ȃ��Ȃ�߂�

        GameObject eObj = Instantiate(eList[effectName], position, rotation, parent); //�G�t�F�N�g�𐶐�
        AbstructEffect ae = eObj.GetComponent<AbstructEffect>();
        ae.FirstSetting(this, soundEffectManager, scale); //�ŏ��̍X�V
        effectList.Add(ae); //�e�����X�g�ɒǉ�
        return ae; //�e��Ԃ�
    }
}
