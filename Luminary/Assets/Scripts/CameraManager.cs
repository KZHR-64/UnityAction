using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    private BackGround[] backGroundList = null; //�w�i

    // Start is called before the first frame update
    void Start()
    {
        backGroundList = GetComponentsInChildren<BackGround>(); //�w�i���擾
    }

    // Update is called once per frame
    void Update()
    {

        foreach(BackGround b in backGroundList)
        {
            b.SetPosition(transform.position);
        }
    }
}
