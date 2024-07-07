using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpeningRoll : MonoBehaviour
{
    //���͏��̍\����
    private struct RollData
    {
        public bool changeBack; //�w�i��ύX���邩
        public string roll; //���͂̓��e
    }

    [SerializeField]
    private TextAsset txtFile = null; //�\�����镶�̓t�@�C��
    [SerializeField]
    private TextMeshProUGUI rollText = null; //�\�����镶�͗p�̃I�u�W�F�N�g
    [SerializeField]
    private Image backImage = null; //�w�i�̉摜
    [SerializeField]
    private Image blackOut = null; //�Ó]�̉摜
    [SerializeField]
    private List<Sprite> backImageList; //�w�i�摜�̃��X�g
    [SerializeField]
    private string bgmName = "tukito_nostalgia.ogg"; //�Đ�����BGM

    private List<RollData> rolls; //�\�����镶��
    private Animator animator;
    private Coroutine moveCoroutine = null; //����p�̃R���[�`��

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g

    private int rollNum = 0; //�\�����镶�͂̔ԍ�
    private int backNum = 0; //�\������w�i�̔ԍ�
    private float rollR = 0.0f; //���͂�R�l
    private float rollG = 0.0f; //���͂�G�l
    private float rollB = 0.0f; //���͂�B�l

    private bool skipFlag = false; //�V�[�����ڂ���

    static private readonly float rollTimeMax = 5.0f; //���͂�\�����鎞��

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        animator = GetComponent<Animator>();
        sceneController = FindObjectOfType<SceneController>();
        rolls = new List<RollData>();
        LoadFile(); //�t�@�C�������[�h
        rollR = rollText.color.r; //���͂�R�l���擾
        rollG = rollText.color.g; //���͂�G�l���擾
        rollB = rollText.color.b; //���͂�B�l���擾
        backImage.sprite = backImageList[rollNum]; //�摜��ύX
        sceneController.SoundManager.PlayBgm(bgmName); //BGM���Đ�
        moveCoroutine = StartCoroutine(RollNextText(true)); //���͂̕\�����J�n
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFlag) return; //���Ɉڂ�Ȃ��΂�

        //�R���[�`�����I����Ă���Ȃ�
        if(moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(RollNextText(false)); //���̕��͂�\��
        }

        //�V�[�����I����Ȃ�
        if ((rolls.Count <= rollNum || sceneController.InputActions["Jump"].triggered) && !skipFlag)
        {
            //sManager.StopBgm(); //BGM���~
            skipFlag = true; //�ڍs�t���O��true��
            rollText.enabled = false; //���͂��\��
        }

        //�V�[���ړ�����Ȃ�
        if (skipFlag)
        {
            sceneController.SetNextScene("Title"); //�^�C�g���Ɉړ�
        }
    }

    private void FixedUpdate()
    {

    }

    //�t�@�C���̓ǂݍ���
    private void LoadFile()
    {
        string[] row = txtFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 0; i < row.Length; i++)
        {
            RollData rd = new RollData();
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            rd.changeBack = col[0].Contains("1"); //�w�i��ύX���邩�ݒ�
            rd.roll = col[1]; //���͂�ݒ�
            rolls.Add(rd); //���X�g�ɒǉ�
        }
    }

    //���̕��͂ɐi�߂�
    private IEnumerator RollNextText(bool firstFlag)
    {
        //�ŏ��ł͂Ȃ��Ȃ�
        if (!firstFlag)
        {
            rollNum++; //���͂�i�߂�
        }
        //�Ō�Ȃ�
        if(rolls.Count <= rollNum)
        {
            rollText.color = new Color(rollR, rollG, rollB, 0.0f); //���͂̓����x��ݒ�
            yield break; //�I��
        }
        rollText.text = rolls[rollNum].roll; //���͂�ύX
        animator.Play("RollTextFadeIn"); //�\���A�j���[�V�������Đ�
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
        blackOut.enabled = rolls[rollNum].changeBack; //�Ó]�̕\����ݒ�
        animator.Play("RollTextFadeOut"); //��\���A�j���[�V�������Đ�
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
        //�w�i��ς���Ȃ�
        if (rolls[rollNum].changeBack)
        {
            ChangeNextBack();
        }
        moveCoroutine = null; //�R���[�`����������
        yield return null;
    }

    //���̔w�i�ɕύX����
    private void ChangeNextBack()
    {
        //�Ō�łȂ����
        if(backNum + 1 < backImageList.Count)
        {
            backNum++; //�w�i�ԍ��𑝉�
            backImage.sprite = backImageList[backNum]; //�摜��ύX
        }
    }
}
