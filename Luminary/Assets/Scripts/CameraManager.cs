using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    private BackGround[] backGroundList = null; //”wŒi

    // Start is called before the first frame update
    void Start()
    {
        backGroundList = GetComponentsInChildren<BackGround>(); //”wŒi‚ðŽæ“¾
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
