using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private Image[] hpImage; //HP�̉摜
    [SerializeField]
    private Image[] healEnImage; //�񕜃G�l���M�[�̉摜

    private Player player = null; //���@�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>(); //�I�u�W�F�N�g���擾
        HpUpdate(); //HP�֘A�̍X�V
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HpUpdate(); //HP�֘A�̍X�V
    }

    private void FixedUpdate()
    {

    }

    //HP�֘A�̍X�V
    private void HpUpdate()
    {
        //HP�c�ʂ�\��
        for (int i = 0; i < hpImage.Length; i++)
        {
            //���@��HP�����Ȃ�
            if(i < player.Hp)
            {
                hpImage[i].enabled = true; //�摜��\��
            }
            //HP�ȏ�Ȃ�
            else
            {
                hpImage[i].enabled = false; //�摜���\��
            }
        }

        //�񕜃G�l���M�[��\��
        for (int i = 0; i < healEnImage.Length; i++)
        {
            //���@�̉񕜃G�l���M�[�����Ȃ�
            if (i < player.HealEnergy)
            {
                healEnImage[i].enabled = true; //�摜��\��
            }
            //�񕜃G�l���M�[�ȏ�Ȃ�
            else
            {
                healEnImage[i].enabled = false; //�摜���\��
            }
        }

    }
}
