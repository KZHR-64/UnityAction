using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject[] pBullet = null; //�e
    [SerializeField]
    private GroundChecker gCheck = null; //�ڒn����
    [SerializeField]
    private List<Renderer> rendererList = null; //�ҏW����摜
    [SerializeField]
    private BulletManager bulletManager = null; //�e�̊Ǘ��I�u�W�F�N�g
    [SerializeField]
    private bool canPunch = true; //�p���`��łĂ邩
    [SerializeField]
    private bool canShot = true; //�e�����Ă邩

    private Rigidbody2D rBody;
    private Collider2D pCollider;
    private Animator animator;
    private Coroutine moveCoroutine = null; //����p�̃R���[�`��
    private bool firstMoveFlag = true; //�n�߂Ă̓��삩
    private bool moveFlag = false; //�������邩
    private int hp; //HP
    private int healEnergy; //�񕜃G�l���M�[
    private float xIn = 0.0f; //���E�̓���
    private float yIn = 0.0f; //�㉺�̓���
    private float xSpeed = 0.0f; //x�����̑��x
    private float ySpeed = 0.0f; //y�����̑��x
    private float accel = 0.0f; //�����x
    private bool jumpFlag = false; //�W�����v���邩
    private float jumpTime = 0.0f; //�W�����v����
    public bool jumpKeyFlag = false; //�W�����v�L�[�������ꂽ��
    public float jumpKeyTime = 0.0f; //�W�����v�̓��͎���
    private bool groundFlag = true; //���n���Ă��邩
    private bool attackFlag = false; //�U�����邩
    private float damageTime = 0.0f; //�_���[�W��̖��G����

    //���͏��
    private Vector2 stickInput;
    private float jumpInput = 0.0f;

    private Manager manager = null; //�Ǘ��p�̃I�u�W�F�N�g
    private SceneController sceneController = null; //�V�[���Ǘ��p�̃I�u�W�F�N�g

    private SaveData saveData = null; //�Z�[�u�֘A�̃I�u�W�F�N�g
    private SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g
    private EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    static private float speedMax = 8.0f; //���x�̊
    static private float accelBase = 0.2f; //�����x�̊
    static private float jumpSpeed = 12.0f; //�W�����v���x�̊
    static private readonly float jumpTimeMax = 0.2f; //�W�����v���Ԃ̍ő�l
    static private readonly float gScale = 2.0f; //�d�͂̊
    static private readonly float damageTimeBase = 2.0f; //�_���[�W���̖��G����
    static private readonly float damageAlphaBase = 0.5f; //�_���[�W���̃A���t�@�l�̊�
    static private readonly int standAnimHash = Animator.StringToHash("PlayerStand"); //�����Ă���A�j���[�V�����̃n�b�V��
    static private readonly int runAnimHash = Animator.StringToHash("PlayerRun"); //����A�j���[�V�����̃n�b�V��
    static private readonly int jumpAnimHash = Animator.StringToHash("PlayerJump"); //�W�����v����A�j���[�V�����̃n�b�V��
    static private readonly int punchAnimHash = Animator.StringToHash("PlayerPunch"); //�p���`����A�j���[�V�����̃n�b�V��
    static private readonly int powPunchAnimHash = Animator.StringToHash("PlayerPowerPunch"); //�p���`����A�j���[�V�����̃n�b�V��
    static private readonly int shotAnimHash = Animator.StringToHash("PlayerShot"); //�U������A�j���[�V�����̃n�b�V��
    static private readonly int damageAnimHash = Animator.StringToHash("PlayerDamage"); //�_���[�W�A�j���[�V�����̃n�b�V��

    public int Hp { get { return hp; } set { hp = value; } } //HP
    public int HealEnergy { get { return healEnergy; } set { healEnergy = value; } } //�񕜃G�l���M�[
    public Collider2D PCollider { get { return pCollider; } } //collider
    public bool MoveFlag { set { moveFlag = value; } } //�������邩
    public Manager Manager { set { manager = value; } } //�Ǘ��p�̃I�u�W�F�N�g
    public SceneController SceneController { set { sceneController = value; } } //�V�[���Ǘ��p�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        rBody = gameObject.GetComponent<Rigidbody2D>(); //RigidBody���擾
        pCollider = gameObject.GetComponent<BoxCollider2D>(); //Collider���擾
        animator = GetComponent<Animator>(); //Animator���擾
        saveData = sceneController.SaveData; //�I�u�W�F�N�g���擾
        eManager = sceneController.SoundEffectManager; //�I�u�W�F�N�g���擾
        effManager = sceneController.EffectManager; //�I�u�W�F�N�g���擾
        hp = saveData.Hp; //HP��ݒ�
        healEnergy = saveData.HealEnergy; //�񕜃G�l���M�[��ݒ�

    }

    // Update is called once per frame
    void Update()
    {
        //�J�n����̃t���[���ł͓��삵�Ȃ�
        if(firstMoveFlag)
        {
            firstMoveFlag = false; //�n�߂̓���̃t���O��false��
            moveFlag = true; //����t���O��true��
            return; //�I��
        }

        //HP��0�Ȃ�
        if(hp <= 0)
        {
            xIn = 0.0f; //���E�̓��͂�0��
            yIn = 0.0f; //�㉺�̓��͂�0��
            return; //�I��
        }

        CheckInput(); //���͂̊m�F

        Attack(); //�U��
    }

    private void FixedUpdate()
    {
        bool lastJumpFlag = jumpFlag; //���O�̃W�����v�t���O

        //HP��0�Ȃ�
        if (hp <= 0)
        {
            rBody.velocity = new Vector2(0.0f, 0.0f); //���x�̐ݒ�
            return; //�I��
        }

        //xSpeed = rBody.velocity.x; //x�����̑��x
        ySpeed = rBody.velocity.y; //y�����̑��x

        if (!groundFlag && 0.01f < ySpeed)
        {
            groundFlag = false;
        }
        else
        {
            groundFlag = gCheck.GetGround(); //�n�ʂɐڐG���Ă��邩����
        }

        //���E�̓��͂�����A�U�����łȂ��ꍇ
        if (0.0f < Mathf.Abs(xIn) && !attackFlag)
        {
            //�E�̓��͂��傫���ꍇ
            if (0.0f < xIn)
            {
                transform.localScale = new Vector2(1.0f, 1.0f); //������ݒ�
            }
            //���̓��͂��傫���ꍇ
            else if (xIn < 0.0f)
            {
                transform.localScale = new Vector2(-1.0f, 1.0f); //�����𔽓]
            }

            accel = accelBase * transform.localScale.x; //�����x��ݒ�
            //�����n�߂Ȃ�
            if (Mathf.Abs(xSpeed) <= 0.5f)
            {
                xSpeed = 0.5f * transform.localScale.x; //������ݒ�
            }
            xSpeed += Mathf.Abs(xSpeed) * accel; //������
        }
        //�Ȃ��ꍇ
        else
        {
            accel = groundFlag ? 0.7f + gCheck.GetMapchipFriction() : 0.9f; //����
            xSpeed *= accel;
        }

        //���Ȃ�x���Ȃ�����
        if (Mathf.Abs(xSpeed) < 0.5f)
        {
            xSpeed = 0.0f; //���x��0��
        }

        //�ڒn���ŃW�����v�{�^����������Ă��Ȃ��ꍇ
        if (groundFlag)
        {
            jumpFlag = false; //�W�����v�t���O��false��
            lastJumpFlag = jumpFlag;
            jumpTime = 0.0f; //�W�����v���Ԃ�������
        }
        //�ڒn���ōU�����łȂ��W�����v����Ȃ�i���O���͂�0.2�b�܂ŗL���j
        if (groundFlag && !jumpFlag && jumpKeyFlag && !attackFlag && jumpKeyTime <= 0.2f)
        {
            ySpeed = jumpSpeed; //�W�����v���x��ݒ�
            jumpFlag = true; //�W�����v�t���O��true��
            jumpTime = 0.0f; //�W�����v���Ԃ�������
            rBody.gravityScale = 0.0f; //�d�͂𖳌���
            eManager.PlaySe("jump.ogg"); //���ʉ����Đ�    
        }

        //�ڒn���Ă��Ȃ��ꍇ
        if (!groundFlag)
        {
            //�W�����v�{�^����������Ă���ꍇ
            if (jumpKeyFlag)
            {
                jumpTime += Time.deltaTime; //�W�����v���Ԃ𑝉�
            }
            //������Ă��Ȃ��ꍇ
            else
            {
                jumpFlag = true; //�W�����v�t���O��true��
                jumpTime = jumpTimeMax; //�W�����v���Ԃ��ő�l��    
            }
        }

        //�W�����v���Ԃ����E�ɒB�����ꍇ
        if (jumpTimeMax <= jumpTime)
        {
            rBody.gravityScale = gScale; //�d�͂�ݒ肵�Ȃ���
        }

        xSpeed = Mathf.Clamp(xSpeed, -speedMax, speedMax); //�ړ����x�̌��E��ݒ�
        ySpeed = Mathf.Clamp(ySpeed, -jumpSpeed, jumpSpeed); //�����A�㏸���x�̌��E��ݒ�

        Vector2 pVelocity = new Vector2(xSpeed, ySpeed); //���x��ݒ�

        //�ڒn���Ă���Ȃ�
        if (groundFlag)
        {
            Vector2 vec = gCheck.GetMapchipVelocity(); //�}�b�v�`�b�v�̑��x���擾
            pVelocity += vec; //���x��ǉ�
        }

        rBody.velocity = pVelocity; //���x�̐ݒ�

        if (moveCoroutine == null)
        {
            //���E�̓��͂�����Ȃ�
            if(0.5f <= Mathf.Abs(xIn) && !jumpFlag)
            {
                animator.Play(runAnimHash); //�A�j���[�V�������Đ�
            }
            else if (!jumpFlag)
            {
                animator.Play(standAnimHash); //�A�j���[�V�������Đ�
            }
            //�W�����v�����u�ԂȂ�
            if (!lastJumpFlag && jumpFlag)
            {
                animator.Play(jumpAnimHash); //�A�j���[�V�������Đ�
            }
            //���n�����Ȃ�
            if (lastJumpFlag && groundFlag)
            {
                animator.Play(standAnimHash); //�A�j���[�V�������Đ�
            }
        }

        Color col = rendererList[0].material.color;
        
        //�_���[�W���̖��G���Ȃ�
        if (0.0f < damageTime)
        {
            foreach (Renderer ren in rendererList) {
                ren.material.color = new Color(col.r, col.g, col.b, damageAlphaBase + Mathf.PingPong(Time.time, 0.4f)); //�L������_�ł�����
            }
        }
        //�����łȂ��Ȃ�
        else
        {
            foreach (Renderer ren in rendererList)
            {
                ren.material.color = new Color(col.r, col.g, col.b, 1.0f); //�L���������̂܂ܕ\��
            }
        }
    }

    //���͂̊m�F
    private void CheckInput()
    {
        //�������Ȃ��Ȃ�I��
        if (!moveFlag)
        {
            xIn = 0.0f; //���E�̓��͊m�F
            yIn = 0.0f; //�㉺�̓��͊m�F
            jumpInput = 0.0f;
            jumpKeyFlag = false; //�W�����v�L�[�t���O��false��
            jumpKeyTime = 0.0f; //���͎��Ԃ�������
            return;
        }

        //���͂̎擾
        stickInput = sceneController.InputActions["Move"].ReadValue<Vector2>();
        jumpInput = sceneController.PlayerInput.actions["Jump"].ReadValue<float>();

        xIn = stickInput.x; //���E�̓��͊m�F
        yIn = stickInput.y; //�㉺�̓��͊m�F
        
        //�_���[�W���̖��G���Ȃ�
        if (0.0f < damageTime)
        {
            damageTime -= Time.deltaTime; //���G���Ԃ�����
            damageTime = Mathf.Max(damageTime, 0.0f); //���Ԃ𒲐�
        }
        //�����~�܂铮�쒆�Ȃ�
        //if ((damageTimeBase - 0.5f) <= damageTime)
        if (moveCoroutine != null)
        {
            xIn = 0.0f; //���E�̓��͂�0��
            yIn = 0.0f; //�㉺�̓��͂�0��
            return; //�I��
        }

        //�W�����v�����͂��ꂽ�ꍇ
        if (sceneController.InputActions["Jump"].triggered)
        {
            jumpKeyFlag = true; //�W�����v�L�[�t���O��true��
        }
        //�W�����v�����͂���Ă���ꍇ
        if (0.0f != jumpInput)
        {
            //jumpKeyFlag = true; //�W�����v�L�[�t���O��true��
            jumpKeyTime += Time.deltaTime; //���͎��Ԃ𑝉�
        }
        else
        {
            jumpKeyFlag = false; //�W�����v�L�[�t���O��false��
            jumpKeyTime = 0.0f; //���͎��Ԃ�������
        }
    }

    //�U��
    private void Attack()
    {
        //���͂̎擾
        bool attackInput = sceneController.PlayerInput.actions["Fire"].triggered;
        bool attackInput2 = sceneController.PlayerInput.actions["Fire2"].triggered;

        //�������U���{�^�������͂��ꂽ�ꍇ
        if (attackInput && canShot)
        {
            //�U�����ł͂Ȃ��Ȃ�
            if (!attackFlag)
            {
                attackFlag = true; //�U���t���O��true��
                moveCoroutine = StartCoroutine(ShotAttack(stickInput.y)); //�U�����J�n
            }
        }

        //�ߋ����U���{�^�������͂��ꂽ�ꍇ
        if (attackInput2 && canPunch)
        {
            //�U�����ł͂Ȃ��Ȃ�
            if (!attackFlag)
            {
                attackFlag = true; //�U���t���O��true��
                //���͕����ɂ���čU��������
                if(0.5f <= stickInput.y)
                {
                    moveCoroutine = StartCoroutine(PowerPunchAttack()); //���U�����J�n
                }
                else
                {
                    moveCoroutine = StartCoroutine(PunchAttack()); //��U�����J�n
                }
            }
        }
    }

    //��
    public void Healing(int heal)
    {
        hp += heal; //HP����
        hp = Math.Min(hp, Calculation.PLAYER_HP_MAX); //HP�𒲐�
        eManager.PlaySe("heal.ogg"); //���ʉ����Đ�
        //�G�t�F�N�g�𔭐�
        effManager.CreateEffect(EffectNameList.EFFECT_HEAL, transform.position, transform.rotation, transform);
    }

    //�񕜃G�l���M�[�̑���
    public void GainHealEnergy(int gainEn)
    {
        healEnergy += gainEn; //�񕜃G�l���M�[�𑝉�
        healEnergy = Math.Min(healEnergy, Calculation.HEAL_EN_MAX); //�G�l���M�[�𒲐�
        eManager.PlaySe("heal.ogg"); //���ʉ����Đ�
    }

    //�����蔻��ƐڐG�����ꍇ
    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    //�����蔻��ƐڐG�����ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool damageFlag = false; //�_���[�W���󂯂邩
        int damageParam = 0; //�_���[�W�l

        //�e�ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AbstructBullet ab = collision.gameObject.GetComponent<AbstructBullet>(); //�I�u�W�F�N�g���擾

            //���@�ɓ�����ꍇ
            if (ab.HitPlayer)
            {
                damageFlag = true; //�_���[�W�t���O��true��
                damageParam = ab.Damage; //�_���[�W�l���擾
            }
        }

        //�_���[�W����ɓ��������ꍇ
        if (collision.gameObject.CompareTag("Damage"))
        {
            damageFlag = true; //�_���[�W�t���O��true��
            damageParam = 1; //�_���[�W�l��ݒ�
        }
        //�������ꍇ
        else if(collision.gameObject.CompareTag("DeadZone"))
        {
            damageFlag = true; //�_���[�W�t���O��true��
            damageParam = Calculation.PLAYER_HP_MAX; //�_���[�W�l��ݒ�
            damageTime = 0.0f; //�_���[�W���G������
        }

        //�_���[�W���󂯂�Ȃ�
        if(damageFlag && damageTime <= 0.0f)
        {
            hp -= damageParam; //HP������
            eManager.PlaySe(SeNameList.SE_HIT); //���ʉ����Đ�
            damageTime = damageTimeBase; //���G���Ԃ�ݒ�

            //HP��0�ɂȂ�����
            if (hp <= 0 && moveFlag)
            {
                effManager.CreateEffect(EffectNameList.EFFECT_DEFEAT, transform.position, transform.rotation); //�G�t�F�N�g�𐶐�
                moveFlag = false; //����t���O��false��

                //���@���\���ɂ���
                foreach (Renderer ren in rendererList)
                {
                    ren.enabled = false;
                }
            }

            //�U�����Ȃ�
            if (attackFlag)
            {
                attackFlag = false; //�U���t���O��false��
                //�R���[�`�������s���Ȃ�
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine); //�R���[�`�����~
                }
            }
            moveCoroutine = StartCoroutine(DamageMotion()); //�_���[�W������J�n
        }
    }

    //�������U���̃R���[�`��
    private IEnumerator ShotAttack(float inputY)
    {
        eManager.PlaySe("pShoot.ogg"); //���ʉ����Đ�
        animator.Play(shotAnimHash); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.2f);
        float zRotate = (1.0f <= inputY) ? 45.0f * transform.localScale.x : 0.0f ;//���͂ɂ���Ċp�x��ݒ�
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, zRotate);
        bulletManager.CreateBullet(pBullet[0], transform.position, rotation, transform); //�e�𐶐�
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        attackFlag = false; //�U���t���O��false��
        moveCoroutine = null; //�R���[�`����������
        yield return null;
    }

    //�ߋ����U���i�p���`�j�̃R���[�`��
    private IEnumerator PunchAttack()
    {
        eManager.PlaySe("pPunch.ogg"); //���ʉ����Đ�
        animator.Play(punchAnimHash, 0, 0); //�A�j���[�V�������Đ�
        yield return new WaitForSeconds(0.1f);
        Vector3 pos = new Vector3(transform.position.x + (1.3f * transform.localScale.x), transform.position.y, transform.position.z);
        bulletManager.CreateBullet(pBullet[1], pos, transform.rotation, transform); //�e�𐶐�
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        attackFlag = false; //�U���t���O��false��
        moveCoroutine = null; //�R���[�`����������
        yield return null;
    }

    //�ߋ����U���i���p���`�j�̃R���[�`��
    private IEnumerator PowerPunchAttack()
    {
        eManager.PlaySe("pPowerPunch.ogg"); //���ʉ����Đ�
        animator.Play(powPunchAnimHash, 0, 0); //�A�j���[�V�������Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        attackFlag = false; //�U���t���O��false��
        moveCoroutine = null; //�R���[�`����������
        yield return null;
    }

    //�_���[�W�̃R���[�`��
    private IEnumerator DamageMotion()
    {
        animator.Play(damageAnimHash, 0, 0); //�A�j���[�V�������Đ�
        yield return null;
        //�A�j���[�V�������I���܂őҋ@
        while (true)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (1.0f <= currentState.normalizedTime)
            {
                break;
            }
            yield return null;
        }
        animator.Play(standAnimHash); //�A�j���[�V�������Đ�
        attackFlag = false; //�U���t���O��false��
        moveCoroutine = null; //�R���[�`����������
        yield return null;
    }

}
