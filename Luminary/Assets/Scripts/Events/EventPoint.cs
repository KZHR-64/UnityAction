using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPoint : MonoBehaviour
{
    [SerializeField]
    private List<AbstructEvent> eventList; //���s����C�x���g

    private Queue<AbstructEvent> events; //���s����C�x���g
    private bool hitFlag = false; //���@���G�ꂽ��

    private EventManager eveManager = null; //�C�x���g�֘A�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        //�C�x���g���Ȃ��Ȃ�
        if(eventList.Count <= 0)
        {
            Destroy(gameObject); //�I�u�W�F�N�g������
            return;
        }
        //�C�x���g���L���[�ɑ}��
        events = new Queue<AbstructEvent>();
        foreach (AbstructEvent item in eventList)
        {
            events.Enqueue(item);
        }
        eventList.Clear(); //List�͔j��
    }

    // Update is called once per frame
    void Update()
    {
        //�C�x���g���I����Ă���Ȃ�
        if (events.Peek().DelFlag)
        {
            //���Ԃ��~�߂�C�x���g�Ȃ�
            if (events.Peek().TimeStopFlag)
            {
                eveManager.RestartGame(); //���Ԓ�~������
            }
            //�I����Ă���C�x���g��j��
            Destroy(events.Peek().gameObject);
            events.Dequeue();

            //�C�x���g�����ׂďI����Ă���Ȃ�
            if (events.Count == 0)
            {
                Destroy(gameObject); //�I�u�W�F�N�g������
            }
            //�c���Ă���Ȃ�
            else
            {
                //���Ԃ��~�߂�C�x���g�Ȃ�
                if(events.Peek().TimeStopFlag)
                {
                    eveManager.StopGame(); //���Ԃ��~
                }
                events.Peek().Activate(); //���̃C�x���g���J�n
            }
        }
    }

    //�ŏ��̐ݒ�
    public void FirstSetting(EventManager em)
    {
        eveManager = em;
    }

    //���@���d�Ȃ����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitFlag || events.Count == 0) return; //���łɐG��Ă���Ȃ�I��
        if (collision.CompareTag("Player"))
        {
            hitFlag = true; //�G�ꂽ�t���O��true��
            //���Ԃ��~�߂�C�x���g�Ȃ�
            if (events.Peek().TimeStopFlag)
            {
                eveManager.StopGame(); //���Ԃ��~
            }
            events.Peek().Activate(); //�C�x���g���J�n
        }
    }
}
