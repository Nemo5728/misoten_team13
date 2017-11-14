using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class loginPlayer : TrueSyncBehaviour 
{
    // コントローラ設定
    private const byte INPUT_CONTROLLER_STICKX = 5;
    private const byte INPUT_CONTROLLER_STICKY = 6;
    private const byte INPUT_CONTROLLER_BUTTON = 7;
    private const byte INPUT_CONTROLLER_STICKBUTTON = 8;

    private TSRigidBody rb = null;                         // rigidbody
    private TSVector directionVector = TSVector.zero;      
    private ControllerInfo info = null;                    // コントローラー
    private bool knockout = false;
    private bool controllerConnect;

    [SerializeField, TooltipAttribute("移動速度")] private float speed;


    private Vector3 areaPos;//エリアの位置を入れる
    const float AreaSize = 2.5f;//床の大きさ
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
        knockout = false;
        controllerConnect = false;
        rb = GetComponent<TSRigidBody>();

        //初期化
        pos = new TSVector(8.0f, 0.0f, 8.0f);

        //
        rb = GetComponent<TSRigidBody>();

        //生成
        areaPos = new Vector3(0.0f, 0.0f, 0.0f);//位置初期化


    }

    //インプット
    public override void OnSyncedInput()
    {
        //BLEなんちゃら
        info = BLEControlManager.GetControllerInfo();
        //info = SerialControllManager.GetControllerInfo();

        if (info != null) controllerConnect = true;

        if (controllerConnect)
        {
            int stickX = info.stickX;
            int stickY = info.stickY;
            bool button = info.isButtonDown;
            bool stickBtn = info.isStickDown;

            TrueSyncInput.SetInt(INPUT_CONTROLLER_STICKX, stickX);
            TrueSyncInput.SetInt(INPUT_CONTROLLER_STICKY, stickY);
            TrueSyncInput.SetBool(INPUT_CONTROLLER_BUTTON, button);
            TrueSyncInput.SetBool(INPUT_CONTROLLER_STICKBUTTON, stickBtn);
        }

    }

    public override void OnSyncedUpdate()
    {
        if (!knockout)
        {
            TSVector vector = TSVector.zero;
 
            if (controllerConnect)
            {
                int stickX = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
                int stickY = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);
                bool button = TrueSyncInput.GetBool(INPUT_CONTROLLER_BUTTON);
                bool stickBtn = TrueSyncInput.GetBool(INPUT_CONTROLLER_STICKBUTTON);

                directionVector.x = vector.x = speed * (stickX / 473);
                directionVector.z = vector.z = speed * (stickY / 473);
            }

            TSVector.Normalize(vector);
            TSVector.Normalize(directionVector);

            rb.AddForce(speed * vector, ForceMode.Force);

            FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0.0f, (float)direction, 0.0f);
            tsTransform.rotation = TSQuaternion.Euler(0.0f, direction, 0.0f);

            pos = rb.position;//位置を変数posに代入

            //areaPos情報を取得
            loginArea GetReady = GetLoginArea.GetComponent<loginArea>();
            GetReady.SetAreaPos(areaPos);


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
            Debug.Log("areaPos" + areaPos);
            Debug.Log("rb.position" + rb.position);
            Debug.Log("pos" + pos);
        }

    }

    //public void SetAreaPos(Vector3 pos)
    //{
    //    areaPos = pos;
    //}

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







