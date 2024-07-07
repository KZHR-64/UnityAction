using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image panel; //暗幕

    private bool fadeOutFlag = false; //暗転させるか
    private bool fadeOutEndFlag = false; //暗転しきったか
    private float time = 0.0f; //暗転時間
    private float alpha = 0.0f; //暗幕のアルファ値

    public bool FadeOutEndFlag { get { return fadeOutEndFlag; } } //暗転しきったか

    static private readonly float fadeOutColor = 0.0f; //暗幕の色
    static private readonly float fadeOutTimeMax = 1.0f; //暗転にかかる時間

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<Image>(); //Imageを取得
    }

    // Update is called once per frame
    void Update()
    {
        //暗転するなら
        if(fadeOutFlag)
        {
            time += Time.deltaTime; //時間を増加
            alpha = time / fadeOutTimeMax; //時間に応じて濃くする
        }

        //暗転しきったら
        if (1.0f <= alpha)
        {
            fadeOutEndFlag = true; //暗転完了
        }

        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f); //透明度の値を調整
    }

    private void FixedUpdate()
    {
        panel.color = new Color(fadeOutColor, fadeOutColor, fadeOutColor, alpha); //透明度を設定
    }

    //暗転開始
    public void FadeOutStart()
    {
        fadeOutFlag = true; //暗転開始
        fadeOutEndFlag = false;
        alpha = 0.0f; //透明から開始
        panel.color = new Color(fadeOutColor, fadeOutColor, fadeOutColor, alpha); //透明度を設定
        panel.enabled = true; //暗幕を表示
        time = 0.0f; //時間を初期化
    }

    //暗転終了
    public void FadeOutEnd()
    {
        fadeOutFlag = false; //暗転終了
        panel.enabled = false; //暗幕を非表示
    }
}
