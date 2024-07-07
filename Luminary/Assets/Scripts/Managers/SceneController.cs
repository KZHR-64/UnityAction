using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private string firstSceneName = "MakerLogo"; //最初にロードするシーン

    [SerializeField]
    private SoundManager soundManager = null; //BGM関連のオブジェクト

    [SerializeField]
    private SoundEffectManager soundEffectManager = null; //効果音関連のオブジェクト

    [SerializeField]
    private EffectManager effectManager = null; //エフェクト関連のオブジェクト

    [SerializeField]
    private SaveData saveData = null; //セーブ関連のオブジェクト

    private FadeOut fadeOut = null; //暗転用のオブジェクト

    private Camera mainCamera; //カメラのオブジェクト
    private PlayerInput playerInput; //入力のオブジェクト
    Scene presentScene; //現在のシーン
    private string nextSceneName = null; //次のシーン

    public SoundManager SoundManager { get { return soundManager; } } //BGM関連のオブジェクト
    public SoundEffectManager SoundEffectManager { get { return soundEffectManager; } } //効果音関連のオブジェクト
    public EffectManager EffectManager { get { return effectManager; } } //エフェクト関連のオブジェクト
    public SaveData SaveData { get{ return saveData; } } //セーブ関連のオブジェクト
    public PlayerInput PlayerInput { get { return playerInput; } } //入力のオブジェクト
    public InputActionMap InputActions { get { return playerInput.currentActionMap; } }

    private void Awake()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        //オブジェクトがすでに存在するなら
        if (1 < FindObjectsOfType<SceneController>().Length)
        {
            Destroy(gameObject); //オブジェクトを消す
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOut = FindObjectOfType<FadeOut>(); //オブジェクトを取得
        playerInput = GetComponent<PlayerInput>(); //オブジェクトを取得
        mainCamera = Camera.main; //オブジェクトを取得
        mainCamera.enabled = false; //カメラを無効に
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Additive); //最初のシーンをロード
        presentScene = SceneManager.GetSceneByName(firstSceneName); //シーンを取得
    }

    //シーンを移動
    IEnumerator ChangeScene()
    {
        fadeOut.FadeOutStart(); //暗転開始
        //暗転できるまで待機
        while (!fadeOut.FadeOutEndFlag)
        {
            yield return null;
        }
        mainCamera.enabled = true; //カメラを有効に
        SceneManager.UnloadSceneAsync(presentScene); //現在のシーンをアンロード
        //アンロードまで待機
        while (presentScene.isLoaded)
        {
            yield return null;
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive); //シーンをロード
        //ロードできるまで待機
        while (!ao.isDone)
        {
            yield return null;
        }
        mainCamera.enabled = false; //カメラを無効に
        presentScene = SceneManager.GetSceneByName(nextSceneName); //次のシーンを取得
        SceneManager.SetActiveScene(presentScene); //シーンをアクティブに
        fadeOut.FadeOutEnd(); //暗転終了
    }

    //次のシーンを設定
    public void SetNextScene(string nextName)
    {
        nextSceneName = nextName; //次のシーン番号を設定
        StartCoroutine(ChangeScene());
    }

    //シーンを戻る
    public void BackScene()
    {

    }

    //現在のシーンをリロード
    public void ReloadScene()
    {
        nextSceneName = presentScene.name;
        StartCoroutine(ChangeScene());
    }
}
