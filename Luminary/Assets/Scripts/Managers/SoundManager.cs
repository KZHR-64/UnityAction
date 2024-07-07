using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour
{
    //BGM���̍\����
    [Serializable]
    private struct BgmData
    {
        public string address; //�A�h���X��
        public bool loop; //���[�v���邩
        public int loopStart; //���[�v�J�n�̃T���v����
        public int loopLength; //���[�v�J�n�n�_���烋�[�v�n�_�܂ł̃T���v����
    }

    [SerializeField]
    private TextAsset bgmFile = null; //BGM�̏��t�@�C��

    [SerializeField]
    private string bgmName = ""; //�Đ�����BGM

    private Dictionary<string, BgmData> bgmList; //BGM���

    private AudioSource aSource = null;
    private AsyncOperationHandle<AudioClip> handle; //BGM�̃n���h��

    private void Awake()
    {
        //�I�u�W�F�N�g�����łɑ��݂���Ȃ�
        if (1 < FindObjectsOfType<SoundManager>().Length)
        {
            Destroy(gameObject); //�I�u�W�F�N�g������
        }

        bgmList = new Dictionary<string, BgmData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>(); //AudioSource���擾
        LoadBgmFile(); //�t�@�C�������[�h

        SaveData saveData = FindObjectOfType<SaveData>(); //�I�u�W�F�N�g���擾
        aSource.volume = saveData.BgmVolume; //���ʂ�ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        if (bgmName.Length == 0) return;
        //���[�v�n�_�ɓ�������
        if(bgmList[bgmName].loopStart + bgmList[bgmName].loopLength <= aSource.timeSamples && bgmList[bgmName].loop)
        {
            aSource.timeSamples = bgmList[bgmName].loopStart + (aSource.timeSamples % (bgmList[bgmName].loopStart + bgmList[bgmName].loopLength)); //�J�n�ʒu�܂Ŗ߂�
            //���[�v�n�_=�I�[��������
            if (!aSource.isPlaying)
            {
                aSource.Play(); //�Đ����Ȃ���
            }
        }
    }

    //BGM���̓ǂݍ���
    private void LoadBgmFile()
    {
        string[] row = bgmFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 1; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            BgmData bd = new BgmData();
            bd.address = col[0]; //�A�h���X��ݒ�
            bd.loop = col[1].Contains("1"); //���[�v���邩��ݒ�
            bd.loopStart = int.Parse(col[2]); //���[�v�J�n�ʒu��ݒ�
            bd.loopLength = int.Parse(col[3]); //���[�v�܂ł̒�����ݒ�
            bgmList.Add(bd.address, bd); //�������X�g�ɒǉ�
        }
    }

    //BGM���Đ�����
    public void PlayBgm(string playName)
    {
        if (!bgmList.ContainsKey(playName)) return; //���݂��Ȃ��Ȃ�߂�
        if (bgmName.Contains(playName)) return; //�����ȂȂ�߂�
        //�Đ�����BGM������Ȃ�
        if (aSource.clip)
        {
            aSource.Stop(); //BGM���~
            Addressables.Release(handle); //BGM�̃n���h�������
        }
        handle = Addressables.LoadAssetAsync<AudioClip>(bgmList[playName].address); //BGM�����[�h
        aSource.clip = handle.WaitForCompletion(); //�I�[�f�B�I�N���b�v��ݒ�
        aSource.timeSamples = 0;
        aSource.Play(); //BGM���Đ�
        bgmName = playName; //�Đ�����BGM��ݒ�
    }

    //BGM���~����
    public void StopBgm()
    {
        aSource.Stop(); //BGM���~
        bgmName = ""; //�Đ�����BGM��ݒ�
    }
}
