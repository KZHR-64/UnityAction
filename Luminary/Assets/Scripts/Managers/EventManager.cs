using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private MessageWindow msgWindow = null; //���b�Z�[�W�E�C���h�E
    [SerializeField]
    private TextMeshProUGUI talkText = null; //�\�����镶�͗p�̃I�u�W�F�N�g
    [SerializeField]
    private MissEvent missEvent = null; //�~�X�������̃C�x���g
    [SerializeField]
    private BeatBossEvent beatBossEvent = null; //�{�X���j���̃C�x���g
    [SerializeField]
    private Manager manager = null; //�Q�[���Ǘ��̃I�u�W�F�N�g
    [SerializeField]
    private Player player = null; //���@�̃I�u�W�F�N�g

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g
    private SoundManager soundManager = null; //BGM�֘A�̃I�u�W�F�N�g
    private SoundEffectManager soundEffectManager = null; //���ʉ��֘A�̃I�u�W�F�N�g
    private EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    private bool clearFlag = false; //�N���A������
    private bool missFlag = false; //�~�X������
    private bool waitFlag = false; //�ҋ@���邩
    private bool gameStopFlag = false; //�Q�[���i�s���~�߂邩
    private int gameStopNum = 0; //�Q�[�����~�߂Ă���C�x���g��

    public bool ClearFlag { get { return clearFlag; } set { clearFlag = value; } } //�N���A������
    public bool MissFlag { get { return missFlag; } set { missFlag = value; } } //�~�X������
    public bool WaitFlag { get { return waitFlag; } set { waitFlag = value; } } //�ҋ@���邩
    public bool GameStopFlag { get { return gameStopFlag; }} //�ҋ@���邩
    public MessageWindow MsgWindow { get { return msgWindow; } } //���b�Z�[�W�E�C���h�E
    public TextMeshProUGUI TalkText { get { return talkText; } } //�\�����镶�͗p�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>(); //�I�u�W�F�N�g���擾
        soundManager = sceneController.SoundManager; //�I�u�W�F�N�g���擾
        soundEffectManager = sceneController.SoundEffectManager; //�I�u�W�F�N�g���擾
        effManager = sceneController.EffectManager; //�I�u�W�F�N�g���擾

        AbstructEvent[] eventList = GetComponentsInChildren<AbstructEvent>(); //���łɂ���C�x���g���擾
        //�C�x���g������Ȃ�
        if(0 < eventList.Length)
        {
            foreach(AbstructEvent ae in eventList)
            {
                ae.FirstSetting(this, player, soundManager, soundEffectManager); //�ŏ��̍X�V
            }
        }
        EventPoint[] eventPoint = GetComponentsInChildren<EventPoint>();
        //�C�x���g������Ȃ�
        if (0 < eventPoint.Length)
        {
            foreach (EventPoint ep in eventPoint)
            {
                ep.FirstSetting(this); //�ŏ��̍X�V
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�~�X�����C�x���g���J�n
    public void ActiveMissEvent()
    {
        missEvent.ActiveFlag = true; //�C�x���g���J�n
    }

    //�{�X���j�C�x���g���J�n
    public void ActiveBeatBossEvent()
    {
        beatBossEvent.Activate();
    }

    //���͂��m�F
    public bool CheckButtonInput()
    {
        //�W�����v���U���{�^���������ꂽ���Ƃ��m�F
        bool jumpInput = sceneController.PlayerInput.actions["Jump"].triggered;
        bool fireInput = sceneController.PlayerInput.actions["Fire"].triggered;
        bool fire2Input = sceneController.PlayerInput.actions["Fire2"].triggered;
        return jumpInput || fireInput || fire2Input;
    }

    //���̃X�e�[�W�Ɉړ�
    public void WarpNextMap(string mapName)
    {
        soundEffectManager.PlaySe("warp.ogg"); //���ʉ����Đ�
        manager.SetNextMap(mapName); //���̃}�b�v�̈ړ�����
    }

    //�Q�[���̐i�s���~
    public void StopGame()
    {
        gameStopNum++; //��~���Ă���C�x���g���𑝉�
        gameStopFlag = true; //��~�t���O��true��
    }

    //�Q�[�����ĊJ
    public void RestartGame()
    {
        gameStopNum--; //��~��������
        gameStopNum = Mathf.Max(0, gameStopNum);

        //��~����0�ɂȂ�����
        if(gameStopNum <= 0)
        {
            gameStopFlag = false; //��~�t���O��false��
        }
    }
}
