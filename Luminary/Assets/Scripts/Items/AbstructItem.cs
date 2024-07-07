using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructItem : MonoBehaviour
{
    protected bool delFlag = false; //�������邩
    protected float time = 0.0f; //�o�ߎ���

    protected Player player = null; //���@�̃I�u�W�F�N�g
    protected SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    public bool DelFlag { get { return delFlag; } } //�������邩

    // Start is called before the first frame update
    protected void Start()
    {
        player = FindObjectOfType<Player>(); //�I�u�W�F�N�g���擾
        eManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //��������Ȃ�I��
        time += Time.deltaTime; //���Ԃ𑝉�
    }

    //�p�����̍X�V
    protected virtual void SubUpdate() { }

    //�p�����̍X�V
    protected virtual void SubFixUpdate() { }

    //���@�ƐڐG�����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ActiveItem(); //�擾���̏��������s
            delFlag = true; //�A�C�e��������
        }
    }

    //�A�C�e���l�����̏���
    protected virtual void ActiveItem() { }
}
