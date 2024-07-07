using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AbstructEnemy : MonoBehaviour
{
    [SerializeField]
    protected int maxHp = 0; //HP�̍ő�l
    [SerializeField]
    protected bool bossFlag = false; //�{�X��
    [SerializeField]
    protected bool actOutCamera = false; //��ʊO�ł�������
    [SerializeField]
    protected bool itemDropFlag = true; //�A�C�e���𗎂Ƃ���
    [SerializeField]
    protected GroundChecker groundChecker = null; //���n����
    [SerializeField]
    protected Collider2D[] damageCollider = null; //�_���[�W����
    [SerializeField]
    protected Transform[] boneList = null; //�Q�Ƃ���{�[��
    [SerializeField]
    protected GameObject[] bulletList = null; //���\��̒e
    [SerializeField]
    protected GameObject[] enemyList = null; //�o���\��̓G

    protected Rigidbody2D rBody = null;
    protected Animator animator = null;
    protected Renderer rend = null;
    protected Renderer[] rendererList = null;
    protected Coroutine moveCoroutine = null; //����p�̃R���[�`��
    protected int hp; //HP
    protected bool firstMoveFlag = true; //�n�߂Ă̓��삩
    protected bool delFlag = false; //�������邩
    protected bool defeatFlag = false; //���ꂽ��
    protected bool hitFlag = false; //�����ɓ���������
    protected bool groundFlag = true; //���n���Ă��邩
    protected bool landingFlag = false; //���n���ォ
    protected float time = 0.0f; //�o�ߎ���
    protected float xSpeed = 0.0f; //x�����̑��x
    protected float ySpeed = 0.0f; //y�����̑��x
    protected bool xStop = false; //�������̈ړ�����߂邩
    protected bool yMove = false; //�c�����Ɉړ����邩
    protected int status = 0; //�s���p�^�[��
    protected bool timerStop = false; //���Ԃ��~�߂邩
    protected float damageTime = 0.0f; //�_���[�W��̖��G����

    protected Player player = null; //���@�̃I�u�W�F�N�g
    protected EnemyManager enemyManager = null; //�G�֘A�̃I�u�W�F�N�g
    protected BulletManager bulletManager = null; //�e�̊Ǘ��I�u�W�F�N�g
    protected SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g
    protected EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g
    protected CameraManager cameraManager = null; //�J�����֘A�̃I�u�W�F�N�g

    static protected readonly float damageAlphaBase = 0.5f; //�_���[�W���̃A���t�@�l�̊�

    public int MaxHp { get { return maxHp; } } //�ő�HP
    public int Hp { get { return hp; } } //HP
    public bool DelFlag { get { return delFlag; } } //�������邩
    public bool DefeatFlag { get { return defeatFlag; } } //���ꂽ��
    public bool BossFlag { get { return bossFlag; } } //�{�X��
    public bool ItemDropFlag { get { return itemDropFlag; } } //�A�C�e���𗎂Ƃ���

    // Start is called before the first frame update
    protected void Start()
    {
        rBody = GetComponent<Rigidbody2D>(); //RigidBody���擾
        animator = GetComponent<Animator>(); //Animator���擾
        rend = GetComponent<Renderer>(); //Renderer���擾
        rendererList = GetComponentsInChildren<Renderer>();
        hp = maxHp; //HP��ݒ�
    }

    // Update is called once per frame
    protected void Update()
    {
        //�J�n����̃t���[���ł͓��삵�Ȃ�
        if (firstMoveFlag)
        {
            firstMoveFlag = false; //�n�߂̓���̃t���O��false��
            return; //�I��
        }

        if (delFlag) return; //��������Ȃ�I��
        //�_���[�W���̖��G���Ȃ�
        if(0.0f < damageTime)
        {
            damageTime -= Time.deltaTime; //���G���Ԃ�����
            damageTime = Mathf.Max(damageTime, 0.0f); //���Ԃ𒲐�
        }
        //����Ă���Ȃ�
        if (defeatFlag)
        {
            DefeatUpdate(); //���ꂽ���̓���
        }
        //����Ă��Ȃ��Ȃ�
        else
        {
            SubUpdate(); //�p�����̍X�V
        }
        //���Ԃ��~�߂Ȃ��Ȃ�
        if (!timerStop)
        {
            time += Time.deltaTime; //���Ԃ𑝉�
        }
        hitFlag = false; //�������������������
    }

    protected void FixedUpdate()
    {
        float xs = xSpeed; //x�����̑��x
        float ys = rBody.velocity.y; //y�����̑��x

        bool justGroundFlag = groundFlag; //���O�̐ڒn����

        //�ڐG���肪����Ȃ�
        if(groundChecker != null)
        {
            groundFlag = groundChecker.GetGround(); //�n�ʂɐڐG���Ă��邩����
        }

        //���n����Ȃ�
        if(!justGroundFlag && groundFlag)
        {
            landingFlag = true; //���n�t���O��true��
        }
        //�����łȂ��Ȃ�
        else
        {
            landingFlag = false; //���n�t���O��false��
        }

        //�~�܂�Ȃ�
        if (xStop)
        {
            xs = 0.0f; //���x��0��
            xStop = false; //�~�܂�t���O��false��
        }
        //�c�����Ɉړ�����Ȃ�
        if(yMove)
        {
            ys = ySpeed; //y�����̑��x��ݒ�
        }

        SubFixUpdate(); //�p�����̍X�V

        rBody.velocity = new Vector2(xs, ys); //���x�̐ݒ�

        //Renderer������Ȃ�
        if(rend)
        {
            Color col = rend.material.color;
            //�_���[�W���̖��G���Ȃ�
            if (0.0f < damageTime)
            {
                rend.material.color = new Color(col.r, col.g, col.b, damageAlphaBase + Mathf.PingPong(time, 0.4f)); //�L������_�ł�����
            }
            //�����łȂ��Ȃ�
            else
            {
                rend.material.color = new Color(col.r, col.g, col.b, 1.0f); //�L���������̂܂ܕ\��
            }
        }

        //�q�I�u�W�F�N�g���ύX
        foreach(Renderer r in rendererList)
        {
            Color col = r.material.color;
            //�_���[�W���̖��G���Ȃ�
            if (0.0f < damageTime)
            {
                r.material.color = new Color(col.r, col.g, col.b, damageAlphaBase + Mathf.PingPong(time, 0.4f)); //�L������_�ł�����
            }
            //�����łȂ��Ȃ�
            else
            {
                r.material.color = new Color(col.r, col.g, col.b, 1.0f); //�L���������̂܂ܕ\��
            }
        }
    }

    //�p�����̍X�V
    protected virtual void SubUpdate() { }

    //�p�����̍X�V
    protected virtual void SubFixUpdate() { }

    //���ꂽ���̓���
    protected virtual void DefeatUpdate()
    {
        delFlag = true; //�G������
    }

    //�ŏ��̐ݒ�
    public void FirstSetting(EnemyManager em, Player p, BulletManager bm, SoundEffectManager sem, EffectManager eff, CameraManager cam)
    {
        player = p;
        enemyManager = em;
        bulletManager = bm;
        eManager = sem;
        effManager = eff;
        cameraManager = cam;
    }

    //�����蔻��ƐڐG�����ꍇ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�}�b�v�`�b�v�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Mapchip") || collision.gameObject.CompareTag("Ground"))
        {
            hitFlag = true; //�������������true��
        }
    }

    //�����蔻��ƐڐG�����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���@�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            hitFlag = true; //�������������true��
        }
        //�e�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (defeatFlag) return; //����Ă���Ȃ�I��
            AbstructBullet ab = collision.gameObject.GetComponent<AbstructBullet>(); //�I�u�W�F�N�g���擾
            if (!ab.HitEnemy) return; //�G�ɓ�����Ȃ��Ȃ��΂�
            //���G���Ԃ��������Ă��Ȃ��Ȃ�
            if (damageTime <= 0.0f)
            {
                hp -= ab.Damage; //�_���[�W��HP�����炷
                damageTime = 1.0f; //���G���Ԃ�ݒ�
            }

            //HP��0�ɂȂ�����
            if (hp <= 0)
            {
                defeatFlag = true; //���ꂽ�t���O��true��
                time = 0.0f; //���Ԃ�������
                status = 0; //��Ԃ�������
                foreach(Collider2D col in damageCollider)
                {
                    col.enabled = false; //�����蔻�������
                }
                //�R���[�`�������s���Ȃ�
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine); //�R���[�`�����~
                }
                //�{�X�Ȃ�
                if (bossFlag)
                {
                    enemyManager.BeatBoss(); //�{�X���j���̃C�x���g���J�n
                }
            }
            //0�łȂ����
            else
            {
                //�G�t�F�N�g�𔭐�
                effManager.CreateEffect(EffectNameList.EFFECT_HIT, transform.position, transform.rotation, transform);
            }

            eManager.PlaySe(SeNameList.SE_HIT_ENEMY); //���ʉ����Đ�
        }
    }

    //�ʒu��ݒ�
    public void SetPosition(float setX, float setY)
    {
        transform.position = new Vector3(setX, setY, transform.position.z);
    }

    //���x��ݒ�
    public void SetXSpeed(float setSpeed)
    {
        xSpeed = setSpeed * transform.localScale.x; //���x��ݒ�
    }

    //���x��ݒ�
    public void SetYSpeed(float setSpeed)
    {
        ySpeed = setSpeed; //���x��ݒ�
    }
}
