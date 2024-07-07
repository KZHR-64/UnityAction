using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset txtFile = null; //表示する文章ファイル
    [SerializeField]
    private TextMeshProUGUI rollText = null; //表示する文章用のオブジェクト
    [SerializeField]
    private string bgmName = "tukito287_gorgeous_moment_acoustic.ogg"; //再生するBGM

    List<string> rollList = null; //スタッフロールのリスト

    private SceneController sceneController = null; //シーン管理用のオブジェクト
    private Animator animator;
    private Coroutine moveCoroutine = null; //動作用のコルーチン

    private int rollNum = 0; //表示する文章の番号
    private bool rollEndFlag = false; //文章が最後に達しているか
    private bool backFlag = false; //タイトルに戻るか

    static private readonly float rollTimeMax = 5.0f; //文章を表示する時間

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        animator = GetComponent<Animator>();
        sceneController = FindObjectOfType<SceneController>(); //オブジェクトを取得

        rollList = new List<string>();
        LoadFile(); //ファイルを読み込み

        sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
        moveCoroutine = StartCoroutine(SetNextText(true)); //文章の表示開始
    }

    // Update is called once per frame
    void Update()
    {
        if (backFlag) return; //次に移るなら飛ばす

        //コルーチンが終わっているなら
        if (moveCoroutine == null && !rollEndFlag)
        {
            moveCoroutine = StartCoroutine(SetNextText(false)); //次の文章を表示
        }

        //ポーズが押されたら
        if (sceneController.InputActions["Pause"].triggered && !backFlag)
        {
            backFlag = true; //戻るフラグをtrueに

            sceneController.SetNextScene("Title"); //タイトルに戻る
        }
    }

    //ファイルの読み込み
    private void LoadFile()
    {
        string[] row = txtFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 0; i < row.Length; i++)
        {
            string roll = row[i].Replace(';', '\n'); //セミコロンを改行に変換
            rollList.Add(roll); //リストに追加
        }
    }

    //文章の変更
    private IEnumerator SetNextText(bool firstFlag)
    {
        //最初ではないなら
        if (!firstFlag)
        {
            rollNum++; //文章を進める
        }
        //最後なら
        if (rollList.Count <= rollNum)
        {
            rollEndFlag = true; //終了フラグをtrueに
            yield break; //終了
        }
        rollText.text = rollList[rollNum]; //文章を変更
        animator.Play("EndingRollFadeIn"); //表示アニメーションを再生
        yield return null;
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(rollTimeMax); //しばらく文章を表示
        animator.Play("EndingRollFadeOut"); //非表示アニメーションを再生
        yield return null;
        //アニメーションが終わるまで待機
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        moveCoroutine = null; //コルーチンを初期化
        yield return null;
    }
}