using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�e�[�W2���{�X�A�f�J�N�[�w�C
public class DekaKurhei1 : AbstructEnemy
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //�p�����̍X�V
    protected override void SubUpdate()
    {
        //�R���[�`�����I����Ă���Ȃ�
        if (moveCoroutine == null)
        {
            //���@�̈ʒu�ɍ��킹�Č�����ݒ�
            float lScale = (player.transform.position.x < transform.position.x) ? -1.0f : 1.0f;
            transform.localScale = new Vector2(lScale, 1.0f);

            //�p�^�[���ɂ���čs����ύX
            if (status == 0)
            {
                moveCoroutine = StartCoroutine(RapidShoot1()); //�R���[�`�����ĊJ
            }
            else if (status == 2)
            {
                moveCoroutine = StartCoroutine(RapidShoot2()); //�R���[�`�����ĊJ
            }
            else
            {
                moveCoroutine = StartCoroutine(MissileShoot()); //�R���[�`�����ĊJ
            }
        }
    }

    //�p�����̍X�V
    protected override void SubFixUpdate()
    {

    }

    //���ꂽ���̓���
    protected override void DefeatUpdate()
    {
        //���j����Ă����Ȃ�
        if (status == 0)
        {
            rBody.gravityScale = 1.0f; //�d�͂��󂯂�悤��
            yMove = false;
            status = 1; //��Ԃ�ύX
        }

        //��莞�Ԍo������
        if (0.2f <= time)
        {
            //�G�t�F�N�g�𔭐�
            effManager.CreateEffect(EffectNameList.EFFECT_EXPLODE_M, transform.position, transform.rotation);
            delFlag = true; //�G������
        }
    }

    //�e��A��1
    private IEnumerator RapidShoot1()
    {
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        SetYSpeed(-8.0f); //���ɍ~���
        yMove = true;
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        SetYSpeed(0.0f); //��~
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[1].position, transform.rotation, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.1f); //�����ҋ@
        }
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        float zAngle = Calculation.GetAngle(boneList[0].position, player.transform.position, true, transform.localScale.x, 30.0f); //���@�ւ̊p�x���擾
        Quaternion qua = Quaternion.Euler(0.0f, 0.0f, zAngle); //�p�x��ݒ�
        for (int i = 0; i < 5; i++)
        {
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.1f); //�����ҋ@
        }
        yield return new WaitForSeconds(3.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 1; //�p�^�[����ύX
        yield return null;
    }

    //�e��A��2
    private IEnumerator RapidShoot2()
    {
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        SetYSpeed(-8.0f); //���ɍ~���
        yMove = true;
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        SetYSpeed(0.0f); //��~
        for (int i = 0; i < 5; i++)
        {
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z + 30.0f); //�p�x��ݒ�
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, transform); //�e�𐶐�
            qua = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z - 30.0f); //�p�x��ݒ�
            bulletManager.CreateBullet(bulletList[0], boneList[0].position, qua, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.5f); //�����ҋ@
            bulletManager.CreateBullet(bulletList[0], boneList[1].position, transform.rotation, transform); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_1); //���ʉ����Đ�
            yield return new WaitForSeconds(0.5f); //�����ҋ@
        }
        yield return new WaitForSeconds(3.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = 3; //�p�^�[����ύX
        yield return null;
    }

    //�ǔ��~�T�C��
    private IEnumerator MissileShoot()
    {
        yield return new WaitForSeconds(1.0f); //�����ҋ@
        SetYSpeed(8.0f); //��ɏオ��
        yMove = true;
        yield return new WaitForSeconds(0.5f); //�����ҋ@
        SetYSpeed(0.0f); //��~
        for (int i = 0; i < 3; i++)
        {
            float zAngle = (transform.localScale.x < 0) ? 0.0f : 180.0f; // �����ɂ���Ċp�x��ݒ�
            Quaternion qua = Quaternion.Euler(0.0f, 0.0f, zAngle); //�p�x��ݒ�
            enemyManager.CreateEnemy(enemyList[0], boneList[2].position, qua); //�e�𐶐�
            eManager.PlaySe(SeNameList.SE_ESHOOT_2); //���ʉ����Đ�
            yield return new WaitForSeconds(1.0f); //�����ҋ@
        }
        yield return new WaitForSeconds(3.0f); //�����ҋ@
        moveCoroutine = null; //�R���[�`����������
        status = (status + 1) % 4; //�p�^�[����ύX
        yield return null;
    }
}
