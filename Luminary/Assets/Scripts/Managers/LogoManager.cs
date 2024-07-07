using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour
{
    [SerializeField]
    private Image logoImage = null; //背景の画像

    private float time = 0.0f; //ロゴを表示してからの時間
    private float logoR = 0.0f; //背景のR値
    private float logoG = 0.0f; //背景のG値
    private float logoB = 0.0f; //背景のB値
    private float logoAlpha = 0.0f; //背景のアルファ値

    private bool skipFlag = false; //シーンを移すか

    static private readonly float sceneTimeMax = 5.0f; //次のシーンに移る時間

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Calculation.FRAME_RATE; //FPSを60に
        logoR = logoImage.color.r; //文章のR値を取得
        logoG = logoImage.color.g; //文章のG値を取得
        logoB = logoImage.color.b; //文章のB値を取得
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFlag) return; //次に移るなら飛ばす

        time += Time.deltaTime; //時間を増加

        //1秒で出現
        if (time <= 1.0f)
        {
            logoAlpha = 1.0f * time;
        }
        //それ以外なら文字ははっきり
        else if (1.0f < time)
        {
            logoAlpha = 1.0f;
        }

        logoAlpha = Mathf.Clamp(logoAlpha, 0.0f, 1.0f); //透明度の値を調整

        //シーン移動の時間になったら
        if (sceneTimeMax <= time && !skipFlag)
        {
            skipFlag = true; //移行フラグをtrueに
        }

        //シーン移動するなら
        if (skipFlag)
        {
            var sc = FindObjectOfType<SceneController>();
            sc.SetNextScene("Opening"); //オープニングに移動
        }
    }

    private void FixedUpdate()
    {
        logoImage.color = new Color(logoR, logoG, logoB, logoAlpha); //画像の透明度を設定
    }
}
