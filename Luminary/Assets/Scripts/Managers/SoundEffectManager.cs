using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset seFile = null; //���ʉ��̏��t�@�C��

    private Dictionary<string, AudioClip> seList; //���ʉ����

    private AudioSource aSource = null;

    private void Awake()
    {
        //�I�u�W�F�N�g�����łɑ��݂���Ȃ�
        if (1 < FindObjectsOfType<SoundManager>().Length)
        {
            Destroy(gameObject); //�I�u�W�F�N�g������
        }

        seList = new Dictionary<string, AudioClip>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>(); //AudioSource���擾
        LoadSeFile(); //�t�@�C�������[�h

        SaveData saveData = FindObjectOfType<SaveData>(); //�I�u�W�F�N�g���擾
        aSource.volume = saveData.SeVolume; //���ʂ�ݒ�
    }

    //���ʉ����̓ǂݍ���
    private void LoadSeFile()
    {
        string[] row = seFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            string name =  col[1].TrimEnd(); //���s���폜
            //���ʉ������[�h
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(name);
            seList.Add(name, handle.WaitForCompletion());
        }
    }

    //���ʉ����Đ�����
    public void PlaySe(string seName)
    {
        if (!seList.ContainsKey(seName)) return; //���݂��Ȃ��Ȃ�߂�
        aSource.PlayOneShot(seList[seName]); //���ʉ����Đ�
    }
}
