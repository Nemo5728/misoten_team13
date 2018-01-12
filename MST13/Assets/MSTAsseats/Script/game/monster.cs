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

    private GameObject particle;           // particle
    private GameObject particleMnsDamage;
    private GameObject ParticleSwing;
    private GameObject ParticleTransPulse;
    private GameObject ParticleTransOff;

    private Animator anime;             //アニメーター
    private TSVector move;
    private GameObject ready;           // readyオブジェクト
    private GameObject ManagerScore;
    private GameObject imageTarget;
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
    [SerializeField, TooltipAttribute("移動速度")] private FP speed;
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

    private FP DefSpeed;          // 設定速度を保管

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
        particle = GameObject.Find("Particle");
        particleMnsDamage = GameObject.Find("particleMnsDamage");
        ParticleSwing = GameObject.Find("ParticleSwing");
        ParticleTransPulse = GameObject.Find("ParticleTransPulse");
        ParticleTransOff = GameObject.Find("ParticleTransOff");
        imageTarget = GameObject.Find("ImageTarget");

        ManagerScore = transform.parent.gameObject;
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
      //  GetComponent<ParticleManager>().Play("FX_Trans_PulseP" + owner.Id, transform.position);
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
        info = BLEControlManager.GetControllerInfo();
       // info = SerialControllManager.GetControllerInfo();
        if (info != null) controllerConnect = true;

        if (controllerConnect)
        {
            TrueSyncInput.SetInt(INPUT_CONTROLLER_STICKX, info.stickX);
            TrueSyncInput.SetInt(INPUT_CONTROLLER_STICKY, info.stickY);
            TrueSyncInput.SetBool(INPUT_CONTROLLER_BUTTON, info.isButtonDown);
            TrueSyncInput.SetBool(INPUT_CONTROLLER_STICKBUTTON, info.isStickDown);
        }
	}

	// 更新
	public override void OnSyncedUpdate()
	{
      //  if (ready != null) return;

        if (!knockout)
        {
            // 攻撃アニメーション情報を取得
            //    isAttakc    = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monsterWeakAttack");
            //  isStrAttack = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monsterStrAttack");
            isAttakc = false;
            isStrAttack = false;

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

            int stickX = -1, stickY = -1;
            bool button = false, stickBtn = false;

            if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX) != -1) stickX = TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
            if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY) != -1) stickY = TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);
            if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX) != -1) button = TrueSyncInput.GetBool(INPUT_CONTROLLER_BUTTON);
            if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY) != -1) stickBtn = TrueSyncInput.GetBool(INPUT_CONTROLLER_STICKBUTTON);

            // スティック傾けているかチェック*部品ごとの誤差対策
            if(stickX >=  -1 || stickX <= 1)
            {
                if (stickX >= 700)
                {
                    directionVector.x += vector.x += speed;
                }

                if (stickX <= 200)
                {
                    directionVector.x += vector.x += -speed;
                }

                if (stickY >= 700)
                {
                    directionVector.z += vector.z += speed;
                }

                if (stickY <= 200)
                {
                    directionVector.z += vector.z += -speed;
                }
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
                    SeManager.Instance.Play("monsterStrAttack");
                    ParticleSwing.GetComponent<ParticleManager>().Play("FX_SwingB",
                                                          new Vector3(transform.position.x,
                                                                      transform.position.y + 2f,
                                                                      transform.position.z));


                     foreach (minion mi in FindObjectsOfType<minion>())
                    {
                        if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < 30f)
                        {
                            Debug.Log("ダメージだぜ");
                            mi.AddDamage(5);
                        }
                    }
                  
                }
            }
            ///// その他のモーション /////
            else
            {
                // スティックを押したら
            if (button || weakAttack)
                {
                    if (bAttack) return;    // 
                    //Debug.Log("スティックボタン押されたよ！");

                    bAttack = true;

                    // 弱攻撃モーション
                    anime.SetTrigger("monsterWeakAttack");
                    SeManager.Instance.Play("monsterWeakAttack");
                    foreach (minion mi in FindObjectsOfType<minion>())
                    {
                        if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < 30f)
                        {
                            mi.AddDamage(3);
                        }
                    }

                }
                speed = DefSpeed;   // 設定速度へ戻す
            }

            // 攻撃モーションを使用していなかったら
            if (isAttakc == false && isStrAttack == false)
            {
                bAttack = false;
            }
            else
            {
                ///// 攻撃モーション中は移動不可にしたい /////
                if (isAttakc)
                {
                    speed = 0f; // 移動速度を0に

                    // スティックを押したら
                    if ( weakAttack)
                    {
                        if (isStrAttack) return;
                        // 弱攻撃モーション
                        anime.SetTrigger("monsterStrAttack");
                        SeManager.Instance.Play("monsterStrAttack");
                        ParticleSwing.GetComponent<ParticleManager>().Play("FX_SwingB",
                                                              new Vector3(transform.position.x,
                                                                          transform.position.y + 2f,
                                                                          transform.position.z));

                         foreach (minion mi in FindObjectsOfType<minion>())
                        {
                            if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < 30f)
                            {
                            //   Debug.Log("ダメージだぜ");
                                mi.AddDamage(3);
                            }
                        }
                    }
                }
                ///// その他のモーション /////
                else
                {
                    // スティックを押したら
                    if (weakAttack)
                    {
                        if (bAttack) return;    // 
                        //Debug.Log("スティックボタン押されたよ！");

                        bAttack = true;

                        // 弱攻撃モーション
                        anime.SetTrigger("monsterWeakAttack");
                        SeManager.Instance.Play("monsterWeakAttack");

          

                        foreach (minion mi in FindObjectsOfType<minion>())
                        {
                            if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < 30f)
                            {
                            //    Debug.Log("ダメージだぜ!!!!!!!");
                                mi.AddDamage(1);
                            }
                        }
                        //HitWeakAttack(hitWeakObject, hitWeakOffset);

                    }
                    speed = DefSpeed;   // 設定速度へ戻す

                }

                // 攻撃モーションを使用していなかったら
                if (isAttakc == false && isStrAttack == false)
                {
                    bAttack = false;
                }
            }
          

            switch(state)
            {
                // 召喚時
                case STATE.STATE_TRANSFORM:
                {
                
                        // 出現モーション
                        //anime.SetTrigger("monsterTransform");
                     //   SeManager.Instance.Play("monsterrespon");
                      
                        
                        bool isTransform = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.monster_Idle");
                        if (isTransform == true)
                        {
                            ParticleTransPulse.GetComponent<ParticleManager>().Play("FX_Trans_PulseP" + owner.Id,
                                                            new Vector3(transform.position.x,
                                                                        transform.position.y + 2f,
                                                                        transform.position.z));
                            Debug.Log("アイドルへ");
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

                    if (!(TSMath.Abs(TSVector.Distance(TSVector.zero, tsTransform.position + vector)) >= STAGE_LENGTH))
                        tsTransform.Translate((vector * speed) * TrueSyncManager.DeltaTime, Space.World);

                    break;
                }  
                    // 攻撃状態
                case STATE.STATE_ATTACK:
                {
                        ParticleSwing.GetComponent<ParticleManager>().Play("FX_SwingA",
                                                            new Vector3(transform.position.x,
                                                                        transform.position.y + 2f,
                                                                        transform.position.z));
                        state = STATE.STATE_NORMAL;
                    break;
                }
                    // ダウン＆消滅
                case STATE.STATE_KNOCKOUT:
                {
                        //Debug.Log("モンスターダウン");
                        // 撃破モーション
                        anime.SetTrigger("monsterDown");
                        SeManager.Instance.Play("monsterdown");
                        ParticleTransOff.GetComponent<ParticleManager>().Play("FX_TransOff_PulseP" + owner.Id,
                                                              new Vector3(transform.position.x,
                                                                          transform.position.y + 2f,
                                                                          transform.position.z));

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
                        SeManager.Instance.Play("monsterdown");
                        particle.GetComponent<ParticleManager>().Play("FX_TransOff_PulseP" + owner.Id.ToString(),
                                                              new Vector3(transform.position.x,
                                                                          transform.position.y + 2f,
                                                                          transform.position.z));
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

    public void OnSyncedCollisionEnter(TSCollision c)
    {

        // 被ダメ
        if (c.gameObject.tag == "minion")
        {
            particleMnsDamage.GetComponent<ParticleManager>().Play("FX_MonsterDamageP" + owner.Id,
                                                              new Vector3(transform.position.x,
                                                                          transform.position.y + 2f,
                                                                          transform.position.z));
            AddDamage(1);
        }
    }

    private void SetChangePlayer()
    {
        GameObject Manager = transform.parent.gameObject;
        Manager.GetComponent<PlayManager>().SertActivePlayer();

        GameObject player = Manager.transform.Find("player" + owner.Id).gameObject;
        player.GetComponent<player>().TransformInit(tsTransform.position, tsTransform.rotation);

    }

    public void AddDamage(int damage)
    {
        health -= damage;
    }

    public void AddScoreNum(int score)
    {
        ManagerScore.GetComponent<PlayManager>().AddScore(score);
    }
}
