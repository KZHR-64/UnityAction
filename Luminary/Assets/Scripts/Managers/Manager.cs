using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private PauseManager pauseManager = null; //�|�[�Y��ʂ̃I�u�W�F�N�g
    [SerializeField]
    private BossLife lifeBar = null; //�{�X�̗̑̓o�[�̃I�u�W�F�N�g
    [SerializeField]
    private ItemManager itemManager = null; //�A�C�e���Ǘ��̃I�u�W�F�N�g
    [SerializeField]
    private Player player = null; //���@�̃I�u�W�F�N�g
    [SerializeField]
    private EventManager eveManager = null; //�C�x���g�֘A�̃I�u�W�F�N�g
    [SerializeField]
    private MapchipManager mapchipManager = null; //�}�b�v�`�b�v�֘A�̃I�u�W�F�N�g
    [SerializeField]
    private EnemyManager enemyManager = null; //�G�Ǘ��̃I�u�W�F�N�g
    [SerializeField]
    private string bgmName = ""; //�Đ�����BGM

    private bool missFlag = false; //�~�X������
    private bool warpFlag = false; //���̃}�b�v�Ɉڂ邩
    private string nextMapName = null; //���̃}�b�v�̖��O
    private bool nextFlag = false; //���̃V�[���Ɉڂ邩

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g

    public Player Player { get{ return player; } } //���@�̃I�u�W�F�N�g
    public EventManager EveManager { get { return eveManager; } } //�C�x���g�֘A�̃I�u�W�F�N�g
    public ItemManager ItemManagerObj { get { return itemManager; } } //�A�C�e���Ǘ��̃I�u�W�F�N�g

    private void Awake()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        pauseManager.gameObject.SetActive(false); //�|�[�Y��ʂ���Ă���
        lifeBar.gameObject.SetActive(false); //�̗̓o�[����Ă���
    }

    // Start is called before the first frame update
    void Start()
    {
        mapchipManager.Manager = this; //�I�u�W�F�N�g��ݒ�
        sceneController = FindObjectOfType<SceneController>(); //�I�u�W�F�N�g���擾
        sceneController.SoundManager.PlayBgm(bgmName); //BGM���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        if (nextFlag) return; //���Ɉڂ�Ȃ��΂�
        //�|�[�Y���Ȃ�
        if (pauseManager.gameObject.activeSelf)
        {
            //�X�e�[�W�Z���N�g�ɖ߂�Ȃ�
            if (pauseManager.BackFlag)
            {
                nextFlag = true; //���Ɉڂ�t���O��true��
                var sc = FindObjectOfType<SceneController>();
                sc.SetNextScene("StageSelect"); //�X�e�[�W�Z���N�g�ɖ߂�
            }
            return; //��΂�
        }

        //�Q�[�����~�߂�Ȃ�
        if (eveManager.GameStopFlag)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        //�|�[�Y�������ꂽ��
        if (sceneController.InputActions["Pause"].triggered && !eveManager.GameStopFlag)
        {
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //���ʉ����Đ�
            pauseManager.gameObject.SetActive(true); //�|�[�Y��ʂ�Active��
            pauseManager.PauseActivate(); //�|�[�Y��ʂ��J��
        }

        GameObject[] iList = GameObject.FindGameObjectsWithTag("Item"); //�A�C�e���̃��X�g���擾
        //�A�C�e��������Ȃ�
        if (0 < iList.Length)
        {
            //�A�C�e���̐��J��Ԃ�
            foreach (var it in iList)
            {
                AbstructItem itCom = it.GetComponent<AbstructItem>(); //�A�C�e���̃X�N���v�g���擾
                //�e�������Ȃ�
                if (itCom.DelFlag)
                {
                    Destroy(it); //�I�u�W�F�N�g������
                }
            }
        }

        //�{�X�킪�I�������
        if (enemyManager.CheckBossBattleEnd())
        {
            //�C�x���g�ҋ@���Ȃ�
            if (eveManager.WaitFlag)
            {
                eveManager.WaitFlag = false; //�ҋ@�t���O��false��
            }
        }

        //���[�v����Ȃ�
        if(warpFlag && !nextFlag)
        {
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SaveData.Hp = player.Hp; //���@��HP�����p��
            sceneController.SaveData.HealEnergy = player.HealEnergy; //�񕜃G�l���M�[�����p��
            sceneController.SetNextScene(nextMapName); //���̃}�b�v�Ɉړ�
        }

        //�N���A�����Ȃ�
        if(eveManager.ClearFlag && !nextFlag)
        {
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SaveData.CheckClearStage(); //�X�e�[�W���X�V�̊m�F
            sceneController.SaveData.SetNovelFileName(false); //�t�@�C������ݒ�
            sceneController.SetNextScene("NovelPart"); //���̃}�b�v�Ɉړ�
        }

        //�~�X���Ă���Ȃ�
        if (missFlag && eveManager.MissFlag)
        {
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SaveData.Hp = Calculation.PLAYER_HP_MAX; //���@��HP��������
            sceneController.SaveData.HealEnergy = player.HealEnergy; //�񕜃G�l���M�[�����p��
            sceneController.ReloadScene(); //�V�[���������[�h
        }

        //���@��HP��0�Ȃ�
        if (player.Hp <= 0 && !missFlag)
        {
            missFlag = true; //�~�X�����t���O��true��
            eveManager.ActiveMissEvent(); //�~�X���̃C�x���g�𔭐�
        }

        //���Ɉڂ�Ȃ�
        if (nextFlag)
        {
            //var sc = FindObjectOfType<SceneController>();
            //sc.ReloadScene(); //�V�[���������[�h
        }
    }

    //���̃}�b�v��ݒ�
    public void SetNextMap(string nextName)
    {
        if (warpFlag) return; //���[�v�����ς݂Ȃ�I��

        nextMapName = nextName; //���̃}�b�v�����擾
        warpFlag = true; //���[�v�t���O��true��
        //�G�t�F�N�g�𔭐�
    }
}
