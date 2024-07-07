using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructMapchip : MonoBehaviour
{
    [SerializeField]
    protected int hardness = -1; //�d�x
    [SerializeField]
    protected float friction = 1.0f; //���C�i�~�߂�́j
    [SerializeField]
    protected Vector2 plusVelocity; //�ǉ����鑬�x

    protected Rigidbody2D rBody;
    protected bool delFlag = false; //�������邩
    protected bool brokenFlag = false; //���Ă��邩
    protected float time = 0.0f; //�o�ߎ���

    protected SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g
    protected EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    public bool DelFlag { get { return delFlag; } } //�������邩
    public float Friction { get { return friction; } } //���C�i�~�߂�́j
    public Vector2 PlusVelocity { get { return plusVelocity; } } //�ǉ����鑬�x

    // Start is called before the first frame update
    protected void Start()
    {
        rBody = gameObject.GetComponent<Rigidbody2D>(); //RigidBody���擾
        eManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
        effManager = FindObjectOfType<EffectManager>(); //�I�u�W�F�N�g���擾
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //��������Ȃ�I��
        time += Time.deltaTime; //���Ԃ𑝉�
        //���Ă���Ȃ�
        if (brokenFlag)
        {
            BrokenUpdate(); //��ꂽ���̏���
        }
        //���Ă��Ȃ��Ȃ�
        else
        {
            SubUpdate(); //�p�����̍X�V
        }
    }
    
    protected void FixedUpdate()
    {
        SubFixUpdate(); //�p�����̍X�V
    }

    //�p�����̍X�V
    protected virtual void SubUpdate() { }

    //�p�����̍X�V
    protected virtual void SubFixUpdate() { }

    //��ꂽ���̏���
    protected virtual void BrokenUpdate() { }

    //�����蔻��ƐڐG�����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�e�ɓ��������ꍇ
        if(collision.CompareTag("Bullet"))
        {
            AbstructBullet ab = collision.gameObject.GetComponent<AbstructBullet>(); //�I�u�W�F�N�g���擾

            //�З͂��d�x����Ȃ�
            if(hardness < ab.BreakPower && 0 <= hardness)
            {
                brokenFlag = true; //��ꂽ�t���O��true��
            }
        }
    }

}
