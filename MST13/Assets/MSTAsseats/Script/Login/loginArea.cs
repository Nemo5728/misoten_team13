using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginArea : MonoBehaviour
{

    public Vector3 posAreaTex;        //計算用座標（位置座標）
    public GameObject AreaTex;        //オブジェクト本体（プレハブ）
    private GameObject AreaCreator;
    Color CircleColorValue;//色付け用

    private Vector3[] targetPos;//プレイヤーの位置を入れる
    const int PlayerNam_Max = 4;

    const int AreaSize = 5;//床の大きさ

    bool[] bPlayerArea;
    int onArea;//何人床に乗っているか？
    bool bGetReadyOK;//準備が完了したか？

    public GameObject refNet;

    // Use this for initialization
    void Start()
    {
        
        onArea = 0;
        bGetReadyOK = false;
        bPlayerArea = new bool[PlayerNam_Max];

        //初期座標設定
        posAreaTex = new Vector3(0.0f, 0.0f, 0.0f);

        //色を設定
        CircleColorValue = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        AreaCreator = Instantiate(AreaTex); //オブジェクト割り振り
        AreaCreator.transform.position = posAreaTex;  //初期座標設定
        AreaCreator.GetComponent<MeshRenderer>().material.color = CircleColorValue;//色を変更

        //ターゲット生成
        targetPos = new Vector3[PlayerNam_Max];
        for (int i = 0; i < PlayerNam_Max; i++)
        {
            bPlayerArea[i] = false;//初期化
            targetPos[i] = new Vector3(0.0f, 0.0f, 0.0f);//位置初期化
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PlayerNam_Max; i++)
        {
            //当り判定：床の上に乗っているなら
            if (posAreaTex.x + AreaSize > targetPos[i].x &&
            posAreaTex.x - AreaSize < targetPos[i].x &&
            posAreaTex.z + AreaSize > targetPos[i].z &&
            posAreaTex.z - AreaSize < targetPos[i].z)
            {
                bPlayerArea[i] = true;
            }
            else
            {
                bPlayerArea[i] = false;
            }
        }

        for (int i = 0; i < PlayerNam_Max; i++)
        {
            if (bPlayerArea[i] == true)
            {
                onArea++;//床に乗っている数を数える
            }
        }
        //色を計算
        CircleColorValue.r = onArea * 0.25f;// (1 / PlayerNam_Max);//床に乗っている人数 x 0.25
        CircleColorValue.g = (PlayerNam_Max - onArea) * 0.25f;//(1 / PlayerNam_Max);//最大人数 - 床に乗っている人数 x 0.25
        AreaCreator.GetComponent<MeshRenderer>().material.color = CircleColorValue;//色を変更
        

        //4人とも床の上に乗ったら
        if (onArea == 4)
        {
            bGetReadyOK = true;
        }
        onArea = 0;

        //準備OKかどうかの情報を渡す
        loginNetwork GetReady = refNet.GetComponent<loginNetwork>();
        GetReady.GetReadyOK(bGetReadyOK);

        //デバックログ
        if (Input.GetKey("p"))
        {
            Debug.Log("床→:" + (posAreaTex.x + AreaSize));
            Debug.Log("床←:" + (posAreaTex.x - AreaSize));
            Debug.Log("床↑:" + (posAreaTex.z + AreaSize));
            Debug.Log("床↓:" + (posAreaTex.z - AreaSize));
            Debug.Log("プレイヤー0:" + targetPos[0]);
            Debug.Log("プレイヤー1:" + targetPos[1]);
            Debug.Log("プレイヤー2:" + targetPos[2]);
            Debug.Log("プレイヤー3:" + targetPos[3]);
            Debug.Log("床の色:" + CircleColorValue);
            Debug.Log("bool0:" + bPlayerArea[0]);
            Debug.Log("bool1:" + bPlayerArea[1]);
            Debug.Log("bool2:" + bPlayerArea[2]);
            Debug.Log("bool3:" + bPlayerArea[3]);
        }
    }

    public void SetTargetPos_0(Vector3 pos)
    {
        targetPos[0] = pos;
    }
    public void SetTargetPos_1(Vector3 pos)
    {
        targetPos[1] = pos;
    }
    public void SetTargetPos_2(Vector3 pos)
    {
        targetPos[2] = pos;
    }
    public void SetTargetPos_3(Vector3 pos)
    {
        targetPos[3] = pos;
    }
}