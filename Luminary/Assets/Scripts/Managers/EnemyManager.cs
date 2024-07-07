using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Player player; //���@�̃I�u�W�F�N�g
    [SerializeField]
    private EventManager eveManager = null; //�C�x���g�֘A�̃I�u�W�F�N�g
    [SerializeField]
    private ItemManager itemManager = null; //�A�C�e���Ǘ��̃I�u�W�F�N�g
    [SerializeField]
    private BulletManager bulletManager = null; //�e�̊Ǘ��I�u�W�F�N�g

    private List<AbstructEnemy> enemyList; //�e�̃��X�g
    private EnemyWave enemyWave = null; //�G�E�F�[�u
    private bool bossBattleFlag = false; //�{�X�풆��
    private bool bossLiveFlag = false; //�{�X�����邩

    private CameraManager cameraManager = null; //�J�����֘A�̃I�u�W�F�N�g
    private SoundEffectManager soundEffectManager = null; //���ʉ��֘A�̃I�u�W�F�N�g
    private EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    public bool BossBattleFlag { get { return bossBattleFlag; } } //�{�X�풆��

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>(); //�I�u�W�F�N�g���擾
        soundEffectManager = FindObjectOfType<SoundEffectManager>(); //�I�u�W�F�N�g���擾
        effManager = FindObjectOfType<EffectManager>(); //�I�u�W�F�N�g���擾

        enemyList = new List<AbstructEnemy>();
        GetComponentsInChildren<AbstructEnemy>(enemyList); //���łɂ���G���擾
        //�G������Ȃ�
        if (0 < enemyList.Count)
        {
            foreach (AbstructEnemy ae in enemyList)
            {
                ae.FirstSetting(this, player, bulletManager, soundEffectManager, effManager, cameraManager); //�ŏ��̍X�V
            }
        }
        enemyWave = GetComponentInChildren<EnemyWave>(); //�I�u�W�F�N�g���擾
        //�G�E�F�[�u������Ȃ�
        if(enemyWave)
        {
            enemyWave.FirstSetting(this, player, effManager); //�ŏ��̍X�V
        }
    }

    // Update is called once per frame
    void Update()
    {
        bossLiveFlag = false;
        //�G������Ȃ�
        if (0 < enemyList.Count)
        {
            //�G�̐��J��Ԃ�
            foreach (AbstructEnemy e in enemyList)
            {
                //�{�X������Ȃ�
                if(e.BossFlag)
                {
                    bossLiveFlag = true; //�{�X������t���O��true��
                    //�{�X�풆�łȂ��Ȃ�
                    if(!bossBattleFlag)
                    {
                        bossBattleFlag = true; //�{�X��t���O��true��
                    }
                }
                //�G�������Ȃ�
                if (e.DelFlag)
                {
                    //�A�C�e���𗎂Ƃ��Ȃ�
                    if (e.ItemDropFlag)
                    {
                        itemManager.LotteryItem(e.transform); //�A�C�e���̒��I
                    }
                    Destroy(e.gameObject); //�I�u�W�F�N�g������
                }
            }
            enemyList.RemoveAll(ene => ene.DelFlag);
        }
    }

    //�G�̐���
    public AbstructEnemy CreateEnemy(GameObject enemy, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject eObj = Instantiate(enemy, position, rotation, parent); //�G�𐶐�
        AbstructEnemy ae = eObj.GetComponent<AbstructEnemy>();
        ae.FirstSetting(this, player, bulletManager, soundEffectManager, effManager, cameraManager); //�ŏ��̍X�V
        enemyList.Add(ae); //�G�����X�g�ɒǉ�
        return ae; //�G��Ԃ�*/
    }

    //�{�X�킪�I��������擾
    public bool CheckBossBattleEnd()
    {
        if(enemyWave)
        {
            return enemyWave.CheckWaveEnd(); //�E�F�[�u���I��������m�F
        }
        //�{�X������Ă�����
        if (!bossLiveFlag && bossBattleFlag)
        {
            bossBattleFlag = false; //�{�X��t���O��false��
            return true;
        }
        return false;
    }

    //�{�X���j���̏���
    public void BeatBoss()
    {
        eveManager.ActiveBeatBossEvent(); //�{�X���j�C�x���g���J�n
    }
}
