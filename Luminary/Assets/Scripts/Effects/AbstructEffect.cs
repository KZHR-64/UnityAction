using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstructEffect : MonoBehaviour
{
    [SerializeField]
    protected float delTime = -1.0f; //�����鎞��

    protected Rigidbody2D rBody = null;
    protected Animator animator = null;
    protected bool delFlag = false; //�������邩
    protected float time = 0; //�o�ߎ���
    protected float xSpeed = 0.0f; //x�����̑��x
    protected float ySpeed = 0.0f; //y�����̑��x
    private bool changeXSpeed = false; //x�����̑��x��ς��邩
    private bool changeYSpeed = false; //y�����̑��x��ς��邩

    protected EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g
    protected SoundEffectManager eManager = null; //���ʉ��֘A�̃I�u�W�F�N�g

    public bool DelFlag { get { return delFlag; } set { delFlag = value; } } //�������邩

    protected void Awake()
    {
        rBody = GetComponent<Rigidbody2D>(); //RigidBody���擾
    }

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>(); //Animator���擾
    }

    // Update is called once per frame
    protected void Update()
    {
        if (delFlag) return; //��������Ȃ�I��
        time += Time.deltaTime; //���Ԃ𑝉�
        SubUpdate(); //�p�����̍X�V
        
        //�����鎞�ԂȂ�
        if(0.0f < delTime && delTime <= time)
        {
            delFlag = true; //�G�t�F�N�g������
        }
    }

    protected void FixedUpdate()
    {
        SubFixUpdate(); //�p�����̍X�V

        if (rBody)
        {
            float setX = changeXSpeed ? xSpeed : rBody.velocity.x; //�ݒ肷�鑬�x������
            float setY = changeYSpeed ? ySpeed : rBody.velocity.y;
            rBody.velocity = new Vector2(setX, setY); //���x�̐ݒ�
        }
        changeXSpeed = false; //���x�ύX�t���O��false��
        changeYSpeed = false;
    }

    //�ŏ��̐ݒ�
    public void FirstSetting(EffectManager em, SoundEffectManager sem, float scale)
    {
        effManager = em;
        eManager = sem;
        transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z * scale); //�G�t�F�N�g�̊g�k
        FirstUpdate();
    }

    //�ŏ��̍X�V
    protected virtual void FirstUpdate() { }

    //�p�����̍X�V
    protected virtual void SubUpdate() { }

    //�p�����̍X�V
    protected virtual void SubFixUpdate() { }

    //x�����̑��x��ݒ�
    protected void SetXSpeed(float setSpeed)
    {
        xSpeed = setSpeed; //���x��ݒ�
        changeXSpeed = true; //���x�ύX�t���O��true��
    }

    //y�����̑��x��ݒ�
    protected void SetYSpeed(float setSpeed)
    {
        ySpeed = setSpeed; //���x��ݒ�
        changeYSpeed = true; //���x�ύX�t���O��true��
    }
}
