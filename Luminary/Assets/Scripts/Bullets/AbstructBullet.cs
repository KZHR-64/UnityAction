using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructBullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 0; //�_���[�W��
    [SerializeField]
    protected float baseSpeed = 0.0f; //��ƂȂ鑬�x
    [SerializeField]
    private bool actOutCamera = false; //��ʊO�ł�������
    [SerializeField]
    private bool hitMap = true; //�n�`�ɓ����邩
    [SerializeField]
    private bool hitPlayer = true; //���@�ɓ����邩
    [SerializeField]
    private bool hitEnemy = true; //�G�ɓ����邩
    [SerializeField]
    protected bool penetration = false; //�ђʂ��邩
    [SerializeField]
    protected int breakPower = 0; //�n�`���󂷗�

    protected Rigidbody2D rBody = null;
    protected Transform causer = null; //�e���������I�u�W�F�N�g��Transform
    protected Coroutine moveCoroutine = null; //����p�̃R���[�`��
    protected bool delFlag = false; //�������邩
    protected bool hitFlag = false; //�n�`�ɓ���������
    protected bool outCameraFlag = false; //��ʊO�ɏo����
    protected float time = 0.0f; //�o�ߎ���
    protected float xSpeed = 0.0f; //x�����̑��x
    protected float ySpeed = 0.0f; //y�����̑��x

    protected Player player = null; //���@�̃I�u�W�F�N�g
    protected BulletManager bulManager = null; //�e�Ǘ��̃I�u�W�F�N�g
    protected EffectManager effManager = null; //�G�t�F�N�g�Ǘ��̃I�u�W�F�N�g
    protected SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    public int Damage { get { return damage; } } //�_���[�W��
    public bool DelFlag { get { return delFlag; } } //�������邩
    public bool HitPlayer { get { return hitPlayer; } } //���@�ɓ����邩
    public bool HitEnemy { get { return hitEnemy; } } //�G�ɓ����邩
    public int BreakPower { get { return breakPower; } } //�n�`���󂷗�

    // Start is called before the first frame update
    protected void Start()
    {
        rBody = GetComponent<Rigidbody2D>(); //RigidBody���擾
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //��������Ȃ�I��
        SubUpdate(); //�p�����̍X�V
        time += Time.deltaTime; //���Ԃ𑝉�
    }

    protected void FixedUpdate()
    {
        SubFixUpdate(); //�p�����̍X�V
        if (rBody)
        {
            rBody.velocity = new Vector2(xSpeed, ySpeed); //���x�̐ݒ�
        }
        //�������Ă��Ċђʂ��Ȃ��ꍇ
        if(hitFlag && !penetration)
        {
            EraseUpdate(); //�e����������
        }
        hitFlag = false; //�n�`�ɓ������Ă���t���O��false��
    }

    //�p�����̍X�V
    protected virtual void SubUpdate() { }

    //�p�����̍X�V
    protected virtual void SubFixUpdate() { }

    //������Ƃ��̍X�V
    protected virtual void EraseUpdate()
    {
        delFlag = true; //�e������
    }

    //�ŏ��̐ݒ�
    public void FirstSetting(BulletManager bm, Player p, EffectManager eff, SoundEffectManager sem)
    {
        bulManager = bm;
        player = p;
        effManager = eff;
        eManager = sem;
    }

    //���x��ݒ�
    protected void SetSpeed(bool checkScale = true)
    {
        float rad = transform.localEulerAngles.z * Mathf.Deg2Rad; //���W�A�����擾
        xSpeed = baseSpeed * Mathf.Cos(rad); //���x��ݒ�
        ySpeed = baseSpeed * Mathf.Sin(rad); //���x��ݒ�
        //�����𔽉f����ꍇ
        if(checkScale)
        {
            xSpeed *= transform.localScale.x; //���x��ݒ�
            ySpeed *= transform.localScale.x; //���x��ݒ�
        }
    }
    
    //�����蔻��ƐڐG�����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�n�`�ɓ��������ꍇ
        if(collision.gameObject.CompareTag("Ground") && hitMap)
        {
            hitFlag = true; //���������t���O��true��
        }
        //���@�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Player") && hitPlayer)
        {
            hitFlag = true; //���������t���O��true��
        }
        //�G�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Enemy") && hitEnemy)
        {
            AbstructEnemy ae = collision.gameObject.GetComponent<AbstructEnemy>(); //�I�u�W�F�N�g���擾
            hitFlag = true; //���������t���O��true��
        }
    }

    //�J�����͈̔͂���o���ꍇ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("GeneratorActiveArea")) return;

        //��ʊO�œ��삵�Ȃ��Ȃ�
        if (!actOutCamera)
        {
            delFlag = true; //�e������
        }
        //���삷��Ȃ�
        else
        {
            outCameraFlag = true; //�o���t���O��true��
        }
    }

    //�ʒu��ݒ�
    public void SetPosition(float setX, float setY)
    {
        transform.position = new Vector3(setX, setY, transform.position.z);
    }

    //��ƂȂ鑬�x��ݒ�
    public void SetBaseSpeed(float setBaseSpeed)
    {
        baseSpeed = setBaseSpeed; //��̑��x��ݒ�
        SetSpeed(); //���x���Đݒ�
    }
}
