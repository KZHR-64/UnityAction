using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset txtFile = null; //�\�����镶�̓t�@�C��
    [SerializeField]
    private TextMeshProUGUI rollText = null; //�\�����镶�͗p�̃I�u�W�F�N�g
    [SerializeField]
    private string bgmName = "tukito287_gorgeous_moment_acoustic.ogg"; //�Đ�����BGM

    List<string> rollList = null; //�X�^�b�t���[���̃��X�g

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g
    private Animator animator;
    private Coroutine moveCoroutine = null; //����p�̃R���[�`��

    private int rollNum = 0; //�\�����镶�͂̔ԍ�
    private bool rollEndFlag = false; //���͂��Ō�ɒB���Ă��邩
    private bool backFlag = false; //�^�C�g���ɖ߂邩

    static private readonly float rollTimeMax = 5.0f; //���͂�\�����鎞��

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        animator = GetComponent<Animator>();
        sceneController = FindObjectOfType<SceneController>(); //�I�u�W�F�N�g���擾

        rollList = new List<string>();
        LoadFile(); //�t�@�C����ǂݍ���

        sceneController.SoundManager.PlayBgm(bgmName); //BGM���Đ�
        moveCoroutine = StartCoroutine(SetNextText(true)); //���͂̕\���J�n
    }

    // Update is called once per frame
    void Update()
    {
        if (backFlag) return; //���Ɉڂ�Ȃ��΂�

        //�R���[�`�����I����Ă���Ȃ�
        if (moveCoroutine == null && !rollEndFlag)
        {
            moveCoroutine = StartCoroutine(SetNextText(false)); //���̕��͂�\��
        }

        //�|�[�Y�������ꂽ��
        if (sceneController.InputActions["Pause"].triggered && !backFlag)
        {
            backFlag = true; //�߂�t���O��true��

            sceneController.SetNextScene("Title"); //�^�C�g���ɖ߂�
        }
    }

    //�t�@�C���̓ǂݍ���
    private void LoadFile()
    {
        string[] row = txtFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 0; i < row.Length; i++)
        {
            string roll = row[i].Replace(';', '\n'); //�Z�~�R���������s�ɕϊ�
            rollList.Add(roll); //���X�g�ɒǉ�
        }
    }

    //���͂̕ύX
    private IEnumerator SetNextText(bool firstFlag)
    {
        //�ŏ��ł͂Ȃ��Ȃ�
        if (!firstFlag)
        {
            rollNum++; //���͂�i�߂�
        }
        //�Ō�Ȃ�
        if (rollList.Count <= rollNum)
        {
            rollEndFlag = true; //�I���t���O��true��
            yield break; //�I��
        }
        rollText.text = rollList[rollNum]; //���͂�ύX
        animator.Play("EndingRollFadeIn"); //�\���A�j���[�V�������Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(rollTimeMax); //���΂炭���͂�\��
        animator.Play("EndingRollFadeOut"); //��\���A�j���[�V�������Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        moveCoroutine = null; //�R���[�`����������
        yield return null;
    }
}