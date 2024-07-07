using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�Ƃ̘A��
public class EnemyWave : MonoBehaviour
{
    //�G���̍\����
    [System.Serializable]
    private struct SpawnStruct
    {
        public GameObject enemy; //�o���G
        public Vector3 position; //�o���ʒu
    }

    [SerializeField]
    EnemySpawner enemySpawner = null; // �G���o�����߂̃I�u�W�F�N�g
    [SerializeField]
    private int spawnMax = 1; //�����ɉ�ʂɏo���G�̐�
    [SerializeField]
    private bool checkZero = false; //��ʂ�|����Ă����[���邩
    [SerializeField]
    private List<SpawnStruct> spawnList = null; //�o���G�̏�񃊃X�g

    private bool activeFlag = false; //�J�n������
    private int enemyCount = 0; //�o�����G�̐�
    private bool replenishFlag = true; //�G���[����t���O
    private EnemySpawner[] spawnedEnemy = new EnemySpawner[4]; //���łɏo�Ă���G

    private EnemyManager enemyManager = null; //�G�֘A�̃I�u�W�F�N�g
    protected EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        spawnMax = Mathf.Clamp(spawnMax, 1, 4); //�����ɏo���G�̍ő吔�𒲐�
        StartCoroutine(FirstWait()); //�����ҋ@
    }

    // Update is called once per frame
    void Update()
    {
        int enemyNum = 0; //�G�̐�
        //�J�n���Ă��Ȃ��Ȃ�
        if (!activeFlag)
        {
            return; //�I��
        }

        //�����o���̐��J��Ԃ�
        for(int i = 0; i < spawnMax; i++)
        {
            //�o�Ă��Ȃ��G������Ȃ�
            if (!spawnedEnemy[i])
            {
                //�G���[����Ȃ�
                if (replenishFlag)
                {
                    CreateEnemy1(i); //�G���o��
                    enemyNum++; //�J�E���g�𑝂₷
                }
            }
            //�o�Ă���Ȃ�
            else
            {
                enemyNum++; //�J�E���g�𑝂₷
            }
        }
        //�ő�܂œG������A�o���\��Ȃ�
        if (spawnMax <= enemyNum && checkZero)
        {
            replenishFlag = false; //���΂炭��[���Ȃ�
        }
        //�G�����Ȃ��Ȃ�
        if(enemyNum <= 0 && checkZero)
        {
            replenishFlag = true; //�G���[����
        }
        //�Ō�܂ŏo������
        if (spawnList.Count <= enemyCount)
        {
            activeFlag = false; //�J�n�t���O��false��
        }
    }

    //�ŏ��̐ݒ�
    public void FirstSetting(EnemyManager em, Player p, EffectManager eff)
    {
        //player = p;
        enemyManager = em;
        effManager = eff;
    }

    //�G���o��
    private void CreateEnemy1(int num)
    {
        //�Ō�܂ŏo���Ă�����߂�
        if (spawnList.Count <= enemyCount) return;
        spawnedEnemy[num] = Instantiate<EnemySpawner>(enemySpawner, transform.position, transform.rotation); //�G�̌Ăяo���I�u�W�F�N�g�𐶐�
        spawnedEnemy[num].FirstSetting(spawnList[enemyCount].enemy, spawnList[enemyCount].position, enemyManager, effManager); //�����ݒ�
        enemyCount++; //�o�����G�̍��v�𑝉�
    }

    //�����ҋ@
    private IEnumerator FirstWait()
    {
        yield return new WaitForSeconds(0.2f);
        activeFlag = true; //�J�n�t���O��true��

        yield return null;
    }

    //�I���������m�F
    public bool CheckWaveEnd()
    {
        bool beatFlag = true; //�S���|������

        //�����o���̐��J��Ԃ�
        for (int i = 0; i < spawnedEnemy.Length; i++)
        {
            //�o�Ă���G������Ȃ�
            if (spawnedEnemy[i])
            {
                beatFlag = false; //�|�����t���O��false��
                break;
            }
        }
        return beatFlag && (spawnList.Count <= enemyCount);
    }
}
