using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackGround : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeedX = 0.0f; //�X�N���[�����x

    [SerializeField]
    private float scrollSpeedY = 0.0f; //�X�N���[�����x

    [SerializeField]
    private GameObject linkBackX; //�A��������w�i

    [SerializeField]
    private GameObject linkBackY; //�A��������w�i

    [SerializeField]
    private GameObject linkBackXY; //�A��������w�i

    private float imgSizeX; //�w�i�摜�̑傫��
    private float imgSizeY; //�w�i�摜�̑傫��

    // Start is called before the first frame update
    void Start()
    {
        imgSizeX = GetComponent<SpriteRenderer>().bounds.size.x; //�摜�̑傫�����擾
        imgSizeY = GetComponent<SpriteRenderer>().bounds.size.y;

        //�Ȃ���w�i������Ȃ�
        if (linkBackX)
        {
            linkBackX.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite; //�摜���R�s�[
        }
        if (linkBackY)
        {
            linkBackY.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite; //�摜���R�s�[
        }
        if (linkBackXY)
        {
            linkBackXY.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite; //�摜���R�s�[
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�J�����ɍ��킹�ړ�
    public void SetPosition(Vector3 cameraPosition)
    {
        float scrollX = cameraPosition.x; //�J������x���W���擾
        float scrollY = cameraPosition.y; //�J������y���W���擾

        float posX = (scrollX * scrollSpeedX) % imgSizeX; //�摜��u���ʒu��ݒ�
        float posY = (scrollY * scrollSpeedY) % imgSizeY;

        transform.position = new Vector3(scrollX - posX, scrollY - posY, 0.0f); //���W��ݒ�

        //�Ȃ���w�i������Ȃ�
        if (linkBackX)
        {
            float linkPos = (0.0f <= posX) ? 1.0f : -1.0f; //�w�i�̍��W�ɉ����ĂȂ���ʒu��ݒ�
            linkBackX.transform.position = transform.position + new Vector3(imgSizeX * linkPos, 0.0f, 0.0f); //�摜���Ȃ���
        }
        if (linkBackY)
        {
            float linkPos = (0.0f <= posY) ? 1.0f : -1.0f; //�w�i�̍��W�ɉ����ĂȂ���ʒu��ݒ�
            linkBackY.transform.position = transform.position + new Vector3(0.0f, imgSizeY * linkPos, 0.0f); //�摜���Ȃ���
        }
        if (linkBackXY)
        {
            float linkPosX = (0.0f <= posX) ? 1.0f : -1.0f; //�w�i�̍��W�ɉ����ĂȂ���ʒu��ݒ�
            float linkPosY = (0.0f <= posY) ? 1.0f : -1.0f;
            linkBackXY.transform.position = transform.position + new Vector3(imgSizeX * linkPosX, imgSizeY * linkPosY, 0.0f); //�摜���Ȃ���
        }
    }
}
