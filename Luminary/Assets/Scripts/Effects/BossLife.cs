using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLife : MonoBehaviour
{
    [SerializeField]
    private Image bossLifeBar = null; //�̗̓o�[�̉摜

    private AbstructEnemy bossObject = null; //�{�X�̃I�u�W�F�N�g

    // Update is called once per frame
    void Update()
    {
        //�{�X�����Ȃ����
        if(!bossObject)
        {
            gameObject.SetActive(false); //�̗̓o�[���\��
            return; //�I��
        }

        bossLifeBar.rectTransform.sizeDelta = new Vector2(480.0f * bossObject.Hp / bossObject.MaxHp, 48.0f); //�c��HP�ɉ����Ē�����ύX
    }

    //�{�X�̃I�u�W�F�N�g��ݒ�
    public void SetBossLife(AbstructEnemy enemy)
    {
        bossObject = enemy;
    }
}
