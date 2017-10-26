using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class loginPlayer : MonoBehaviour {

    public Vector3[] posPlayerTex;        //計算用座標（位置座標）
    public GameObject[] PlayerTex;        //オブジェクト本体（プレハブ）
    private GameObject[] PlayerCreator;
    const int PlayerNam_Max = 4;          //最大数を設定

    public GameObject refObj;

    // Use this for initialization
    void Start () {

        posPlayerTex = new Vector3[PlayerNam_Max];//生成

        //初期座標設定
        posPlayerTex[0] = new Vector3(6.0f, 0.0f, 6.0f);//位置
        posPlayerTex[1] = new Vector3(6.0f, 0.0f, -6.0f);//位置
        posPlayerTex[2] = new Vector3(-6.0f, 0.0f, 6.0f);//位置
        posPlayerTex[3] = new Vector3(-6.0f, 0.0f, -6.0f);//位置

        PlayerCreator = new GameObject[PlayerNam_Max];

        for (int i = 0; i < PlayerNam_Max; i++)
        {
            GameObject playerObj = Instantiate(PlayerTex[i]);
            // cursorを子要素にする
            playerObj.transform.parent = this.transform;
            this.transform.GetChild(i).GetComponent<GameObject>();
            playerObj.name = "player" + i.ToString();               //名前セット
            PlayerCreator[i] = playerObj;                           //オブジェクト割り振り
            PlayerCreator[i].transform.position = posPlayerTex[i];  //初期座標設定
        }    
}

// Update is called once per frame
void Update () {
       
        //とりあえずの適当な移動処理
        //---------------------------------------------------------------------------------
        if (Input.GetKey("w"))
        {
            posPlayerTex[0].z += 0.1f;
        }
        if (Input.GetKey("s"))
        {
            posPlayerTex[0].z -= 0.1f;
        }
        if (Input.GetKey("d"))
        {
            posPlayerTex[0].x += 0.1f;
        }
        if (Input.GetKey("a"))
        {
            posPlayerTex[0].x -= 0.1f;
        }
        PlayerCreator[0].transform.position = posPlayerTex[0];//位置変更

        //---------------------------------------------------------------------------------
        if (Input.GetKey("f"))
        {
            posPlayerTex[1].z += 0.1f;
        }
        if (Input.GetKey("c"))
        {
            posPlayerTex[1].z -= 0.1f;
        }
        if (Input.GetKey("v"))
        {
            posPlayerTex[1].x += 0.1f;
        }
        if (Input.GetKey("x"))
        {
            posPlayerTex[1].x -= 0.1f;
        }
        PlayerCreator[1].transform.position = posPlayerTex[1];//位置変更

        //---------------------------------------------------------------------------------
        if (Input.GetKey("h"))
        {
            posPlayerTex[2].z += 0.1f;
        }
        if (Input.GetKey("n"))
        {
            posPlayerTex[2].z -= 0.1f;
        }
        if (Input.GetKey("m"))
        {
            posPlayerTex[2].x += 0.1f;
        }
        if (Input.GetKey("b"))
        {
            posPlayerTex[2].x -= 0.1f;
        }
        PlayerCreator[2].transform.position = posPlayerTex[2];//位置変更

        //---------------------------------------------------------------------------------
        if (Input.GetKey("i"))
        {
            posPlayerTex[3].z += 0.1f;
        }
        if (Input.GetKey("k"))
        {
            posPlayerTex[3].z -= 0.1f;
        }
        if (Input.GetKey("l"))
        {
            posPlayerTex[3].x += 0.1f;
        }
        if (Input.GetKey("j"))
        {
            posPlayerTex[3].x -= 0.1f;
        }
        PlayerCreator[3].transform.position = posPlayerTex[3];//位置変更

        //--------------------------------------------------------------------------------- 

        //pos情報を渡す
        loginArea area = refObj.GetComponent<loginArea>();
        area.SetTargetPos_0(posPlayerTex[0]);
        area.SetTargetPos_1(posPlayerTex[1]);
        area.SetTargetPos_2(posPlayerTex[2]);
        area.SetTargetPos_3(posPlayerTex[3]);

    }

}
