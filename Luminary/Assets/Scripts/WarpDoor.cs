using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDoor : MonoBehaviour
{
    //[SerializeField]
    //private string nextMapName = null; //�ړ���̃}�b�v��
    //[SerializeField]
    //private bool checkBoss = false; //�{�X�풆���Q�Ƃ��邩

    //private bool activeFlag = true; //���삷�邩
    //private bool hitFlag = false; //���@���d�Ȃ��Ă��邩

    //private Manager manager = null; //�Q�[���Ǘ��̃I�u�W�F�N�g
    //private SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        //manager = FindObjectOfType<Manager>(); //�I�u�W�F�N�g���擾
        //eManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
    }

    // Update is called once per frame
    void Update()
    {
        //���삵�Ȃ��Ȃ�
        /*if(!activeFlag)
        {
            //�{�X�킪�I����Ă���Ȃ�
            if(checkBoss && !manager.BossBattleFlag)
            {
                activeFlag = true; //����t���O��true��
                gameObject.GetComponent<Renderer>().enabled = true; //����\��
            }
            return; //�߂�
        }

        //�{�X�풆�Ȃ�
        if(checkBoss && manager.BossBattleFlag && activeFlag)
        {
            activeFlag = false; //����t���O��false��
            gameObject.GetComponent<Renderer>().enabled = false; //�����\��
            return; //�߂�
        }

        //���@���d�Ȃ��Ă���Ȃ�
        if (hitFlag && nextMapName != null)
        {
            //�オ�����ꂽ��
            if (Input.GetButtonDown("Vertical") && 0.0f < Input.GetAxisRaw("Vertical"))
            {
                eManager.PlaySe("warp.ogg"); //���ʉ����Đ�
                manager.SetNextMap(nextMapName); //���̃}�b�v�̈ړ�����
            }
        }*/
    }

    //���@���d�Ȃ����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //hitFlag = true; //�d�Ȃ����t���O��true��
        }
    }

    //���@�����ꂽ�ꍇ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //hitFlag = false; //�d�Ȃ����t���O��false��
        }
    }
}
