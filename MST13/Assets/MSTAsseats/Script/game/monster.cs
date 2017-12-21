using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private const byte INPUT_ATTACK = 9;
    private const float STAGE_LENGTH = 56.0f;

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
    private bool bAttack;               // 攻撃

    private Animator anime;             //アニメーター
    private TSVector move;
    private GameObject ready;           // readyオブジェクト

    // ステータス制御
    private enum STATE
    {
        STATE_NONE,
        STATE_TRANSFORM,    // 変身時
        STATE_NORMAL,       // 通常
        STATE_ATTACK,       // 攻撃時
        STATE_KNOCKOUT,     // ダウン
        STATE_SPLIT          // 時間消滅
    };
    private STATE state;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private int attackSpeed = 0;
    [SerializeField, TooltipAttribute("移動速度")] private float speed;
    [SerializeField, TooltipAttribute("ラブゲージMAX")] private int loveGaugeMax = 100;
    [SerializeField, TooltipAttribute("ラブゲージ上昇率")] private int loveGaugeLate = 1;
    [SerializeField, TooltipAttribute("変身時攻撃力(弱)")] public int WeakAttack = 1;
    [SerializeField, TooltipAttribute("変身時攻撃力(強）")] public int StrAttack = 3;
    [SerializeField, TooltipAttribute("連打判定終了時間")] private float powerUpEndTime = 0.5f;

    [AddTracking]
    [SerializeField, TooltipAttribute("変身時HP")] private int health = 100; //HPの変数をAddTrakingする

    [SerializeField, TooltipAttribute("触るな危険")] private GameObject playerObject;
    [SerializeField, TooltipAttribute("移動減衰係数"), Range(0.0f, 1.0f)] private float drag;


    public bool isAttakc;               // 弱攻撃判定
    public bool isStrAttack;            // 強攻撃判定

    public GameObject Collider;         // コライダーsss
    public Text DebugHealthText;

    private float DefSpeed;          // 設定速度を保管

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update ()
    {
      //  DebugHealthText.text = "HP:" + health.ToString();

        if(Input.GetKey(KeyCode.T))
        {
            health -= 10;
        }

    /*     
        if (Input.GetKeyDown(KeyCode.Y))
       {
          //  if (isStrAttack) return;
            // 弱攻撃モーション
            anime.SetTrigger("monsterWeakAttack");
        }
     */           
    }

	// 初期化
	public override void OnSyncedStart()
	{
        DefSpeed = speed;
        ready = GameObject.Find("ready");
		anime = GetComponent<Animator> ();	//アニメーションの取得
		rb = GetComponent<TSRigidBody>();	// RigidBodyの取得
        knockout = false;
        transformFlag = false;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        health = 100;
        powerUpFlag = false;
        controllerConnect = false;
        bAttack = false;
        loveGauge = loveGaugeMax;
        move = TSVector.zero;
        state = STATE.STATE_TRANSFORM;
    
	}

	// インプット
	public override void OnSyncedInput()
	{
        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool space = Input.GetKeyDown(KeyCode.Space);
        bool weakAttack = Input.GetKeyDown(KeyCode.Y);
        TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
        TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
        TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
        TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);
        TrueSyncInput.SetBool(INPUT_KEY_SPACE, space);
        TrueSyncInput.SetBool(INPUT_ATTACK,weakAttack);

        //BLEなんちゃら
        //info = BLEControlManager.GetControllerInfo();
        info = SerialControllManager.GetControllerInfo();
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
      //  if (ready != null) return;

        if (!knockout)
        {
            
           // Debug.Log("HP" + health);
            // 攻撃アニメーション情報を取得
            isAttakc    = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monsterWeakAttack");
            isStrAttack = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monsterStrAttack");


            bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
            bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
            bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
            bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);
            bool space = TrueSyncInput.GetBool(INPUT_KEY_SPACE);
            bool weakAttack = TrueSyncInput.GetBool(INPUT_ATTACK);

            TSVector vector = TSVector.zero;

            if (forward) directionVector = vector += TSVector.forward;
            if (back) directionVector = vector += TSVector.back;
            if (left) directionVector = vector += TSVector.left;
            if (right) directionVector = vector += TSVector.right;

            if (controllerConnect)
            {
              //  Debug.Log("コントローラ繋がってるよ！");

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
          
                ///// 攻撃モーション中は移動不可にしたい /////
                if (isAttakc)
                {
                    speed = 0f; // 移動速度を0に

                    // スティックを押したら
                    if (stickBtn || weakAttack)
                    {
                        if (isStrAttack) return;
                        // 弱攻撃モーション
                        anime.SetTrigger("monsterStrAttack");
                        GetComponent<ParticleManager>().Play("FX_SwingB" , transform.position);
                    }
                }
                ///// その他のモーション /////
                else
                {
                    // スティックを押したら
                    if (stickBtn || weakAttack)
                    {
                        if (bAttack) return;    // 
                        //Debug.Log("スティックボタン押されたよ！");

                        bAttack = true;

                        // 弱攻撃モーション
                        anime.SetTrigger("monsterWeakAttack");
                        GetComponent<ParticleManager>().Play("FX_SwingA", transform.position);
                        //HitWeakAttack(hitWeakObject, hitWeakOffset);

                    }
                    speed = DefSpeed;   // 設定速度へ戻す

                }

                // 攻撃モーションを使用していなかったら
                if (isAttakc == false && isStrAttack == false)
                {
                    bAttack = false;
                }
                // ボタンが押されたら
                if(button == true)
                {
                    speed = 0f;
                    //Debug.Log("ボタン押されたよ！");
                    // 弱攻撃モーション
                    anime.SetTrigger("monsterWeakAttack");
                  
                }
            }

          

            switch(state)
            {
                // 召喚時
                case STATE.STATE_TRANSFORM:
                {
                        // 出現モーション
                        anime.SetTrigger("monsterTransform");

                        bool isTransform = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monster_Idle");
                        if (isTransform == true)
                        {
                            state = STATE.STATE_NORMAL;
                        }

                    break;
                }
                    // 通常状態
                case STATE.STATE_NORMAL:
                {
                        // ラブゲージ処理
                        timeLeft -= Time.deltaTime;
                        if (timeLeft <= 0)
                        {
                            // loveGauge -= loveGaugeLate;
                            loveGauge -= 10;
                            timeLeft = 1.0f;
                        }

                     // ラブゲージが尽きたら
                    if (loveGauge <= 0)
                    {
                            // 時間消滅へ
                            state = STATE.STATE_SPLIT;
                    }

                    // 体力処理
                    if (health <= 0)
                    {
                            // ダウン処理へ
                            state = STATE.STATE_KNOCKOUT;
                    }
                        // 移動処理
                        vector = TSVector.Normalize(vector);
                        directionVector = TSVector.Normalize(directionVector);

                        FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
                        tsTransform.rotation = TSQuaternion.Euler(0.0f, direction, 0.0f);

                        if (!(TSVector.Distance(TSVector.zero, tsTransform.position + vector) >= STAGE_LENGTH))
                            tsTransform.Translate(vector * speed, Space.World);

                    break;
                }  
                    // 攻撃状態
                case STATE.STATE_ATTACK:
                {
                        
                    break;
                }
                    // ダウン＆消滅
                case STATE.STATE_KNOCKOUT:
                {
                        //Debug.Log("モンスターダウン");
                        // 撃破モーション
                        anime.SetTrigger("monsterDown");

                        // リスポーンステートになったら
                        bool isDown = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monterOut");

                        if (isDown == true)
                        {
                            //Debug.Log("モンスターダウン切り替え");
                            // playerにチェンジ
                            SetChangePlayer();
                            gameObject.SetActive(false);
                            state = STATE.STATE_TRANSFORM;
                        }

                    break;
                }
                case STATE.STATE_SPLIT:
                {
                        //Debug.Log("モンスター時間消滅");
                        // 時間切れ
                        anime.SetTrigger("monsterSplit");

                        // リスポーンステートになったら
                        bool isSplit = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monterOut");

                        if (isSplit == true)
                        {
                            //Debug.Log("モンスター時間消滅切り替え");
                            SetChangePlayer();
                            gameObject.SetActive(false);
                            state = STATE.STATE_TRANSFORM;
                        }

                    break;
                }

                default:
                {
                    break;
                }
            }
            if(isAttakc)
            {
                //Debug.Log(("攻撃モーションだよ！"));
            }


        }
       
	
	}

    public void TransformInit(TSVector pos, TSQuaternion rot)
    {
        //Debug.Log("モンスターInit");
        tsTransform.position = new TSVector(pos.x, 3f, pos.z);;
        tsTransform.rotation = rot;

        knockout = false;
        transformFlag = false;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        controllerConnect = false;
        bAttack = false;
        loveGauge = loveGaugeMax;
        move = TSVector.zero;
        health = 100;
        //2017/12/6 追加
        state = STATE.STATE_TRANSFORM;

    }

    // 弱攻撃処理
    public void HitWeakAttack(GameObject hitObject, GameObject hitOffset)
    {
        //GameObject hit;
       // hit = Instantiate(hitObject, hitOffset.transform.position, transform.rotation) as GameObject;
        //hit.transform.parent = hitOffset.transform;  
        //hit.GetComponent<HitMonster>().pc = this;
      //  hit.gameObject.GetComponent<HitMonster>().pc = this;
    }

    public void OnSyncedCollisionEnter(TSCollision c)
    {
        //Debug.Log("monこりチェック");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "minion")
        {
            //Debug.Log("minionからDamage!");
           // health -= 1;
        }

        /*
        else if(other.gameObject.tag == "Player")
        {
            Debug.Log("PlayerからDamage!");
            health -= 10;
        }
        */
    }

    /*
    private void OnCollisionEnter(Collider other)
    {
        Debug.Log("damage!");
        if (other.gameObject.tag == "Player")
        {
            health -= 10;

        }
    }
*/
    private void SetChangePlayer()
    {
        GameObject Manager = transform.parent.gameObject;
        Manager.GetComponent<PlayManager>().SertActivePlayer();

        GameObject player = Manager.transform.Find("player" + owner.Id).gameObject;
        player.GetComponent<player>().TransformInit(tsTransform.position, tsTransform.rotation);

    }

    public void AddDamage(int damage)
    {
        
    }
}
