using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class player : TrueSyncBehaviour {

    private const byte INPUT_KEY_FORWARD = 0;
    private const byte INPUT_KEY_BACK = 1;
    private const byte INPUT_KEY_RIGHT = 2;
    private const byte INPUT_KEY_LEFT = 3;
    private const byte INPUT_KEY_SPACE = 4;
    private const byte INPUT_CONTROLLER_STICKX = 5;
    private const byte INPUT_CONTROLLER_STICKY = 6;
    private const byte INPUT_CONTROLLER_BUTTON = 7;
    private const byte INPUT_CONTROLLER_STICKBUTTON = 8;
    private const byte INPUT_KEY_Y = 9;

    private const float STAGE_LENGTH = 56.0f;
    private const byte INPUT_TAP = 10;
    private const byte INOPUT_MOUSE = 11;
    private const int SHOOTER_VALUE = 3;

    private TSRigidBody rb = null;
    private TSVector directionVector = TSVector.zero;
    private ControllerInfo info = null;

    // パーティクル
    private GameObject particle;
    private GameObject particleItemGet;

    private float[] minionRespawnCount = new float[8];

    private float playerRespawnCount = 10.0f;
    private int minionCount = 0;
    private int loveGauge;

    private FP timeLeft = 1.0f;
    private FP transformCount = 0.0f;
    private int powerUpButton = 0;
    private float powerUpCount = 0.0f;
    private bool powerUpFlag;
    private TSVector move;
    private GameObject signObject;
    private bool controllerConnect;
    private int knockback;
    public GameObject ManagerScore;
    // 2017/12/1 追加
    private Animator anim;  // アニメーター
    FP time;

    private enum STATE
    {
        STATE_NONE,
        STATE_AWAKE,
        STATE_NORMAL,
        STATE_PREPARATION,
        STATE_TRANSFORM,
        STATE_KNOCKOUT
    };
    private STATE state;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private int attackSpeed = 0;
    [SerializeField, TooltipAttribute("復帰時間(sec)")] private float respawnTime = 0;
    [SerializeField, TooltipAttribute("移動速度")] private FP speed;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] markerList;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject minionDog;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject minionShooter;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject sign;
    [SerializeField, TooltipAttribute("ラブゲージMAX")] private int loveGaugeMax = 100;
    [SerializeField, TooltipAttribute("ラブゲージ上昇率")] private int loveGaugeLate = 50;
    [SerializeField, TooltipAttribute("変身時HP")] private int health = 100;
    [SerializeField, TooltipAttribute("変身時間")] private float transformTime = 5;
    [SerializeField, TooltipAttribute("変身時攻撃力")] private int attack = 1;
    [SerializeField, TooltipAttribute("連打判定終了時間")] private float powerUpEndTime = 0.5f;
    [SerializeField, TooltipAttribute("パワーアップ開始回数")] private int powerUpStart = 10;
    [SerializeField, TooltipAttribute("スタミナ")] private int stamina = 10;
    [SerializeField, TooltipAttribute("ノックバック値")] private int knockBackMax = 100;
    [SerializeField, TooltipAttribute("ノックバック上昇値")] private int knockBackValue = 2;
    [SerializeField, TooltipAttribute("ノックバックパワー")] private float knockBackPower = 100;

    // Use this for initialization
    void Start () {}
    
    // Update is called once per frame
    void Update () {}

    public override void OnSyncedStart()
    {
        particle = GameObject.Find("ParticleBanner");
        particleItemGet = GameObject.Find("ParticleItemGet");

        ManagerScore = transform.parent.gameObject;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        move = TSVector.zero;
        controllerConnect = false;
        rb = GetComponent<TSRigidBody>();
        knockback = 0;
        TSVector signPos = new TSVector(tsTransform.position.x, tsTransform.position.y + 13f, tsTransform.position.z);
        signObject = TrueSyncManager.SyncedInstantiate(sign, signPos, TSQuaternion.identity);
        signObject.transform.parent = transform;
        if(owner.Id != 0) signObject.GetComponent<MeshRenderer>().material.SetFloat("_Player", (float)owner.Id);
        else signObject.GetComponent<MeshRenderer>().material.SetFloat("_Player", 1);    //オフラインモード例外処理


        // 2017/12/1 追加
        anim = GetComponent<Animator>();    // アニメーションの取得
        state = STATE.STATE_AWAKE;
    }

    public override void OnSyncedInput()
    {
        //Debug.Log("TrueSyncInputなう");
        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool space = Input.GetKeyDown(KeyCode.Space);
        bool kib_y = Input.GetKey(KeyCode.Y);
        int touch = Input.touchCount;

        TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
        TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
        TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
        TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);
        TrueSyncInput.SetBool(INPUT_KEY_SPACE, space);
        TrueSyncInput.SetInt(INPUT_TAP,touch);
        TrueSyncInput.SetBool(INPUT_KEY_Y, kib_y);

        info = BLEControlManager.GetControllerInfo();

        if (info != null) controllerConnect = true;

        if(controllerConnect)
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

        //time += TrueSyncManager.DeltaTime;
        time += Time.deltaTime;

        if (time <= 3) return;
        switch(state)
        {
            case STATE.STATE_AWAKE:
                if(gameObject.activeSelf == true)
                {
                    for (int i = 0; i < markerList.Length; i++)
                    {
                        TSVector vec;
                        vec.x = markerList[i].transform.position.x;
                        vec.y = markerList[i].transform.position.y;
                        vec.z = markerList[i].transform.position.z;

                        GameObject createMinion;

                        if (i > SHOOTER_VALUE) createMinion = TrueSyncManager.SyncedInstantiate(minionDog, vec, TSQuaternion.identity);
                        else createMinion = TrueSyncManager.SyncedInstantiate(minionShooter, vec, TSQuaternion.identity);

                        minion mi = createMinion.GetComponent<minion>();
                        mi.Create(gameObject, i, owner.Id);
                        minionCount++;
                    }
                    SeManager.Instance.Play("minionRespon");
                    state = STATE.STATE_NORMAL;
                }

                break;
            case STATE.STATE_NORMAL:
                bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
                bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
                bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
                bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);
                bool space = TrueSyncInput.GetBool(INPUT_KEY_SPACE);
                bool mouse = TrueSyncInput.GetBool(INOPUT_MOUSE);
                bool kib_y = TrueSyncInput.GetBool(INPUT_KEY_Y);
                int Tc = TrueSyncInput.GetInt(INPUT_TAP);

                TSVector vector = TSVector.zero;
                if (forward) directionVector += vector += TSVector.forward;
                if (back) directionVector += vector += TSVector.back;
                if (left) directionVector += vector += TSVector.left;
                if (right) directionVector += vector += TSVector.right;

                if(kib_y){
                    rb.AddForce(tsTransform.forward * -knockBackPower, ForceMode.Impulse);
                }

                if(controllerConnect)
                {
                    int stickX = -1, stickY = -1;
                    bool button = false, stickBtn = false;

                    if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX) != -1) stickX = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
                    if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY) != -1) stickY = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);
                    if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX) != -1) button = TrueSyncInput.GetBool(INPUT_CONTROLLER_BUTTON);
                    if (TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY) != -1) stickBtn = TrueSyncInput.GetBool(INPUT_CONTROLLER_STICKBUTTON);

                    // 2017/12/1 追記
                    // Playerの移動モーション管理
                    MoveAnimetion(stickX);

                    if (stickX != -1 && stickY != -1)
                    {
                        directionVector.x = vector.x = speed * (stickX / 473);
                        directionVector.z = vector.z = speed * (stickY / 473);
                    }

                    if (stickBtn && stickX != -1 && stickY != -1)
                    {
                        knockBackValue++;
                        loveGauge++;
                        // minion強攻撃へ！
                    }
                }
             

                if(Tc > 0)
                {
                    
                }

                if (kib_y)
                {
                     knockBackValue++;
                     loveGauge++;

                    // minion強攻撃へ！
                }
                   

                 if (space)
                 {
                     powerUpButton++;
                     powerUpCount = 0.0f;
                 }
                 else
                 {
                     powerUpCount += Time.deltaTime;
                 
                     if (powerUpCount >= powerUpEndTime)
                     {
                         powerUpCount = 0.0f;
                         powerUpButton = 0;
                     }
                 }

                 if (powerUpButton > powerUpStart)
                 {
                      powerUpFlag = true;
                 }
                 else
                 {
                     powerUpFlag = false;
                 }

               
                 vector = TSVector.Normalize(vector);
                 directionVector = TSVector.Normalize(directionVector);
                 FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
                 tsTransform.rotation = TSQuaternion.Euler(0.0f, direction, 0.0f);

                if (!(TSVector.Distance(TSVector.zero, tsTransform.position + vector) >= STAGE_LENGTH))
                    tsTransform.Translate((vector * speed)* TrueSyncManager.DeltaTime, Space.World);


                //ノックバック処理
                foreach (minion mi in FindObjectsOfType<minion>())
                {
                    if (mi.GetAttack() && mi.GetOwner() == owner.Id)
                    {
                        knockback += knockBackValue;
                    }
                }

                if (knockback >= knockBackMax)
                {
                    particle.GetComponent<ParticleManager>().Play("FX_HitEffect_back", transform.position);
                    rb.AddForce(tsTransform.forward * -knockBackPower, ForceMode.Impulse);
                }

                if (TSVector.Distance(TSVector.zero, tsTransform.position + rb.velocity) >= STAGE_LENGTH)
                {
                    rb.velocity = TSVector.zero;
                }

                //ミニオンリスポーン処理
                for (int i = 0; i < markerList.Length; i++)
                {
                    if (minionRespawnCount[i] > 0)
                    {
                        minionRespawnCount[i] -= Time.deltaTime;

                        if (minionRespawnCount[i] < 0)
                        {
                            TSVector vec;
                            vec.x = markerList[i].transform.position.x;
                            vec.y = markerList[i].transform.position.y;
                            vec.z = markerList[i].transform.position.z;

                            GameObject createMinion;
                            if (i > SHOOTER_VALUE) createMinion = TrueSyncManager.SyncedInstantiate(minionDog, vec, TSQuaternion.identity);
                            else createMinion = TrueSyncManager.SyncedInstantiate(minionShooter, vec, TSQuaternion.identity);
                            minion mi = createMinion.GetComponent<minion>();
                            mi.Create(gameObject, i, owner.Id);
                            minionCount++;
                            
                        }
                    }
                }

                //ラブゲージ処理
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    loveGauge += loveGaugeLate;
                    timeLeft = 1.0f;
                }

                if (loveGauge >= loveGaugeMax)
                {
                //transformCount = transformTime;
                    AddScoreNum(100);
                     transformCount = 2f;
                    state = STATE.STATE_PREPARATION;
                }

       
                break;
            case STATE.STATE_PREPARATION:
                //変身前処理
                // 2017/12/1 追記
                anim.SetTrigger("bannerTransform");
                 // SeManager.Instance.Play("playerrespon");
                 // 2017/12/6 追記
                foreach (minion mi in FindObjectsOfType<minion>())
                {
                    if (owner.Id == mi.GetOwner())
                    {
                        mi.SetTransform();
                    }
                     
                }
        
                particle.GetComponent<ParticleManager>().Play("FX_BannerTransP" + owner.Id.ToString(), 
                                                              new Vector3(transform.position.x, 
                                                                          transform.position.y + 2f,
                                                                          transform.position.z));
                state = STATE.STATE_TRANSFORM;
                
                break;
            case STATE.STATE_TRANSFORM:
                transformCount -= Time.deltaTime;
                tsTransform.position += (tsTransform.up * (Time.deltaTime * 1f));
                if (transformCount <= 0f)
                    {  Debug.Log("変身や！");
                         // 2017/12/6 追加
                         // 親オブジェクトを参照してmonsterを生成する
                         GameObject Manager = transform.parent.gameObject;
                         Manager.GetComponent<PlayManager>().SertActiveMonster();

                         //Debug.Log("monster" + owner.Id);
                         GameObject monster = Manager.transform.Find("monster" + owner.Id).gameObject;
                         monster.GetComponent<monster>().TransformInit(tsTransform.position, tsTransform.rotation);
                        
                        //GetComponent<monster>().TransformInit(tsTransform.position, tsTransform.rotation);
                        //Debug.Log("変化");
                        gameObject.SetActive(false);

                        foreach (minion mi in FindObjectsOfType<minion>())
                        {
                            if (owner.Id == mi.GetOwner())
                            {
                                mi.Destroy();
                            }
                        }
                         state = STATE.STATE_NONE;
                    }

                    break;
            case STATE.STATE_KNOCKOUT:
                //2017/12/2 追記
                anim.SetTrigger("bannerSit");

                playerRespawnCount -= Time.deltaTime;

                if (playerRespawnCount <= 0)
                {
                    //2017/12/2 追記
                    anim.SetTrigger("bannerRise");
                    for (int i = 0; i < markerList.Length; i++)
                    {
                        TSVector vec;
                        vec.x = markerList[i].transform.position.x;
                        vec.y = markerList[i].transform.position.y;
                        vec.z = markerList[i].transform.position.z;

                        GameObject createMinion;

                        if (i > SHOOTER_VALUE) createMinion = TrueSyncManager.SyncedInstantiate(minionDog, vec, TSQuaternion.identity);
                        else createMinion = TrueSyncManager.SyncedInstantiate(minionShooter, vec, TSQuaternion.identity);

                        minion mi = createMinion.GetComponent<minion>();
                        mi.Create(gameObject, i, owner.Id);
                        minionCount++;

                        minionRespawnCount[i] = 0.0f;
                    }

                    playerRespawnCount = 10f;
                    state = STATE.STATE_NORMAL;
                }
                break;
            default:
                break;
        };
        

        signObject.GetComponent<MeshRenderer>().material.SetFloat("_barValue", (float)loveGauge / 100);
    }

    public TSVector GetMarkerPosition(int marker)
    {
        TSVector vec;
        vec.x = markerList[marker].transform.position.x;
        vec.y = markerList[marker].transform.position.y;
        vec.z = markerList[marker].transform.position.z;
        return vec;
    }

    public void SetResporn(float time, int num)
    {
        minionRespawnCount[num] = time;

        minionCount--;
        if(minionCount <= 0)
        {
            //2017/12/2 追記
            anim.SetTrigger("bannerDown");
            SeManager.Instance.Play("playerdown");
           
            state = STATE.STATE_KNOCKOUT;
            playerRespawnCount = respawnTime;
        }
    }


    public void TransformInit(TSVector pos, TSQuaternion rot)
    {
        tsTransform.position = new TSVector(pos.x, 0f,pos.z);
        tsTransform.rotation = rot;

       
        //2017/12/6 追加
        transformCount = 0.0f;
        loveGauge = 0;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        move = TSVector.zero;
        knockback = 0;
        state = STATE.STATE_KNOCKOUT;   // ダウン状態へ
       

    }

    public void AllDeleteMinion()
    {
           foreach (minion mi in FindObjectsOfType<minion>())
            {
                if (owner.Id == mi.GetOwner())
                {
                    mi.Destroy();
                }
            }
    }
    public float GetStageLength()
    {
        return STAGE_LENGTH;
    }

    public TSVector GetPosition()
    {
        return tsTransform.position;
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

    public bool GetPowerUp(){
        return powerUpFlag;
    }
   
    public void OnSyncedCollisionEnter(TSCollision col)
    {
        
        if (col.gameObject.tag == "ItemLoveUp")
        {
          
            particleItemGet.GetComponent<ParticleManager>().Play("FX_itemGot_Gauge", new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z));
            loveGauge += 10;
            AddScoreNum(10);

        }
        else if (col.gameObject.tag == "ItemMiniUp")
        {
            particleItemGet.GetComponent<ParticleManager>().Play("FX_itemGot_Respawn", new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z));
            SetItemResponMinion();
            AddScoreNum(10);
        }
        else if (col.gameObject.tag == "ItemSpeed")
        {
            particleItemGet.GetComponent<ParticleManager>().Play("FX_itemGot_Speed", new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z));
         
            speed += 5f;
            AddScoreNum(10);
           // particle.GetComponent<ParticleManager>().Play("FX_SpeedUp", new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z));
        }
        else if (col.gameObject.tag == "ItemPower")
        {
            particleItemGet.GetComponent<ParticleManager>().Play("FX_itemGot_Power", new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z));
            speed += 3f;
            AddScoreNum(10);
        }
    }
    // アイテムを手に入れたら
    void SetItemResponMinion()
    {
        int ResponCount = 0;
        //ミニオンリスポーン処理
        for (int i = 0; i < markerList.Length; i++)
        {
            if (ResponCount >= 5) continue;
            if (minionRespawnCount[i] > 0)
            {
                minionRespawnCount[i] -= Time.deltaTime;

                if (minionRespawnCount[i] < 0)
                {
                    TSVector vec;
                    vec.x = markerList[i].transform.position.x;
                    vec.y = markerList[i].transform.position.y;
                    vec.z = markerList[i].transform.position.z;

                    GameObject createMinion;
                    if (i > SHOOTER_VALUE) createMinion = TrueSyncManager.SyncedInstantiate(minionDog, vec, TSQuaternion.identity);
                    else createMinion = TrueSyncManager.SyncedInstantiate(minionShooter, vec, TSQuaternion.identity);
                    minion mi = createMinion.GetComponent<minion>();
                    mi.Create(gameObject, i, owner.Id);
                    minionCount++;
                    ResponCount++;
                   
                }
            }
        }
    }

    public void AddScoreNum(int score)
    {
        ManagerScore.GetComponent<PlayManager>().AddScore(score);
    }

}
