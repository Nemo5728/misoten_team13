using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class loginPlayer : TrueSyncBehaviour 
{
    // コントローラ設定
    private const byte INPUT_KEY_FORWARD = 0;
    private const byte INPUT_KEY_BACK = 1;
    private const byte INPUT_KEY_RIGHT = 2;
    private const byte INPUT_KEY_LEFT = 3;
    private const byte INPUT_KEY_SPACE = 4;
    private const byte INPUT_CONTROLLER_STICKX = 5;
    private const byte INPUT_CONTROLLER_STICKY = 6;
    private const byte INPUT_CONTROLLER_BUTTON = 7;
    private const byte INPUT_CONTROLLER_STICKBUTTON = 8;
    private const float STAGE_LENGTH = 56.0f;

    private TSRigidBody rb = null;                         // rigidbody
    private TSVector directionVector = TSVector.zero;      
    private ControllerInfo info = null;                    // コントローラー
    private bool knockout = false;
    private bool controllerConnect;
    private TSVector move;
    private Animator anim;  // アニメーター

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
        move = TSVector.zero;

        //生成
        areaPos = new Vector3(0.0f, 0.0f, 0.0f);//位置初期化
        anim = GetComponent<Animator>();    // アニメーションの取得

    }

    //インプット
    public override void OnSyncedInput()
    {
        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool space = Input.GetKeyDown(KeyCode.Space);

        TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
        TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
        TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
        TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);
        TrueSyncInput.SetBool(INPUT_KEY_SPACE, space);


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

            bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
            bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
            bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
            bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);
            bool space = TrueSyncInput.GetBool(INPUT_KEY_SPACE);

            TSVector vector = TSVector.zero;
            if (forward) directionVector += vector += TSVector.forward;
            if (back) directionVector += vector += TSVector.back;
            if (left) directionVector += vector += TSVector.left;
            if (right) directionVector += vector += TSVector.right;


            if (controllerConnect)
            {
                Debug.Log("TrueSyncコントローラなう");
                int stickX = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
                int stickY = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);
                bool button = TrueSyncInput.GetBool(INPUT_CONTROLLER_BUTTON);
                bool stickBtn = TrueSyncInput.GetBool(INPUT_CONTROLLER_STICKBUTTON);

                // 2017/12/1 追記
                // Playerの移動モーション管理
                MoveAnimetion(stickX);

                directionVector.x = vector.x = speed * (stickX / 473);
                directionVector.z = vector.z = speed * (stickY / 473);
            }

                  vector = TSVector.Normalize(vector);
                        directionVector = TSVector.Normalize(directionVector);
                        FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
                        tsTransform.rotation = TSQuaternion.Euler(0.0f, direction, 0.0f);

                        if (!(TSVector.Distance(TSVector.zero, tsTransform.position + vector) >= STAGE_LENGTH))
                            tsTransform.Translate(vector * speed, Space.World);


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
                Debug.Log("乗ってるよ！");
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

    // 2017/12/1 追加
    private void MoveAnimetion(float stickX)
    {
        // スティック傾けているかチェック*部品ごとの誤差対策
        if (stickX >= -20 && stickX <= 20)
        {
            // 
            anim.SetBool("bannerMove", false);
        }
        else
        {
            anim.SetBool("bannerMove", true);
        }
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







