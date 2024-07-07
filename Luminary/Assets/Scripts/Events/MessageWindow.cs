using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class MessageWindow : MonoBehaviour
{
    private Animator animator;
    private Image img;

    static private readonly int openAnimHash = Animator.StringToHash("OpenWindow"); //開くアニメーションのハッシュ
    static private readonly int neutralAnimHash = Animator.StringToHash("NeutralWindow"); //開くアニメーションのハッシュ
    static private readonly int closeAnimHash = Animator.StringToHash("CloseWindow"); //閉じるアニメーションのハッシュ

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //Animatorを取得
        img = GetComponent<Image>(); //Imageを取得
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("CloseWindow"))
        {
            if (1.0f <= currentState.normalizedTime)
            {
                animator.Play(neutralAnimHash);
                img.enabled = false;
            }
        }
    }

    //会話ウィンドウを開く
    public void OpenWindow()
    {
        img.enabled = true;
        animator.Play(openAnimHash); //アニメーションを再生
    }

    //会話ウィンドウを閉じる
    public void CloseWindow()
    {
        animator.Play(closeAnimHash); //アニメーションを再生
    }
}
