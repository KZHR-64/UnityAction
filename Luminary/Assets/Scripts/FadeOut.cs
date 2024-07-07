using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image panel; //�Ö�

    private bool fadeOutFlag = false; //�Ó]�����邩
    private bool fadeOutEndFlag = false; //�Ó]����������
    private float time = 0.0f; //�Ó]����
    private float alpha = 0.0f; //�Ö��̃A���t�@�l

    public bool FadeOutEndFlag { get { return fadeOutEndFlag; } } //�Ó]����������

    static private readonly float fadeOutColor = 0.0f; //�Ö��̐F
    static private readonly float fadeOutTimeMax = 1.0f; //�Ó]�ɂ����鎞��

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<Image>(); //Image���擾
    }

    // Update is called once per frame
    void Update()
    {
        //�Ó]����Ȃ�
        if(fadeOutFlag)
        {
            time += Time.deltaTime; //���Ԃ𑝉�
            alpha = time / fadeOutTimeMax; //���Ԃɉ����ĔZ������
        }

        //�Ó]����������
        if (1.0f <= alpha)
        {
            fadeOutEndFlag = true; //�Ó]����
        }

        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f); //�����x�̒l�𒲐�
    }

    private void FixedUpdate()
    {
        panel.color = new Color(fadeOutColor, fadeOutColor, fadeOutColor, alpha); //�����x��ݒ�
    }

    //�Ó]�J�n
    public void FadeOutStart()
    {
        fadeOutFlag = true; //�Ó]�J�n
        fadeOutEndFlag = false;
        alpha = 0.0f; //��������J�n
        panel.color = new Color(fadeOutColor, fadeOutColor, fadeOutColor, alpha); //�����x��ݒ�
        panel.enabled = true; //�Ö���\��
        time = 0.0f; //���Ԃ�������
    }

    //�Ó]�I��
    public void FadeOutEnd()
    {
        fadeOutFlag = false; //�Ó]�I��
        panel.enabled = false; //�Ö����\��
    }
}
