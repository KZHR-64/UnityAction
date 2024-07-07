using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveData : MonoBehaviour
{
    //�Z�[�u�f�[�^�̍\����
    [System.Serializable]
    private struct SaveDataStruct
    {
        public int playableStage; //�V�ׂ�X�e�[�W��
        public float bgmVolume; //BGM�̉���
        public float seVolume; //���ʉ��̉���
    }
    [SerializeField]
    private List<string> StartNovelFileNameList = null; //�X�e�[�W�J�n���̃m�x���t�@�C����
    [SerializeField]
    private List<string> EndNovelFileNameList = null; //�X�e�[�W�I�����̃m�x���t�@�C����

    private string filePath = ""; //�Z�[�u�f�[�^�̃p�X
    private SaveDataStruct saveData; //�Z�[�u�f�[�^
    private string novelFileName = "stage1Start.csv"; //�m�x���p�[�g�p�̃t�@�C����
    private int selestStage = 0; //�I�𒆂̃X�e�[�W
    private int hp = Calculation.PLAYER_HP_MAX; //���@��HP
    private int healEnergy = 0; //�񕜃G�l���M�[

    public string NovelFileName { get { return novelFileName; } } //�m�x���p�[�g�p�̃t�@�C����
    public int SelestStage { get { return selestStage; } set { selestStage = value; } } //�I�𒆂̃X�e�[�W
    public int Hp { get { return hp; } set { hp = value; } } //HP
    public int HealEnergy { get { return healEnergy; } set { healEnergy = value; } } //�񕜃G�l���M�[
    public int PlayableStage { get { return saveData.playableStage; } set { saveData.playableStage = value; } } //�V�ׂ�X�e�[�W��
    public float BgmVolume { get { return saveData.bgmVolume; } set { saveData.bgmVolume = value; } } //BGM�̉���
    public float SeVolume { get { return saveData.seVolume; } set { saveData.seVolume = value; } } //���ʉ��̉���

    private void Awake()
    {
        //�I�u�W�F�N�g�����łɑ��݂���Ȃ�
        if (1 < FindObjectsOfType<SaveData>().Length)
        {
            Destroy(gameObject); //�I�u�W�F�N�g������
        }
        //�t�@�C���p�X�̐ݒ�
#if UNITY_EDITOR
        filePath = Path.Combine(Directory.GetCurrentDirectory(), Calculation.SAVE_FILE_NAME);
#else
        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), Calculation.SAVE_FILE_NAME);
#endif
        LoadData(); //�Z�[�u�f�[�^�̓ǂݍ���
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDestroy()
    {
        WriteSaveData();
    }

    //�Z�[�u�f�[�^�̓ǂݍ���
    private void LoadData()
    {
        //�Z�[�u�f�[�^������Ȃ�
        if (File.Exists(filePath))
        {
            //�t�@�C������ǂݍ���
            StreamReader reader = new StreamReader(filePath);
            string strData = reader.ReadToEnd();
            saveData = JsonUtility.FromJson<SaveDataStruct>(strData);
        }
        //�Ȃ��Ȃ�
        else
        {
            //�f�t�H���g�l��ݒ�
            saveData.playableStage = 0;
            saveData.bgmVolume = Calculation.VOLUME_DEFAULT;
            saveData.seVolume = Calculation.VOLUME_DEFAULT;

            WriteSaveData(); //�Z�[�u�f�[�^�𐶐�
        }
    }

    //�Z�[�u�f�[�^�̍X�V
    public void WriteSaveData()
    {
        string strData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, strData); //�t�@�C���ɕۑ�
    }

    //�N���A�����X�V���邩�m�F
    public void CheckClearStage()
    {
        //���I�ׂ�Ō�̃X�e�[�W���N���A���Ă�����
        if(saveData.playableStage <= selestStage)
        {
            saveData.playableStage++; //�V�ׂ�X�e�[�W�����X�V
        }
    }

    //���@�̏�����
    public void ResetStatus()
    {
        hp = Calculation.PLAYER_HP_MAX; //HP��������
        healEnergy = 0; //�񕜃G�l���M�[��������
    }

    //�m�x���p�[�g�t�@�C�����̐ݒ�
    public void SetNovelFileName(bool startStage)
    {
        //�J�n���Ȃ�
        if(startStage)
        {
            novelFileName = StartNovelFileNameList[selestStage]; //�t�@�C������ݒ�
        }
        //�I�����Ȃ�
        else
        {
            novelFileName = EndNovelFileNameList[selestStage]; //�t�@�C������ݒ�
        }
    }
}
