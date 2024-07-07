using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpeningRoll : MonoBehaviour
{
    //文章情報の構造体
    private struct RollData
    {
        public bool changeBack; //背景を変更するか
        public string roll; //文章の内容
    }

    [SerializeField]
    private TextAsset txtFile = null; //表示する文章ファイル
    [SerializeField]
    private TextMeshProUGUI rollText = null; //表示する文章用のオブジェクト
    [SerializeField]
    private Image backImage = null; //背景の画像
    [SerializeField]
    private Image blackOut = null; //暗転の画像
    [SerializeField]
    private List<Sprite> backImageList; //背景画像のリスト
    [SerializeField]
    private string bgmName = "tukito_nostalgia.ogg"; //再生するBGM

    private List<RollData> rolls; //表示する文章
    private Animator animator;
    private Coroutine moveCoroutine = null; //動作用のコルーチン

    private SceneController sceneController = null; //シーン管理用のオブジェクト

    private int rollNum = 0; //表示する文章の番号
    private int backNum = 0; //表示する背景の番号
    private float rollR = 0.0f; //文章のR値
    private float rollG = 0.0f; //文章のG値
    private float rollB = 0.0f; //文章のB値

    private bool skipFlag = false; //シーンを移すか

    static private readonly float rollTimeMax = 5.0f; //文章を表示する時間

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        animator = GetComponent<Animator>();
        sceneController = FindObjectOfType<SceneController>();
        rolls = new List<RollData>();
        LoadFile(); //ファイルをロード
        rollR = rollText.color.r; //文章のR値を取得
        rollG = rollText.color.g; //文章のG値を取得
        rollB = rollText.color.b; //文章のB値を取得
        backImage.sprite = backImageList[rollNum]; //画像を変更
        sceneController.SoundManager.PlayBgm(bgmName); //BGMを再生
        moveCoroutine = StartCoroutine(RollNextText(true)); //文章の表示を開始
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFlag) return; //次に移るなら飛ばす

        //コルーチンが終わっているなら
        if(moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(RollNextText(false)); //次の文章を表示
        }

        //シーンを終えるなら
        if ((rolls.Count <= rollNum || sceneController.InputActions["Jump"].triggered) && !skipFlag)
        {
            //sManager.StopBgm(); //BGMを停止
            skipFlag = true; //移行フラグをtrueに
            rollText.enabled = false; //文章を非表示
        }

        //シーン移動するなら
        if (skipFlag)
        {
            sceneController.SetNextScene("Title"); //タイトルに移動
        }
    }

    private void FixedUpdate()
    {

    }

    //ファイルの読み込み
    private void LoadFile()
    {
        string[] row = txtFile.text.Split('\n'); //行ごとに分割
        //一行ずつ読み込み
        for (int i = 0; i < row.Length; i++)
        {
            RollData rd = new RollData();
            string[] col = row[i].Split(','); //カンマで区切る
            rd.changeBack = col[0].Contains("1"); //背景を変更するか設定
            rd.roll = col[1]; //文章を設定
            rolls.Add(rd); //リストに追加
        }
    }

    //次の文章に進める
    private IEnumerator RollNextText(bool firstFlag)
    {
        //最初ではないなら
        if (!firstFlag)
        {
            rollNum++; //文章を進める
        }
        //最後なら
        if(rolls.Count <= rollNum)
        {
            rollText.color = new Color(rollR, rollG, rollB, 0.0f); //文章の透明度を設定
            yield break; //終了
        }
        rollText.text = rolls[rollNum].roll; //文章を変更
        animator.Play("RollTextFadeIn"); //表示アニメーションを再生
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
        blackOut.enabled = rolls[rollNum].changeBack; //暗転の表示を設定
        animator.Play("RollTextFadeOut"); //非表示アニメーションを再生
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
        //背景を変えるなら
        if (rolls[rollNum].changeBack)
        {
            ChangeNextBack();
        }
        moveCoroutine = null; //コルーチンを初期化
        yield return null;
    }

    //次の背景に変更する
    private void ChangeNextBack()
    {
        //最後でなければ
        if(backNum + 1 < backImageList.Count)
        {
            backNum++; //背景番号を増加
            backImage.sprite = backImageList[backNum]; //画像を変更
        }
    }
}
