using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class monster : TrueSyncBehaviour {

    private const byte INPUT_KEY_FORWARD = 0;
    private const byte INPUT_KEY_BACK = 1;
    private const byte INPUT_KEY_RIGHT = 2;
    private const byte INPUT_KEY_LEFT = 3;
    private const byte INPUT_KEY_SPACE = 4;
    private const byte INPUT_CONTROLLER_STICKX = 5;
    private const byte INPUT_CONTROLLER_STICKY = 6;
    private const byte INPUT_CONTROLLER_BUTTON = 7;
    private const byte INPUT_CONTROLLER_STICKBUTTON = 8;

    private TSRigidBody rb = null;                          // RigidBodyの取得
    private TSVector directionVector = TSVector.zero;       
    private ControllerInfo info = null;                     // コントローラ
    private bool knockout = false;
    private int loveGauge;
    private float timeLeft = 1.0f;
    private bool transformFlag = false;
    private float transformCount = 0.0f;
    private int powerUpButton = 0;
    private float powerUpCount = 0.0f;
    private bool powerUpFlag;
    private bool controllerConnect;
    private Animator anime;             //アニメーター
    private TSVector move;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private int attackSpeed = 0;
    [SerializeField, TooltipAttribute("移動速度")] private float speed;
    [SerializeField, TooltipAttribute("ラブゲージMAX")] private int loveGaugeMax = 100;
    [SerializeField, TooltipAttribute("ラブゲージ上昇率")] private int loveGaugeLate = 1;
    [SerializeField, TooltipAttribute("変身時攻撃力")] private int attack = 1;
    [SerializeField, TooltipAttribute("連打判定終了時間")] private float powerUpEndTime = 0.5f;
    [SerializeField, TooltipAttribute("変身時HP")] private int health = 100;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject playerObject;
    [SerializeField, TooltipAttribute("移動減衰係数"), Range(0.0f, 1.0f)] private float drag;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	// 初期化
	public override void OnSyncedStart()
	{
        
		anime = GetComponent<Animator> ();	//アニメーションの取得
		rb = GetComponent<TSRigidBody>();	// RigidBodyの取得
        knockout = false;
        transformFlag = false;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        controllerConnect = false;
        loveGauge = loveGaugeMax;
        move = TSVector.zero;
        // 出現モーション
        anime.SetTrigger("monsterTransform");
	}

	// インプット
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

	// 更新
	public override void OnSyncedUpdate()
	{

        if (!knockout)
        {
            // 攻撃アニメーション情報を取得
            bool isAttakc = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monsterWeakAttack");

            bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
            bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
            bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
            bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);
            bool space = TrueSyncInput.GetBool(INPUT_KEY_SPACE);

            TSVector vector = TSVector.zero;

            if (forward) directionVector = vector += TSVector.forward;
            if (back) directionVector = vector += TSVector.back;
            if (left) directionVector = vector += TSVector.left;
            if (right) directionVector = vector += TSVector.right;

            if (controllerConnect)
            {
                Debug.Log("コントローラ繋がってるよ！");

                // スティック処理
                int stickX = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
                int stickY = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);
                bool button = TrueSyncInput.GetBool(INPUT_CONTROLLER_BUTTON);
                bool stickBtn = TrueSyncInput.GetBool(INPUT_CONTROLLER_STICKBUTTON);

                directionVector.x = vector.x = speed * (stickX / 473);
                directionVector.z = vector.z = speed * (stickY / 473);


                // スティック傾けているかチェック*部品ごとの誤差対策
                if(stickX >=  -20 && stickX <= 20)
                {
                    // 
                    anime.SetBool("monsterMove", false);
                }
                else
                {
                    anime.SetBool("monsterMove", true); 
                }
          
                // 攻撃モーション中は移動不可にしたい
                if (isAttakc)
                {
                    speed = 0f;
                    if (stickBtn)
                    {
                        // 弱攻撃モーション
                        anime.SetTrigger("monsterStrAttack");

                    }

                }
                else
                {
                    if (stickBtn)
                    {
                        Debug.Log("スティックボタン押されたよ！");
                        // 弱攻撃モーション
                        anime.SetTrigger("monsterWeakAttack");

                    }
                    speed = 3f;
                }

                // ボタンが押されたら
                if(button == true)
                {
                    Debug.Log("ボタン押されたよ！");
                    // 弱攻撃モーション
                    anime.SetTrigger("monsterWeakAttack");
                  
                }


            }

            // ラブゲージ処理
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                loveGauge -= loveGaugeLate;
                timeLeft = 1.0f;
            }

            // ラブゲージが尽きたら
            if(loveGauge <= 0)
            {
                // 時間切れ
                anime.SetTrigger("monsterSplit");
                // playerにチェンジ
                playerObject.SetActive(true);
                GetComponent<player>().TransformInit(tsTransform.position, tsTransform.rotation);
                gameObject.SetActive(false);
            }

            // 体力処理
            if(health <= 0)
            {
                // 撃破モーション
                anime.SetTrigger("monsterDown");
                // playerにチェンジ
                playerObject.SetActive(true);
                GetComponent<player>().TransformInit(tsTransform.position, tsTransform.rotation);
                gameObject.SetActive(false);
            }
            if(isAttakc)
            {
                Debug.Log(("攻撃モーションだよ！"));
            }
                TSVector.Normalize(vector);
                TSVector.Normalize(directionVector);

                //rb.AddForce(speed * vector, ForceMode.Force);

                FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
                //transform.rotation = Quaternion.Euler(0.0f, (float)direction, 0.0f);
                tsTransform.rotation = TSQuaternion.Euler(0.0f, direction, 0.0f);

                move += vector * speed;
                tsTransform.Translate(move, Space.World);
                move.x += (0.0f - move.x) / drag;
                move.z += (0.0f - move.z) / drag;
        }
       
	
	}

    public void TransformInit(TSVector pos, TSQuaternion rot){
        tsTransform.position = pos;
        tsTransform.rotation = rot;

        knockout = false;
        transformFlag = false;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        controllerConnect = false;
        move = TSVector.zero;
        loveGauge = loveGaugeMax;
    }
}
