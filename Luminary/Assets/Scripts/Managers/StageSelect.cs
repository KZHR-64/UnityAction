using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSelect : MonoBehaviour
{
    //�X�e�[�W���̍\����
    private struct StageDataStruct
    {
        public bool playable; //�I�ׂ邩
        public string subTitle; //�T�u�^�C�g��
        public string novelName; //�ǂݍ��ރt�@�C����
    }

    [SerializeField]
    private TextAsset txtFile = null; //�T�u�^�C�g���t�@�C��
    [SerializeField]
    private List<TextMeshProUGUI> titleText = null; //�\�����镶�͗p�̃I�u�W�F�N�g
    [SerializeField]
    private string bgmName = "tukito_search_for_ideals.ogg"; //�Đ�����BGM

    private List<StageDataStruct> stagedata; //�X�e�[�W���

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g

    private bool nextFlag = false; //���̃V�[���Ɉڂ邩
    private int cursor = 0; //�J�[�\���̈ʒu

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        sceneController = FindObjectOfType<SceneController>(); //�I�u�W�F�N�g���擾
        stagedata = new List<StageDataStruct>();
        LoadFile(); //�t�@�C�������[�h
        sceneController.SoundManager.PlayBgm(bgmName); //BGM���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        if (nextFlag) return; //���Ɉڂ�Ȃ��΂�

        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();
        //�㉺�̓��͂���������
        if (sceneController.InputActions["Move"].triggered)
        {
            //�オ�����ꂽ��
            if (0.0f < stickInput.y)
            {
                cursor += (stagedata.Count - 1); //�J�[�\�������
            }
            //���������ꂽ��
            else if (stickInput.y < 0.0f)
            {
                cursor++; //�J�[�\��������
            }
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //���ʉ����Đ�
        }

        cursor %= stagedata.Count; //�J�[�\���𒲐�

        //�㉺�̃T�u�^�C�g����ݒ�
        int cursor0 = (cursor + (stagedata.Count - 1)) % stagedata.Count;
        int cursor2 = (cursor + 1) % stagedata.Count;

        //�J�[�\���ɍ��킹�T�u�^�C�g����\��
        titleText[0].text = stagedata[cursor0].subTitle;
        titleText[1].text = stagedata[cursor].subTitle;
        titleText[2].text = stagedata[cursor2].subTitle;

        //���肪�����ꂽ��
        if (sceneController.InputActions["Jump"].triggered && !nextFlag && stagedata[cursor].playable)
        {
            sceneController.SoundManager.StopBgm(); //BGM���~
            sceneController.SaveData.SelestStage = cursor; //�I�񂾃X�e�[�W�ԍ���ݒ�
            sceneController.SaveData.SetNovelFileName(true); //�t�@�C������ݒ�
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //���ʉ����Đ�
            var sc = FindObjectOfType<SceneController>();
            sc.SetNextScene("NovelPart"); //�m�x���p�[�g�Ɉړ�
        }

        //�߂�Ȃ�
        if (sceneController.InputActions["Fire"].triggered && !nextFlag)
        {
            sceneController.SoundManager.StopBgm(); //BGM���~
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CANCEL); //���ʉ����Đ�
            var sc = FindObjectOfType<SceneController>();
            sc.SetNextScene("Title"); //�^�C�g���ɖ߂�
        }
    }
    
    //�t�@�C���̓ǂݍ���
    private void LoadFile()
    {
        string[] row = txtFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 0; i < row.Length; i++)
        {
            StageDataStruct sdata = new StageDataStruct();
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            //�I���\�ȃX�e�[�W�Ȃ�
            if (i <= sceneController.SaveData.PlayableStage)
            {
                sdata.playable = true; //�I�ׂ�t���O��true��
                sdata.subTitle = col[0].Replace(';', '\n'); //�Z�~�R���������s�ɕϊ�
                sdata.novelName = col[1].TrimEnd(); //�t�@�C������ݒ�
            }
            //�I��s�\�Ȃ�
            else
            {
                sdata.playable = false; //�I�ׂ�t���O��false��
                sdata.subTitle = "�H�H�H"; //�T�u�^�C�g�����\��
                sdata.novelName = ""; //�t�@�C������ݒ�
            }
            stagedata.Add(sdata); //���X�g�ɒǉ�
        }
    }
}
