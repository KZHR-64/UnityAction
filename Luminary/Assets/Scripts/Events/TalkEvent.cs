using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEvent : AbstructEvent
{
    //�C�x���g���̍\����
    private struct TalkData
    {
        public int type; //��ށi0=��b�A1=�ҋ@�j
        public string talk; //��b���e
    }

    [SerializeField]
    private TextAsset talkFile = null; //��b�̏��t�@�C��

    [SerializeField]
    private List<TalkData> talkList; //��b���
    private int talkNum = 0; //���݂̉�b
    private bool startFlag = false; //��b���J�n���邩

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        talkList = new List<TalkData>();
        LoadTalkFile();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        if (eveManager.WaitFlag) return; //�ҋ@���Ȃ�I��
        
        //�n�܂��Ă��Ȃ����
        if (!startFlag)
        {
            //���΂炭������
            if (0.8f < time)
            {
                eveManager.TalkText.enabled = true; //���͂�\��
                SetText(); //���̕��͂�ݒ�
                startFlag = true; //��b���J�n
            }
            return;
        }

        //�Ō�ɒB���Ă��Ȃ����
        if (talkNum < talkList.Count)
        {
            //���肪�����ꂽ��
            if (eveManager.CheckButtonInput())
            {
                talkNum++; //���̉�b��ݒ�
                SetText(); //���̕��͂�ݒ�
            }
            //�Ō�ɒB�����Ȃ�
            if (talkNum == talkList.Count)
            {
                eveManager.TalkText.enabled = false; //���͂��\����
                eveManager.MsgWindow.CloseWindow(); //��b�E�B���h�E���\����
                time = 0.0f; //���Ԃ�������
            }
        }
        //�Ō�ɒB�����Ȃ�
        else
        {
            //���΂炭������
            if (0.8f < time)
            {
                //�Ō�ɒB���Ă���Ȃ�
                if (talkList.Count <= talkNum)
                {
                    delFlag = true; //�I�u�W�F�N�g������
                }
                //�Ō�ł͂Ȃ��Ȃ�
                else
                {
                    eveManager.WaitFlag = true; //�ҋ@�t���O��true��
                }
            }
        }
    }

    //�C�x���g�̋N��
    public override void Activate()
    {
        eveManager.MsgWindow.OpenWindow(); //��b�E�B���h�E��\��
        activeFlag = true; //�C�x���g���N��
        time = 0.0f; //���Ԃ�������
    }

    //��b���̓ǂݍ���
    private void LoadTalkFile()
    {
        string[] row = talkFile.text.Split('\n'); //�s���Ƃɕ���
        //��s���ǂݍ���
        for (int i = 0; i < row.Length; i++)
        {
            string[] col = row[i].Split(','); //�J���}�ŋ�؂�
            string talkStr = ""; //��b���e
            TalkData td = new TalkData();
            td.type = int.Parse(col[0]); //��ނ�ݒ�
            if(0 < col[1].Length)
            {
                talkStr = string.Concat(talkStr, col[1].TrimEnd(), '\n'); //���e��ݒ�
            }
            td.talk = string.Concat(talkStr, col[2].TrimEnd()); //���e��ݒ�
            talkList.Add(td); //�������X�g�ɒǉ�
        }
    }

    //���̕��͂�ݒ�
    private void SetText()
    {
        if(talkNum == talkList.Count) return; //�Ō�ɒB���Ă�����I��
        eveManager.TalkText.text = talkList[talkNum].talk; //���͂�ݒ�

    }
}
