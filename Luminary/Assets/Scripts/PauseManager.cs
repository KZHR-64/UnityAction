using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private Image[] choice = null; //�I����

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g
    private SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    private float closeTime = 0.0f; //����܂ł̎���
    private bool closeFlag = false; //�|�[�Y��ʂ���邩
    private bool backFlag = false; //�X�e�[�W�Z���N�g�ɖ߂邩
    private int cursor = 0; //�J�[�\���̈ʒu

    static readonly float UnableColor = 0.5f; //��I�����̐F

    public bool BackFlag { get { return backFlag && 0.5f < closeTime; } } //�X�e�[�W�Z���N�g�ɖ߂邩

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        eManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();

        //�|�[�Y��ʂ����Ȃ�
        if (closeFlag)
        {
            //��莞�Ԍo�߂�����
            if(0.5 <= closeTime)
            {
                Time.timeScale = 1.0f; //���Ԃ�i�߂�
                //�X�e�[�W�Z���N�g�ɖ߂�Ȃ�
                if (backFlag)
                {
                }
                //�ĊJ����Ȃ�
                else
                {
                    gameObject.SetActive(false); //�I�u�W�F�N�g�����
                }
            }
            closeTime += Time.fixedDeltaTime; //���Ԃ𑝉�
            return; //�I��
        }

        //�㉺�̓��͂���������
        if (sceneController.InputActions["Move"].triggered)
        {
            //�オ�����ꂽ��
            if (0.0f < stickInput.y)
            {
                cursor--; //�J�[�\�������
            }
            //���������ꂽ��
            else
            {
                cursor++; //�J�[�\��������
            }
            eManager.PlaySe(SeNameList.SE_CURSOR); //���ʉ����Đ�
        }
        cursor = Mathf.Clamp(cursor, 0, 1); //�J�[�\���𒲐�

        //���肪�����ꂽ��
        if (sceneController.InputActions["Jump"].triggered)
        {
            //�J�[�\���̈ʒu�ɂ���ď���
            switch(cursor)
            {
                case 0:
                    backFlag = false; //�߂�t���O��false��
                    break;
                case 1:
                    backFlag = true; //�߂�t���O��true��
                    break;
                default:
                    break;
            }
            closeFlag = true; //����t���O��true��
            eManager.PlaySe(SeNameList.SE_DECISION); //���ʉ����Đ�
        }

        //�߂�Ȃ�
        if (sceneController.InputActions["Fire"].triggered)
        {
            closeFlag = true; //����t���O��true��
            backFlag = false; //�߂�t���O��false��
            eManager.PlaySe(SeNameList.SE_CANCEL); //���ʉ����Đ�
        }

        //�I�����̐F��ύX
        for (int i = 0; i < choice.Length; i++)
        {
            //�I������Ă���Ȃ�
            if (i == cursor)
            {
                choice[i].color = new Color(1.0f, 1.0f, 1.0f); //���ʂ̐F��
            }
            //����Ă��Ȃ��Ȃ�
            else
            {
                choice[i].color = new Color(UnableColor, UnableColor, UnableColor); //��I�����̐F��
            }
        }
    }

    //�|�[�Y��ʂ̋N��
    public void PauseActivate()
    {
        closeFlag = false; //����t���O��false��
        backFlag = false; //�߂�t���O��false��
        closeTime = 0.0f; //���Ԃ�������
        Time.timeScale = 0.0f; //���Ԃ��~�߂�
    }
}
