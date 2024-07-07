using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private Image volumeEnable = null; //�I�����Ă��Ȃ��ӏ�
    [SerializeField]
    private Image bgmVolumeMeter = null; //BGM�̉���
    [SerializeField]
    private Image seVolumeMeter = null; //���ʉ��̉���
    [SerializeField]
    private string bgmName = "tukito_color_of_the_light.ogg"; //�Đ�����BGM

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g
    private AudioSource sManSource; //BGM�֘A��AudioSource
    private AudioSource eManSource; //���ʉ��֘A��AudioSource
    private float bgmVolume; //BGM�̉���
    private float seVolume; //���ʉ��̉���

    private float yIn; //�㉺�̓���
    private float xIn; //���E�̓���
    private float xInTime = 0.0f; //���E�̓��͎���
    private bool backFlag = false; //�^�C�g���ɖ߂邩
    private int cursor = 0; //�J�[�\���̈ʒu

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        sceneController = FindObjectOfType<SceneController>(); //�I�u�W�F�N�g���擾
        sManSource = sceneController.SoundManager.GetComponent<AudioSource>();
        eManSource = sceneController.SoundEffectManager.GetComponent<AudioSource>();
        bgmVolume = sManSource.volume; //���ʂ��擾
        seVolume = eManSource.volume; //���ʂ��擾

        sceneController.SoundManager.PlayBgm(bgmName); //BGM���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        float changeVolume = 0.0f; //���ʂ̕ω���

        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();

        yIn = stickInput.y; //�㉺�̓��͂��擾
        //�㉺�̓��͂���������
        if (sceneController.InputActions["Move"].triggered && 0.0f < Mathf.Abs(yIn))
        {
            //�オ�����ꂽ��
            if (0.0f < yIn)
            {
                cursor--; //�J�[�\�������
            }
            //���������ꂽ��
            else
            {
                cursor++; //�J�[�\��������
            }
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //���ʉ����Đ�
        }

        xIn = stickInput.x; //���E�̓��͂��擾
        //���E�̓��͂���������
        if (0.0f < Mathf.Abs(xIn))
        {
            bool volumeFlag = false; //���ʂ�ς��邩
            //�͂��߂Ă̓��͂Ȃ�
            if (xInTime == 0.0f)
            {
                volumeFlag = true; //���ʂ�ς���
            }
            xInTime += Time.deltaTime; //���͎��Ԃ𑝉�
            //0.1�b���Ƃ�
            if(0.1f <= xInTime)
            {
                volumeFlag = true; //���ʂ�ς���
                xInTime %= 0.1f; //���Ԃ�����
            }

            //���ʂ�ς���Ȃ�
            if (volumeFlag)
            {
                //�E�������ꂽ��
                if (0.0f < xIn)
                {
                    changeVolume = 0.02f; //�ω��ʂ�ݒ�
                }
                //���������ꂽ��
                else
                {
                    changeVolume = -0.02f; //�ω��ʂ�ݒ�
                }
            }
        }
        //�Ȃ����
        else
        {
            xInTime = 0.0f; //���͎��Ԃ�������
        }

        cursor = Mathf.Abs(cursor) % 2; //�J�[�\���𒲐�

        //���ʂ�ς���Ȃ�
        if (0.0f < Mathf.Abs(changeVolume))
        {
            //�J�[�\���̈ʒu�ɂ���ď���
            switch (cursor)
            {
                //BGM����
                case 0:
                    bgmVolume += changeVolume; //���ʂ�ς���
                    bgmVolume = Mathf.Clamp(bgmVolume, 0.0f, 1.0f); //���ʂ𒲐�
                    sManSource.volume = bgmVolume;
                    sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //���ʉ����Đ�
                    break;
                //���ʉ�����
                case 1:
                    seVolume += changeVolume; //���ʂ�ς���
                    seVolume = Mathf.Clamp(seVolume, 0.0f, 1.0f); //���ʂ𒲐�
                    eManSource.volume = seVolume;
                    sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //���ʉ����Đ�
                    break;
                default:
                    break;
            }
        }

        //���肪�����ꂽ��
        if (sceneController.InputActions["Jump"].triggered && !backFlag)
        {
            backFlag = true; //�߂�t���O��true��
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //���ʉ����Đ�

            sceneController.SaveData.BgmVolume = bgmVolume; //���ʂ�ݒ�
            sceneController.SaveData.SeVolume = seVolume;
            sceneController.SaveData.WriteSaveData(); //���ʂ�ۑ�

            sceneController.SetNextScene("Title"); //�^�C�g���ɖ߂�
        }
    }

    private void FixedUpdate()
    {
        float enabledY = 216.0f * cursor - 72.0f; //�I�����Ă��Ȃ��ӏ��ɂ���č��W��ݒ�
        volumeEnable.rectTransform.localPosition = new Vector3(0.0f, enabledY, 0.0f); //�I�����Ă��Ȃ��ӏ��ɂ���Ĉړ�

        bgmVolumeMeter.rectTransform.sizeDelta = new Vector2(bgmVolume * 800.0f, 144.0f); //���ʂɂ���ĉ摜�̕���ݒ�
        seVolumeMeter.rectTransform.sizeDelta = new Vector2(seVolume * 800.0f, 144.0f); //���ʂɂ���ĉ摜�̕���ݒ�
    }
}
