using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private const float STAGE_LENGTH = 56.0f;

    private TSRigidBody rb = null;
    private TSVector directionVector = TSVector.zero;
    private ControllerInfo info = null;
    private float[] minionRespawnCount = new float[15];
    private float playerRespawnCount = 0.0f;
    private int minionCount = 0;
    private int loveGauge;
    private float timeLeft = 1.0f;
    private float transformCount = 0.0f;
    private int powerUpButton = 0;
    private float powerUpCount = 0.0f;
    private bool powerUpFlag;
    private bool controllerConnect;
    private TSVector move;
    private GameObject signObject;
    private int knockback;

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
    [SerializeField, TooltipAttribute("移動速度")] private float speed;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] markerList;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject minion;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject monsterObject;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject sign;
    [SerializeField, TooltipAttribute("ラブゲージMAX")] private int loveGaugeMax = 100;
    [SerializeField, TooltipAttribute("ラブゲージ上昇率")] private int loveGaugeLate = 1;
    [SerializeField, TooltipAttribute("変身時HP")] private int health = 100;
    [SerializeField, TooltipAttribute("変身時間")] private float transformTime = 100;
    [SerializeField, TooltipAttribute("変身時攻撃力")] private int attack = 1;
    [SerializeField, TooltipAttribute("連打判定終了時間")] private float powerUpEndTime = 0.5f;
    [SerializeField, TooltipAttribute("パワーアップ開始回数")] private int powerUpStart = 10;
    [SerializeField, TooltipAttribute("スタミナ")] private int stamina = 10;
    [SerializeField, TooltipAttribute("ノックバック値")] private int knockBackMax = 100;
    [SerializeField, TooltipAttribute("ノックバック上昇値")] private int knockBackValue = 2;

    // test 
    private float settimer = 0;
    private float timer;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public override void OnSyncedStart(){
        WebAPIClient.send(2, 50);
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        controllerConnect = false;
        move = TSVector.zero;
        rb = GetComponent<TSRigidBody>();
        knockback = 0;
        state = STATE.STATE_AWAKE;

        signObject = TrueSyncManager.SyncedInstantiate(sign, new TSVector(0.0f, 5.0f, 0.0f), TSQuaternion.identity);
        signObject.transform.parent = transform;
        if(owner.Id != 0) signObject.GetComponent<MeshRenderer>().material.SetFloat("_Player", (float)owner.Id);
        else signObject.GetComponent<MeshRenderer>().material.SetFloat("_Player", 1);    //オフラインモード例外処理
    }

    public override void OnSyncedInput(){
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
        //info = BLEControlManager.GetControllerInfo();
        info = SerialControllManager.GetControllerInfo();
        if (info != null) controllerConnect = true;

        if(controllerConnect){
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
        
        timer += Time.deltaTime;

        if(timer >=  settimer)
        {
            switch(state){
                case STATE.STATE_AWAKE:
                    {
                        for (int i = 0; i < markerList.Length; i++)
                        {
                            TSVector vec;
                            vec.x = markerList[i].transform.position.x;
                            vec.y = markerList[i].transform.position.y + 1.0f;
                            vec.z = markerList[i].transform.position.z;
                            GameObject CreateMinion = TrueSyncManager.SyncedInstantiate(minion, vec, TSQuaternion.identity);
                            minion mi = CreateMinion.GetComponent<minion>();
                            mi.Create(gameObject, i, owner.Id);
                            minionCount++;
                        }

                        state = STATE.STATE_NORMAL;
                        break;
                    }
                case STATE.STATE_NORMAL:
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
                            int stickX = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
                            int stickY = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);
                            bool button = TrueSyncInput.GetBool(INPUT_CONTROLLER_BUTTON);
                            bool stickBtn = TrueSyncInput.GetBool(INPUT_CONTROLLER_STICKBUTTON);

                            directionVector.x = vector.x = speed * (stickX / 473);
                            directionVector.z = vector.z = speed * (stickY / 473);
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
                            tsTransform.Translate(vector * speed, Space.World);

                        //ノックバック処理
                        foreach (minion mi in FindObjectsOfType<minion>())
                        {
                            if (mi.GetAttack() && mi.GetOwner() == owner.Id)
                            {
                                knockback += knockBackValue;
                            }
                        }

                        if (knockBackValue >= knockBackMax)
                        {
                            TSVector knockbackVector;
                            knockbackVector.x = -directionVector.x;
                            knockbackVector.y = -directionVector.y;
                            knockbackVector.z = -directionVector.z;
                            rb.AddForce(knockbackVector, ForceMode.Impulse);
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

                                if (minionRespawnCount[i] <= 0)
                                {
                                    TSVector vec;
                                    vec.x = markerList[i].transform.position.x;
                                    vec.y = markerList[i].transform.position.y;
                                    vec.z = markerList[i].transform.position.z;
                                    GameObject CreateMinion = TrueSyncManager.SyncedInstantiate(minion, vec, TSQuaternion.identity);
                                    minion mi = CreateMinion.GetComponent<minion>();
                                    mi.Create(gameObject, i, owner.Id);
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
                            //変身
                            state = STATE.STATE_PREPARATION;
                            transformCount = transformTime;

                            monsterObject.SetActive(true);
                            GetComponent<monster>().TransformInit(tsTransform.position, tsTransform.rotation);
                            gameObject.SetActive(false);

                            foreach (minion mi in FindObjectsOfType<minion>())
                            {
                                if (owner.Id == mi.GetOwner())
                                {
                                    mi.Destroy();
                                }
                            }
                        }
                        break;
                    }
                case STATE.STATE_PREPARATION:
                    {
                        //変身前処理

                        break;
                    }
                case STATE.STATE_TRANSFORM:
                    {
                        transformCount -= Time.deltaTime;

                        if (transformTime <= 0)
                        {
                            state = STATE.STATE_NORMAL;
                        }
                        break;
                    }
                case STATE.STATE_KNOCKOUT:
                    {
                        playerRespawnCount -= Time.deltaTime;

                        if (playerRespawnCount <= 0)
                        {
                            state = STATE.STATE_NORMAL;

                            for (int i = 0; i < markerList.Length; i++)
                            {
                                TSVector vec;
                                vec.x = markerList[i].transform.position.x;
                                vec.y = markerList[i].transform.position.y;
                                vec.z = markerList[i].transform.position.z;
                                GameObject CreateMinion = TrueSyncManager.SyncedInstantiate(minion, vec, TSQuaternion.identity);
                                minion mi = CreateMinion.GetComponent<minion>();
                                mi.Create(gameObject, i, owner.Id);
                                minionCount++;

                                minionRespawnCount[i] = 0.0f;
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            };
        }

        signObject.GetComponent<MeshRenderer>().material.SetFloat("_barValue", (float)loveGauge / 100);
    }

    public TSVector GetMarkerPosition(int marker){
        TSVector vec;
        vec.x = markerList[marker].transform.position.x;
        vec.y = markerList[marker].transform.position.y;
        vec.z = markerList[marker].transform.position.z;
        return vec;
    }

    public void SetResporn(float time, int num){
        minionRespawnCount[num] = time;

        minionCount--;
        if(minionCount <= 0){
            state = STATE.STATE_KNOCKOUT;
            playerRespawnCount = respawnTime;
        }
    }

    public void TransformInit(TSVector pos, TSQuaternion rot){
        tsTransform.position = pos;
        tsTransform.rotation = rot;

        for (int i = 0; i < markerList.Length; i++)
        {
            TSVector vec;
            vec.x = markerList[i].transform.position.x;
            vec.y = markerList[i].transform.position.y;
            vec.z = markerList[i].transform.position.z;
            GameObject CreateMinion = TrueSyncManager.SyncedInstantiate(minion, vec, TSQuaternion.identity);
            minion mi = CreateMinion.GetComponent<minion>();
            mi.Create(gameObject, i, owner.Id);
            minionCount++;
        }

        state = STATE.STATE_NORMAL;
        powerUpCount = 0.0f;
        powerUpButton = 0;
        powerUpFlag = false;
        move = TSVector.zero;
        knockback = 0;
    }

    public float GetStageLength(){
        return STAGE_LENGTH;
    }

    public TSVector GetPosition(){
        return tsTransform.position;
    }
}
