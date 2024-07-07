using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private string bgmName = "tukito_search_for_ideals.ogg"; //�Đ�����BGM
    [SerializeField]
    private GameObject[] choice = null; //�I����

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g

    private bool nextFlag = false; //���̃V�[���Ɉڂ邩
    private int cursor = 0; //�J�[�\���̈ʒu

    static readonly float UnableColor = 0.5f; //��I�����̐F
    static readonly List<string> sceneList = new List<string>
    {
        "StageSelect",
        "Option",
    };

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        sceneController = FindObjectOfType<SceneController>(); //�I�u�W�F�N�g���擾

        //�I�����̐F��ύX
        for (int i = 0; i < choice.Length; i++)
        {
            //�I������Ă���Ȃ�
            if (i == cursor)
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f); //���ʂ̐F��
            }
            //����Ă��Ȃ��Ȃ�
            else
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(UnableColor, UnableColor, UnableColor); //��I�����̐F��
            }
        }

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
            if(0.0f < stickInput.y)
            {
                cursor = (cursor - 1 + choice.Length) % choice.Length; //�J�[�\�������
            }
            //���������ꂽ��
            else
            {
                cursor = (cursor + 1) % choice.Length; //�J�[�\��������
            }
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //���ʉ����Đ�
        }

        //���肪�����ꂽ��
        if(sceneController.InputActions["Jump"].triggered && !nextFlag)
        {
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //���ʉ����Đ�
            sceneController.EffectManager.CreateEffect(EffectNameList.EFFECT_HIT, choice[cursor].transform.position, choice[cursor].transform.rotation);  //�G�t�F�N�g�𔭐�������
        }

        //�߂�Ȃ�
        if (sceneController.InputActions["Fire"].triggered && !nextFlag)
        {
            sceneController.SoundManager.StopBgm(); //BGM���~
            nextFlag = true; //���Ɉڂ�t���O��true��
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CANCEL); //���ʉ����Đ�
        }

        //���Ɉڂ�Ȃ�
        if(nextFlag)
        {
            //�I������Ȃ�
            if(cursor == choice.Length - 1)
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #elif UNITY_STANDALONE
                    Application.Quit();
                #endif
            }
            //����ȊO�Ȃ�
            else
            {
                sceneController.SetNextScene(sceneList[cursor]);
            }
        }
    }

    private void FixedUpdate()
    {
        //�I�����̐F��ύX
        for(int i = 0; i < choice.Length; i++)
        {
            //�I������Ă���Ȃ�
            if(i == cursor)
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f); //���ʂ̐F��
            }
            //����Ă��Ȃ��Ȃ�
            else
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(UnableColor, UnableColor, UnableColor); //��I�����̐F��
            }
        }
    }
}
