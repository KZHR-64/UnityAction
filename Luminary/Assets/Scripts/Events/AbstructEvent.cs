using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructEvent : MonoBehaviour
{
    [SerializeField]
    protected bool timeStopFlag = false; //���Ԃ��~�߂邩
    [SerializeField]
    protected bool moveWithoutTimeScale = false; //���Ԓ�~������������

    protected bool activeFlag = false; //�C�x���g���������Ă��邩
    protected float time = 0.0f; //�o�ߎ���
    protected bool delFlag = false; //�������邩

    protected Player player; //���@�̃I�u�W�F�N�g
    protected EventManager eveManager = null; //�C�x���g�֘A�̃I�u�W�F�N�g
    protected SoundManager soundManager = null; //BGM�֘A�̃I�u�W�F�N�g
    protected SoundEffectManager soundEffectManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    public bool ActiveFlag { set { activeFlag = value; } } //�C�x���g���������Ă��邩
    public bool TimeStopFlag { get { return timeStopFlag; } } //���Ԃ��~�߂邩
    public bool DelFlag { get { return delFlag; } } //�������邩

    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        if (!activeFlag) return; //�N�����Ă��Ȃ��Ȃ��΂�
        SubUpdate(); //�p�����̍X�V
        //���Ԓ�~���������Ȃ�
        if (moveWithoutTimeScale)
        {
            time += Time.fixedDeltaTime; //���Ԃ𑝉�
        }
        //�����Ȃ��Ȃ�
        else
        {
            time += Time.deltaTime; //���Ԃ𑝉�
        }
    }

    //�p�����̍X�V
    protected virtual void SubUpdate() { }

    //�p�����̍X�V
    protected virtual void SubFixUpdate() { }

    //�C�x���g�̋N��
    public virtual void Activate() { }

    //�ŏ��̐ݒ�
    public void FirstSetting(EventManager em, Player p, SoundManager sm, SoundEffectManager sem)
    {
        eveManager = em;
        player = p;
        soundManager = sm;
        soundEffectManager = sem;
    }
}
