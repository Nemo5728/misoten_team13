using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class loginPlayer : TrueSyncBehaviour 
{
    //キーボード関連
    private const byte INPUT_KEY_FORWARD = 0;
    private const byte INPUT_KEY_BACK = 1;
    private const byte INPUT_KEY_RIGHT = 2;
    private const byte INPUT_KEY_LEFT = 3;

    private TSRigidBody rb = null;//
    private TSVector directionVector = TSVector.zero;//プレイヤーのpos的なもの

    public float speed;

    private TSVector areaPos;//エリアの位置を入れる
    const int AreaSize = 5;//床の大きさ
    bool bPlayerArea;
    
    public GameObject GetLoginArea;
    TSVector pos;
    // Use this for initialization
    void Start()
    {
        

        bPlayerArea = false;
    }

    public override void OnSyncedStart()
    {
        //初期化
        pos = new TSVector(8.0f, 0.0f, 8.0f);

        //
        rb = GetComponent<TSRigidBody>();

        //生成
        areaPos = new TSVector(0.0f, 0.0f, 0.0f);//位置初期化


    }

    //インプット
    public override void OnSyncedInput()
    {
        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);

        TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
        TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
        TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
        TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);
    }

    public override void OnSyncedUpdate()
    {
        bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
        bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
        bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
        bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);

        Debug.Log(bPlayerArea);
       // if (bPlayerArea == false)//プレイヤーがエリアの上にいるなら
        //{ 
            //プレイヤー移動
            TSVector vector = TSVector.zero;//プレイヤー位置数値を初期化
            if (forward)
            {
                directionVector = vector += TSVector.forward;
            }
            if (back)
            {
                directionVector = vector += TSVector.back;
            }
            if (left)
            {
                directionVector = vector += TSVector.left;
            }
            if (right)
            {
                directionVector = vector += TSVector.right;
            }
            TSVector.Normalize(vector);//正規化
            rb.AddForce(speed * vector, ForceMode.Force);
            
       // }
        

        FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, (float)direction, 0.0f);//Rotを設定


        pos = rb.position;
       

        //当り判定：床の上に乗っているなら
        if (areaPos.x + AreaSize > pos.x &&
        areaPos.x - AreaSize < pos.x &&
        areaPos.z + AreaSize > pos.z &&
        areaPos.z - AreaSize < pos.z)
        {
            bPlayerArea = true;
        }
        else
        {
            bPlayerArea = false;
        }
        Debug.Log(areaPos.x);
        Debug.Log(areaPos.z);
        Debug.Log(pos);
    }

   

    public void SetAreaPos(Vector3 pos)
    {
        areaPos.x = pos.x;
        areaPos.y = pos.y;
        areaPos.z = pos.z;

    }

    // Update is called once per frame
    void Update()
    {
          
    }

}





























//public class loginPlayer : MonoBehaviour {

//    public Vector3[] posPlayerTex;        //計算用座標（位置座標）
//    public GameObject[] PlayerTex;        //オブジェクト本体（プレハブ）
//    private GameObject[] PlayerCreator;
//    const int PlayerNam_Max = 4;          //最大数を設定

//    public GameObject refObj;

//    // Use this for initialization
//    void Start () {

//        posPlayerTex = new Vector3[PlayerNam_Max];//生成

//        //初期座標設定
//        posPlayerTex[0] = new Vector3(6.0f, 0.0f, 6.0f);//位置
//        posPlayerTex[1] = new Vector3(6.0f, 0.0f, -6.0f);//位置
//        posPlayerTex[2] = new Vector3(-6.0f, 0.0f, 6.0f);//位置
//        posPlayerTex[3] = new Vector3(-6.0f, 0.0f, -6.0f);//位置

//        PlayerCreator = new GameObject[PlayerNam_Max];

//        for (int i = 0; i < PlayerNam_Max; i++)
//        {
//            GameObject playerObj = Instantiate(PlayerTex[i]);
//            // cursorを子要素にする
//            playerObj.transform.parent = this.transform;
//            this.transform.GetChild(i).GetComponent<GameObject>();
//            playerObj.name = "player" + i.ToString();               //名前セット
//            PlayerCreator[i] = playerObj;                           //オブジェクト割り振り
//            PlayerCreator[i].transform.position = posPlayerTex[i];  //初期座標設定
//        }    
//}

//// Update is called once per frame
//void Update () {

//        //とりあえずの適当な移動処理
//        //---------------------------------------------------------------------------------
//        if (Input.GetKey("w"))
//        {
//            posPlayerTex[0].z += 0.1f;
//        }
//        if (Input.GetKey("s"))
//        {
//            posPlayerTex[0].z -= 0.1f;
//        }
//        if (Input.GetKey("d"))
//        {
//            posPlayerTex[0].x += 0.1f;
//        }
//        if (Input.GetKey("a"))
//        {
//            posPlayerTex[0].x -= 0.1f;
//        }
//        PlayerCreator[0].transform.position = posPlayerTex[0];//位置変更

//        //---------------------------------------------------------------------------------
//        if (Input.GetKey("f"))
//        {
//            posPlayerTex[1].z += 0.1f;
//        }
//        if (Input.GetKey("c"))
//        {
//            posPlayerTex[1].z -= 0.1f;
//        }
//        if (Input.GetKey("v"))
//        {
//            posPlayerTex[1].x += 0.1f;
//        }
//        if (Input.GetKey("x"))
//        {
//            posPlayerTex[1].x -= 0.1f;
//        }
//        PlayerCreator[1].transform.position = posPlayerTex[1];//位置変更

//        //---------------------------------------------------------------------------------
//        if (Input.GetKey("h"))
//        {
//            posPlayerTex[2].z += 0.1f;
//        }
//        if (Input.GetKey("n"))
//        {
//            posPlayerTex[2].z -= 0.1f;
//        }
//        if (Input.GetKey("m"))
//        {
//            posPlayerTex[2].x += 0.1f;
//        }
//        if (Input.GetKey("b"))
//        {
//            posPlayerTex[2].x -= 0.1f;
//        }
//        PlayerCreator[2].transform.position = posPlayerTex[2];//位置変更

//        //---------------------------------------------------------------------------------
//        if (Input.GetKey("i"))
//        {
//            posPlayerTex[3].z += 0.1f;
//        }
//        if (Input.GetKey("k"))
//        {
//            posPlayerTex[3].z -= 0.1f;
//        }
//        if (Input.GetKey("l"))
//        {
//            posPlayerTex[3].x += 0.1f;
//        }
//        if (Input.GetKey("j"))
//        {
//            posPlayerTex[3].x -= 0.1f;
//        }
//        PlayerCreator[3].transform.position = posPlayerTex[3];//位置変更

//        //--------------------------------------------------------------------------------- 

//        //pos情報を渡す
//        loginArea area = refObj.GetComponent<loginArea>();
//        area.SetTargetPos_0(posPlayerTex[0]);
//        area.SetTargetPos_1(posPlayerTex[1]);
//        area.SetTargetPos_2(posPlayerTex[2]);
//        area.SetTargetPos_3(posPlayerTex[3]);

//    }

//}







