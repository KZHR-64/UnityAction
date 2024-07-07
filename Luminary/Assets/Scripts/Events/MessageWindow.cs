using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class MessageWindow : MonoBehaviour
{
    private Animator animator;
    private Image img;

    static private readonly int openAnimHash = Animator.StringToHash("OpenWindow"); //�J���A�j���[�V�����̃n�b�V��
    static private readonly int neutralAnimHash = Animator.StringToHash("NeutralWindow"); //�J���A�j���[�V�����̃n�b�V��
    static private readonly int closeAnimHash = Animator.StringToHash("CloseWindow"); //����A�j���[�V�����̃n�b�V��

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //Animator���擾
        img = GetComponent<Image>(); //Image���擾
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

    //��b�E�B���h�E���J��
    public void OpenWindow()
    {
        img.enabled = true;
        animator.Play(openAnimHash); //�A�j���[�V�������Đ�
    }

    //��b�E�B���h�E�����
    public void CloseWindow()
    {
        animator.Play(closeAnimHash); //�A�j���[�V�������Đ�
    }
}
