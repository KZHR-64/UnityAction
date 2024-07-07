using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private Image[] choice = null; //選択肢

    private SceneController sceneController = null; //シーン管理用のオブジェクト
    private SoundEffectManager eManager = null; //効果音関連のオブジェクト

    private float closeTime = 0.0f; //閉じるまでの時間
    private bool closeFlag = false; //ポーズ画面を閉じるか
    private bool backFlag = false; //ステージセレクトに戻るか
    private int cursor = 0; //カーソルの位置

    static readonly float UnableColor = 0.5f; //非選択時の色

    public bool BackFlag { get { return backFlag && 0.5f < closeTime; } } //ステージセレクトに戻るか

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        eManager = FindObjectOfType<SoundEffectManager>(); //オブジェクトを取得
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();

        //ポーズ画面を閉じるなら
        if (closeFlag)
        {
            //一定時間経過したら
            if(0.5 <= closeTime)
            {
                Time.timeScale = 1.0f; //時間を進める
                //ステージセレクトに戻るなら
                if (backFlag)
                {
                }
                //再開するなら
                else
                {
                    gameObject.SetActive(false); //オブジェクトを閉じる
                }
            }
            closeTime += Time.fixedDeltaTime; //時間を増加
            return; //終了
        }

        //上下の入力があったら
        if (sceneController.InputActions["Move"].triggered)
        {
            //上が押されたら
            if (0.0f < stickInput.y)
            {
                cursor--; //カーソルを上に
            }
            //下が押されたら
            else
            {
                cursor++; //カーソルを下に
            }
            eManager.PlaySe(SeNameList.SE_CURSOR); //効果音を再生
        }
        cursor = Mathf.Clamp(cursor, 0, 1); //カーソルを調整

        //決定が押されたら
        if (sceneController.InputActions["Jump"].triggered)
        {
            //カーソルの位置によって処理
            switch(cursor)
            {
                case 0:
                    backFlag = false; //戻るフラグをfalseに
                    break;
                case 1:
                    backFlag = true; //戻るフラグをtrueに
                    break;
                default:
                    break;
            }
            closeFlag = true; //閉じるフラグをtrueに
            eManager.PlaySe(SeNameList.SE_DECISION); //効果音を再生
        }

        //戻るなら
        if (sceneController.InputActions["Fire"].triggered)
        {
            closeFlag = true; //閉じるフラグをtrueに
            backFlag = false; //戻るフラグをfalseに
            eManager.PlaySe(SeNameList.SE_CANCEL); //効果音を再生
        }

        //選択肢の色を変更
        for (int i = 0; i < choice.Length; i++)
        {
            //選択されているなら
            if (i == cursor)
            {
                choice[i].color = new Color(1.0f, 1.0f, 1.0f); //普通の色に
            }
            //されていないなら
            else
            {
                choice[i].color = new Color(UnableColor, UnableColor, UnableColor); //非選択時の色に
            }
        }
    }

    //ポーズ画面の起動
    public void PauseActivate()
    {
        closeFlag = false; //閉じるフラグをfalseに
        backFlag = false; //戻るフラグをfalseに
        closeTime = 0.0f; //時間を初期化
        Time.timeScale = 0.0f; //時間を止める
    }
}
