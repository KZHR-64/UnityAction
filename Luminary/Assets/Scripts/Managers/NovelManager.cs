using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class NovelManager : MonoBehaviour
{
    //��b���̍\����
    private struct RollDataStruct
    {
        public int type; //��ށi0=��b�A1=�ҋ@�j
        public int[] imageNum; //�����G�̔ԍ�
        public bool blackOut; //�Ó]�����邩
        public string backName; //�ύX����w�i
        public string soundEffect; //���ʉ�
        public string name; //�b�Җ�
        public string talk; //��b���e
    }

    [SerializeField]
    private Image namePanel = null; //���O�\���p�̃I�u�W�F�N�g
    [SerializeField]
    private TextMeshProUGUI nameText = null; //�\�����閼�O�p�̃I�u�W�F�N�g
    [SerializeField]
    private TextMeshProUGUI messageText = null; //�\�����镶�͗p�̃I�u�W�F�N�g
    [SerializeField]
    private Image backImage = null; //�w�i�̉摜
    [SerializeField]
    private List<Image> charaImageList; //�����G�̉摜
    [SerializeField]
    private List<Sprite> charaSpriteList; //�����G�̉摜

    private Animator animator;
    private TextAsset txtFile = null; //�\�����镶�̓t�@�C��
    private string bgmName = null; //�Đ�����BGM
    private string nextSceneName = null; //���̃V�[����
    private List<RollDataStruct> rolls; //�\�����镶��
    private bool rollFlag = false; //���͂�i�߂Ă��邩
    private bool blackOutFlag = false; //�Ó]���Ă��邩
    private int rollNum = 0; //�\�����镶�͂̔ԍ�
    private float time = 0.0f; //�^�C�}�[
    private AsyncOperationHandle<Sprite> handle; //BGM�̃n���h��

    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g

    private bool inputFlag = false; //���肪������Ă��邩
    private bool skipInputFlag = false; //�|�[�Y��������Ă��邩

    private bool skipFlag = false; //�V�[�����ڂ���

    private readonly float talkSpeed = 0.05f; //���͂̕\���X�s�[�h

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        AsyncOperationHandle <TextAsset> ta = Addressables.LoadAssetAsync<TextAsset>(sceneController.SaveData.NovelFileName); //�t�@�C�������[�h
        txtFile = ta.WaitForCompletion(); //�t�@�C�����擾
        LoadFile(); //�t�@�C���̓ǂݍ���
        Addressables.Release(ta); //�n���h�������

        //���O���\��
        namePanel.enabled = false;
        nameText.enabled = false;

        //�����G���\��
        for (int i = 0; i < charaImageList.Count; i++)
        {
            charaImageList[i].enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        messageText.text = "";
        
        //BGM������Ȃ�
        if(0 < bgmName.Length)
        {
            sceneController.SoundManager.PlayBgm(bgmName); //BGM���Đ�
        }

        SetRoll(); //���̕��͂�ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFlag) return; //���Ɉڂ�Ȃ��΂�

        //���͂̊m�F
        inputFlag = sceneController.InputActions["Jump"].triggered;
        skipInputFlag = sceneController.InputActions["Pause"].triggered;

        //�X�L�b�v����Ȃ�
        if(skipInputFlag)
        {
            skipFlag = true; //���Ɉڂ�t���O��true��
        }

        //�Ō�ɒB���Ă��Ȃ����
        if (rollNum < rolls.Count && !rollFlag)
        {
            //���肪�����ꂽ��
            if (inputFlag)
            {
                sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //���ʉ����Đ�
                rollNum++; //���̉�b��ݒ�
                //�Ō�ɒB�����Ȃ�
                if (rollNum == rolls.Count)
                {
                    skipFlag = true; //���Ɉڂ�t���O��true��
                }
                else
                {
                    SetRoll(); //���̕��͂�ݒ�
                }
            }
        }

        //�V�[���ړ�����Ȃ�
        if (skipFlag)
        {
            sceneController.SetNextScene(nextSceneName); //�^�C�g���Ɉړ�
        }
    }

    //���̕��͂�ݒ�
    private void SetRoll()
    {
        //�����G��ݒ�
        for(int i = 0; i < charaImageList.Count; i++)
        {
            //��\���ݒ�Ȃ�
            if(rolls[rollNum].imageNum[i] == -2)
            {
                charaImageList[i].enabled = false; //�摜���\��
            }
            //�����ł͂Ȃ�0�ȏ�Ȃ�
            else if(0 <= rolls[rollNum].imageNum[i])
            {
                charaImageList[i].enabled = true; //�摜��\��
                charaImageList[i].sprite = charaSpriteList[rolls[rollNum].imageNum[i]]; //�摜��ݒ�
            }
        }

        //���ʉ�������Ȃ�
        if(0 < rolls[rollNum].soundEffect.Length)
        {
            sceneController.SoundEffectManager.PlaySe(rolls[rollNum].soundEffect); //���ʉ����Đ�
        }

        //���O������Ȃ�
        if (0 < rolls[rollNum].name.Length)
        {
            //���O��\��
            nameText.text = rolls[rollNum].name;
            namePanel.enabled = true;
            nameText.enabled = true;
        }
        //�Ȃ��Ȃ�
        else
        {
            //���O���\��
            namePanel.enabled = false;
            nameText.enabled = false;
        }

        StartCoroutine(RollText(rolls[rollNum].talk, rolls[rollNum].blackOut, rolls[rollNum].backName)); //���͂̕\���J�n
    }

    //�t�@�C���̃��[�h
    private void LoadFile()
    {
        rolls = new List<RollDataStruct>();
        string[] row = txtFile.text.Split('\n'); //�s���Ƃɕ���
        bgmName = row[0].TrimEnd(); //BGM��ݒ�
        handle = Addressables.LoadAssetAsync<Sprite>(row[1].TrimEnd()); //�w�i�����[�h
        backImage.sprite = handle.WaitForCompletion(); //�w�i��ݒ�
        nextSceneName = row[2].TrimEnd(); //���̃V�[����ݒ�
        //��s���ǂݍ���
        for (int i = 3; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            RollDataStruct rollData = new RollDataStruct();
            rollData.imageNum = new int[3];
            rollData.type = int.Parse(col[0]); //��ނ�ݒ�
            rollData.imageNum[0] = int.Parse(col[1]); //�摜�ԍ���ݒ�
            rollData.imageNum[1] = int.Parse(col[2]); //�摜�ԍ���ݒ�
            rollData.imageNum[2] = int.Parse(col[3]); //�摜�ԍ���ݒ�
            rollData.blackOut = col[4].Contains("1"); //�Ó]��؂�ւ��邩�ݒ�
            rollData.backName = col[5]; //�؂�ւ���w�i��ݒ�
            rollData.soundEffect = col[6]; //���ʉ���ݒ�
            rollData.name = col[7].TrimEnd(); //���e��ݒ�
            rollData.talk = col[8].TrimEnd(); //���e��ݒ�
            rolls.Add(rollData); //�������X�g�ɒǉ�
        }
    }

    //���͂��������\������
    private IEnumerator RollText(string roll, bool boFlag, string backName)
    {
        rollFlag = true; //�i�߂Ă���t���O��true��
        time = 0.0f; //���Ԃ�������
        messageText.text = ""; //�e�L�X�g��������
        //�w�i��ς���Ȃ�
        if(0 < backName.Length)
        {
            backImage.sprite = null; //�w�i���폜
            Addressables.Release(handle); //�w�i�̃n���h�������
            handle = Addressables.LoadAssetAsync<Sprite>(backName.TrimEnd()); //�w�i�����[�h
            backImage.sprite = handle.WaitForCompletion(); //�w�i��ݒ�
        }

        //�Ó]��؂�ւ���Ȃ�
        if(boFlag)
        {
            string animName = blackOutFlag ? "NovelFadeIn" : "NovelFadeOut"; //�Ó]�󋵂ɂ���ăA�j���[�V������ݒ�
            animator.Play(animName); //�\���A�j���[�V�������Đ�
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
            blackOutFlag = !blackOutFlag; //�Ó]�t���O��؂�ւ�
        }

        yield return new WaitForSeconds(0.1f); //�����҂�

        //���͂��Ȃ��Ȃ�
        if(roll.Length <= 0)
        {
            rollFlag = false; //�i�߂Ă���t���O��false��
            time = 0.0f; //���Ԃ�������
            yield break; //�I��
        }

        while (true)
        {
            time += Time.deltaTime; //���Ԃ𑝉�

            if (inputFlag || skipInputFlag) break; //���͂���������I��

            int strLength = Mathf.FloorToInt(time / talkSpeed); //���Ԃɉ����ĕ\�����镶�͗ʂ�ݒ�
            if (roll.Length <= strLength) break;//�ő�ɒB������I��
            messageText.text = roll.Substring(0, strLength); //�\�����镶��ݒ�

            yield return null;
        }
        rollFlag = false; //�i�߂Ă���t���O��false��
        time = 0.0f; //���Ԃ�������
        messageText.text = roll; //�S����\��
        yield return null;
    }
}
