using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private string firstSceneName = "MakerLogo"; //�ŏ��Ƀ��[�h����V�[��

    [SerializeField]
    private SoundManager soundManager = null; //BGM�֘A�̃I�u�W�F�N�g

    [SerializeField]
    private SoundEffectManager soundEffectManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    [SerializeField]
    private EffectManager effectManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    [SerializeField]
    private SaveData saveData = null; //�Z�[�u�֘A�̃I�u�W�F�N�g

    private FadeOut fadeOut = null; //�Ó]�p�̃I�u�W�F�N�g

    private Camera mainCamera; //�J�����̃I�u�W�F�N�g
    private PlayerInput playerInput; //���͂̃I�u�W�F�N�g
    Scene presentScene; //���݂̃V�[��
    private string nextSceneName = null; //���̃V�[��

    public SoundManager SoundManager { get { return soundManager; } } //BGM�֘A�̃I�u�W�F�N�g
    public SoundEffectManager SoundEffectManager { get { return soundEffectManager; } } //���ʉ��֘A�̃I�u�W�F�N�g
    public EffectManager EffectManager { get { return effectManager; } } //�G�t�F�N�g�֘A�̃I�u�W�F�N�g
    public SaveData SaveData { get{ return saveData; } } //�Z�[�u�֘A�̃I�u�W�F�N�g
    public PlayerInput PlayerInput { get { return playerInput; } } //���͂̃I�u�W�F�N�g
    public InputActionMap InputActions { get { return playerInput.currentActionMap; } }

    private void Awake()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPS��60��
        //�I�u�W�F�N�g�����łɑ��݂���Ȃ�
        if (1 < FindObjectsOfType<SceneController>().Length)
        {
            Destroy(gameObject); //�I�u�W�F�N�g������
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOut = FindObjectOfType<FadeOut>(); //�I�u�W�F�N�g���擾
        playerInput = GetComponent<PlayerInput>(); //�I�u�W�F�N�g���擾
        mainCamera = Camera.main; //�I�u�W�F�N�g���擾
        mainCamera.enabled = false; //�J�����𖳌���
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Additive); //�ŏ��̃V�[�������[�h
        presentScene = SceneManager.GetSceneByName(firstSceneName); //�V�[�����擾
    }

    //�V�[�����ړ�
    IEnumerator ChangeScene()
    {
        fadeOut.FadeOutStart(); //�Ó]�J�n
        //�Ó]�ł���܂őҋ@
        while (!fadeOut.FadeOutEndFlag)
        {
            yield return null;
        }
        mainCamera.enabled = true; //�J������L����
        SceneManager.UnloadSceneAsync(presentScene); //���݂̃V�[�����A�����[�h
        //�A�����[�h�܂őҋ@
        while (presentScene.isLoaded)
        {
            yield return null;
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive); //�V�[�������[�h
        //���[�h�ł���܂őҋ@
        while (!ao.isDone)
        {
            yield return null;
        }
        mainCamera.enabled = false; //�J�����𖳌���
        presentScene = SceneManager.GetSceneByName(nextSceneName); //���̃V�[�����擾
        SceneManager.SetActiveScene(presentScene); //�V�[�����A�N�e�B�u��
        fadeOut.FadeOutEnd(); //�Ó]�I��
    }

    //���̃V�[����ݒ�
    public void SetNextScene(string nextName)
    {
        nextSceneName = nextName; //���̃V�[���ԍ���ݒ�
        StartCoroutine(ChangeScene());
    }

    //�V�[����߂�
    public void BackScene()
    {

    }

    //���݂̃V�[���������[�h
    public void ReloadScene()
    {
        nextSceneName = presentScene.name;
        StartCoroutine(ChangeScene());
    }
}
