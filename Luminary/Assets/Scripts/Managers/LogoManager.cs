using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour
{
    [SerializeField]
    private Image logoImage = null; //�w�i�̉摜

    private float time = 0.0f; //���S��\�����Ă���̎���
    private float logoR = 0.0f; //�w�i��R�l
    private float logoG = 0.0f; //�w�i��G�l
    private float logoB = 0.0f; //�w�i��B�l
    private float logoAlpha = 0.0f; //�w�i�̃A���t�@�l

    private bool skipFlag = false; //�V�[�����ڂ���

    static private readonly float sceneTimeMax = 5.0f; //���̃V�[���Ɉڂ鎞��

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        logoR = logoImage.color.r; //���͂�R�l���擾
        logoG = logoImage.color.g; //���͂�G�l���擾
        logoB = logoImage.color.b; //���͂�B�l���擾
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFlag) return; //���Ɉڂ�Ȃ��΂�

        time += Time.deltaTime; //���Ԃ𑝉�

        //1�b�ŏo��
        if (time <= 1.0f)
        {
            logoAlpha = 1.0f * time;
        }
        //����ȊO�Ȃ當���͂͂�����
        else if (1.0f < time)
        {
            logoAlpha = 1.0f;
        }

        logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f); //�����x�̒l�𒲐�

        //�V�[���ړ��̎��ԂɂȂ�����
        if (sceneTimeMax <= time && !skipFlag)
        {
            skipFlag = true; //�ڍs�t���O��true��
        }

        //�V�[���ړ�����Ȃ�
        if (skipFlag)
        {
            var sc = FindObjectOfType<SceneController>();
            sc.SetNextScene("Opening"); //�I�[�v�j���O�Ɉړ�
        }
    }

    private void FixedUpdate()
    {
        logoImage.color = new Color(logoR, logoG, logoB, logoAlpha); //�摜�̓����x��ݒ�
    }
}
