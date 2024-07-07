using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackGround : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeedX = 0.0f; //スクロール速度

    [SerializeField]
    private float scrollSpeedY = 0.0f; //スクロール速度

    [SerializeField]
    private GameObject linkBackX; //連結させる背景

    [SerializeField]
    private GameObject linkBackY; //連結させる背景

    [SerializeField]
    private GameObject linkBackXY; //連結させる背景

    private float imgSizeX; //背景画像の大きさ
    private float imgSizeY; //背景画像の大きさ

    // Start is called before the first frame update
    void Start()
    {
        imgSizeX = GetComponent<SpriteRenderer>().bounds.size.x; //画像の大きさを取得
        imgSizeY = GetComponent<SpriteRenderer>().bounds.size.y;

        //つなげる背景があるなら
        if (linkBackX)
        {
            linkBackX.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite; //画像をコピー
        }
        if (linkBackY)
        {
            linkBackY.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite; //画像をコピー
        }
        if (linkBackXY)
        {
            linkBackXY.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite; //画像をコピー
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カメラに合わせ移動
    public void SetPosition(Vector3 cameraPosition)
    {
        float scrollX = cameraPosition.x; //カメラのx座標を取得
        float scrollY = cameraPosition.y; //カメラのy座標を取得

        float posX = (scrollX * scrollSpeedX) % imgSizeX; //画像を置く位置を設定
        float posY = (scrollY * scrollSpeedY) % imgSizeY;

        transform.position = new Vector3(scrollX - posX, scrollY - posY, 0.0f); //座標を設定

        //つなげる背景があるなら
        if (linkBackX)
        {
            float linkPos = (0.0f <= posX) ? 1.0f : -1.0f; //背景の座標に応じてつなげる位置を設定
            linkBackX.transform.position = transform.position + new Vector3(imgSizeX * linkPos, 0.0f, 0.0f); //画像をつなげる
        }
        if (linkBackY)
        {
            float linkPos = (0.0f <= posY) ? 1.0f : -1.0f; //背景の座標に応じてつなげる位置を設定
            linkBackY.transform.position = transform.position + new Vector3(0.0f, imgSizeY * linkPos, 0.0f); //画像をつなげる
        }
        if (linkBackXY)
        {
            float linkPosX = (0.0f <= posX) ? 1.0f : -1.0f; //背景の座標に応じてつなげる位置を設定
            float linkPosY = (0.0f <= posY) ? 1.0f : -1.0f;
            linkBackXY.transform.position = transform.position + new Vector3(imgSizeX * linkPosX, imgSizeY * linkPosY, 0.0f); //画像をつなげる
        }
    }
}
