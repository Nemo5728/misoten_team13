using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TrueSync;

public class loginArea : MonoBehaviour
{

    public Vector3 posAreaTex;        //計算用座標（位置座標）
    public GameObject AreaTex;        //オブジェクト本体（プレハブ）
    private GameObject AreaCreator;
    Color CircleColorValue;//色付け用

    const int PlayerNam_Max = 4;
    
    int onArea;//何人床に乗っているか？
    bool bGetReadyOK;//準備が完了したか？

    public GameObject refNet;

    // Use this for initialization
    void Start()
    {

        onArea = 0;
        bGetReadyOK = false;
        //bPlayerArea = false;

        //初期座標設定
        posAreaTex = new Vector3(0.0f, 0.0f, 0.0f);

        //色を設定
        CircleColorValue = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        AreaCreator = Instantiate(AreaTex); //オブジェクト割り振り
        AreaCreator.transform.position = posAreaTex;  //初期座標設定
        AreaCreator.GetComponent<MeshRenderer>().material.color = CircleColorValue;//色を変更

        
    }

    // Update is called once per frame
    void Update()
    {
        //pos情報を渡す
        loginPlayer GetReady = refNet.GetComponent<loginPlayer>();
        GetReady.SetAreaPos(posAreaTex);
    }
}