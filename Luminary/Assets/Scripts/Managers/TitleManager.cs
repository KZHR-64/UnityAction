using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private string bgmName = "tukito_search_for_ideals.ogg"; //再生するBGM
    [SerializeField]
    private GameObject[] choice = null; //選択肢

    private SceneController sceneController = null; //シーン管理用のオブジェクト

    private bool nextFlag = false; //次のシーンに移るか
    private int cursor = 0; //カーソルの位置

    static readonly float UnableColor = 0.5f; //非選択時の色
    static readonly List<string> sceneList = new List<string>
    {
        "StageSelect",
        "Option",
    };

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        sceneController = FindObjectOfType<SceneController>(); //オブジェクトを取得

        //選択肢の色を変更
        for (int i = 0; i < choice.Length; i++)
        {
            //選択されているなら
            if (i == cursor)
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f); //普通の色に
            }
            //されていないなら
            else
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(UnableColor, UnableColor, UnableColor); //非選択時の色に
            }
        }

        sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
    }

    // Update is called once per frame
    void Update()
    {
        if (nextFlag) return; //次に移るなら飛ばす

        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();

        //上下の入力があったら
        if (sceneController.InputActions["Move"].triggered)
        {
            //上が押されたら
            if(0.0f < stickInput.y)
            {
                cursor = (cursor - 1 + choice.Length) % choice.Length; //カーソルを上に
            }
            //下が押されたら
            else
            {
                cursor = (cursor + 1) % choice.Length; //カーソルを下に
            }
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CURSOR); //効果音を再生
        }

        //決定が押されたら
        if(sceneController.InputActions["Jump"].triggered && !nextFlag)
        {
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_DECISION); //効果音を再生
            sceneController.EffectManager.CreateEffect(EffectNameList.EFFECT_HIT, choice[cursor].transform.position, choice[cursor].transform.rotation);  //エフェクトを発生させる
        }

        //戻るなら
        if (sceneController.InputActions["Fire"].triggered && !nextFlag)
        {
            sceneController.SoundManager.StopBgm(); //BGMを停止
            nextFlag = true; //次に移るフラグをtrueに
            sceneController.SoundEffectManager.PlaySe(SeNameList.SE_CANCEL); //効果音を再生
        }

        //次に移るなら
        if(nextFlag)
        {
            //終了するなら
            if(cursor == choice.Length - 1)
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #elif UNITY_STANDALONE
                    Application.Quit();
                #endif
            }
            //それ以外なら
            else
            {
                sceneController.SetNextScene(sceneList[cursor]);
            }
        }
    }

    private void FixedUpdate()
    {
        //選択肢の色を変更
        for(int i = 0; i < choice.Length; i++)
        {
            //選択されているなら
            if(i == cursor)
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f); //普通の色に
            }
            //されていないなら
            else
            {
                choice[i].GetComponent<Renderer>().material.color = new Color(UnableColor, UnableColor, UnableColor); //非選択時の色に
            }
        }
    }
}
