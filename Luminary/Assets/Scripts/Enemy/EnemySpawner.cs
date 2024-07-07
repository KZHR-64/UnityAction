using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�Ăяo���A�A��p
public class EnemySpawner : MonoBehaviour
{
    private GameObject spawnEnemy = null; //�Ăяo���G
    private AbstructEnemy abstructEnemy = null; //�o�����G
    private Vector3 spawnPosition; //�o���ʒu
    private bool spawnedFlag = false; //�o�����t���O

    private EnemyManager enemyManager = null; //�G�֘A�̃I�u�W�F�N�g
    private EffectManager effManager = null; //�G�t�F�N�g�֘A�̃I�u�W�F�N�g

    // Update is called once per frame
    void Update()
    {
        //�o�����G������Ă���Ȃ�
        if (spawnedFlag && !abstructEnemy)
        {
            Destroy(this); //�I�u�W�F�N�g������
        }
    }

    //�ŏ��̐ݒ�
    public void FirstSetting(GameObject ene, Vector3 pos, EnemyManager em, EffectManager eff)
    {
        spawnEnemy = ene;
        spawnPosition = pos;
        enemyManager = em;
        effManager = eff;
        StartCoroutine(CreateEnemy()); // �G���o��
    }

    //�G���o���R���[�`��
    private IEnumerator CreateEnemy()
    {
        effManager.CreateEffect(EffectNameList.EFFECT_SPAWN, spawnPosition, new Quaternion()); //�o���G�t�F�N�g���o��

        yield return new WaitForSeconds(0.5f); //�����ҋ@
        abstructEnemy = enemyManager.CreateEnemy(spawnEnemy, spawnPosition, new Quaternion()); //�G���o��
        spawnedFlag = true; //�o�����t���O��true��
        yield return null;
    }
}
