using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerater : MonoBehaviour
{
    [SerializeField]
    private GameObject generateEnemy; //�Ăяo���G

    private bool generatedFrag = false; //�G���o������

    //�N������ɐG�ꂽ�ꍇ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (generatedFrag) return; //�Ăяo���ς݂Ȃ�I��

        //�N������Ȃ�
        if(collision.CompareTag("GeneratorActiveArea")) {
            Instantiate(generateEnemy, transform.position, transform.rotation); //�G�𐶐�
            generatedFrag = true; //�Ăяo���t���O��true��
        }
    }

    //�ċN���s���肩�痣�ꂽ�ꍇ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!generatedFrag) return; //�Ăяo���ς݂łȂ���ΏI��

        //�ċN���s����Ȃ�
        //�N������Ȃ�
        if (collision.CompareTag("GeneratorStopArea"))
        {
            generatedFrag = false; //�Ăяo���t���O��false��
        }
    }
}
