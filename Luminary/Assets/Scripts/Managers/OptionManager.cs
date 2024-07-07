using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private Image volumeEnable = null; //選択していない箇所
    [SerializeField]
    private Image bgmVolumeMeter = null; //BGMの音量
    [SerializeField]
    private Image seVolumeMeter = null; //効果音の音量
    [SerializeField]
    private string bgmName = "tukito_color_of_the_light.ogg"; //再生するBGM

    private SceneController sceneController = null; //シーン管理用のオブジェクト
    private AudioSource sManSource; //BGM関連のAudioSource
    private AudioSource eManSource; //効果音関連のAudioSource
    private float bgmVolume; //BGMの音量
    private float seVolume; //効果音の音量

    private float yIn; //上下の入力
    private float xIn; //左右の入力
    private float xInTime = 0.0f; //左右の入力時間
    private bool backFlag = false; //タイトルに戻るか
    private int cursor = 0; //カーソルの位置

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        sceneController = FindObjectOfType<SceneController>(); //オブジェクトを取得
        sManSource = sceneController.SoundManager.GetComponent<AudioSource>();
        eManSource = sceneController.SoundEffectManager.GetComponent<AudioSource>();
        bgmVolume = sManSource.volume; //音量を取得
        seVolume = eManSource.volume; //音量を取得

        sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
    }

    // Update is called once per frame
    void Update()
    {
        float changeVolume = 0.0f; //音量の変化量

        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();

        yIn = stickInput.y; //上下の入力を取得
        //上下の入力があったら
        if (sceneController.InputActions["Move"].triggered && 0.0f < Mathf.Abs(yIn))
        {
            //上が押されたら
            if (0.0f < yIn)
            {
                cursor--; //カーソルを上に
            }
            //下が押されたら
            else
            {
                cursor++; //カーソルを下に
            }
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //効果音を再生
        }

        xIn = stickInput.x; //左右の入力を取得
        //左右の入力があったら
        if (0.0f < Mathf.Abs(xIn))
        {
            bool volumeFlag = false; //音量を変えるか
            //はじめての入力なら
            if (xInTime == 0.0f)
            {
                volumeFlag = true; //音量を変える
            }
            xInTime += Time.deltaTime; //入力時間を増加
            //0.1秒ごとに
            if(0.1f <= xInTime)
            {
                volumeFlag = true; //音量を変える
                xInTime %= 0.1f; //時間を減少
            }

            //音量を変えるなら
            if (volumeFlag)
            {
                //右が押されたら
                if (0.0f < xIn)
                {
                    changeVolume = 0.02f; //変化量を設定
                }
                //左が押されたら
                else
                {
                    changeVolume = -0.02f; //変化量を設定
                }
            }
        }
        //なければ
        else
        {
            xInTime = 0.0f; //入力時間を初期化
        }

        cursor = Mathf.Abs(cursor) % 2; //カーソルを調整

        //音量を変えるなら
        if (0.0f < Mathf.Abs(changeVolume))
        {
            //カーソルの位置によって処理
            switch (cursor)
            {
                //BGM音量
                case 0:
                    bgmVolume += changeVolume; //音量を変える
                    bgmVolume = Mathf.Clamp(bgmVolume, 0.0f, 1.0f); //音量を調整
                    sManSource.volume = bgmVolume;
                    sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //効果音を再生
                    break;
                //効果音音量
                case 1:
                    seVolume += changeVolume; //音量を変える
                    seVolume = Mathf.Clamp(seVolume, 0.0f, 1.0f); //音量を調整
                    eManSource.volume = seVolume;
                    sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //効果音を再生
                    break;
                default:
                    break;
            }
        }

        //決定が押されたら
        if (sceneController.InputActions["Jump"].triggered && !backFlag)
        {
            backFlag = true; //戻るフラグをtrueに
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //効果音を再生

            sceneController.SaveData.BgmVolume = bgmVolume; //音量を設定
            sceneController.SaveData.SeVolume = seVolume;
            sceneController.SaveData.WriteSaveData(); //音量を保存

            sceneController.SetNextScene("Title"); //タイトルに戻る
        }
    }

    private void FixedUpdate()
    {
        float enabledY = 216.0f * cursor - 72.0f; //選択していない箇所によって座標を設定
        volumeEnable.rectTransform.localPosition = new Vector3(0.0f, enabledY, 0.0f); //選択していない箇所によって移動

        bgmVolumeMeter.rectTransform.sizeDelta = new Vector2(bgmVolume * 800.0f, 144.0f); //音量によって画像の幅を設定
        seVolumeMeter.rectTransform.sizeDelta = new Vector2(seVolume * 800.0f, 144.0f); //音量によって画像の幅を設定
    }
}
