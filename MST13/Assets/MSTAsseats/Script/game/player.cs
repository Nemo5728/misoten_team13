using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class player : TrueSyncBehaviour {

    private const byte INPUT_KEY_FORWARD = 0;
    private const byte INPUT_KEY_BACK = 1;
    private const byte INPUT_KEY_RIGHT = 2;
    private const byte INPUT_KEY_LEFT = 3;

    private TSRigidBody rb = null;
    private TSVector directionVector = TSVector.zero;
    private ControllerInfo info;
    private float[] respornCount = new float[15];

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private int attackSpeed = 0;
    [SerializeField, TooltipAttribute("移動速度")] private float speed;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] markerList;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject minion;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public override void OnSyncedStart(){
        for (int i = 0; i < markerList.Length; i ++){
            TSVector vec;
            vec.x = markerList[i].transform.position.x;
            vec.y = markerList[i].transform.position.y;
            vec.z = markerList[i].transform.position.z;
            GameObject CreateMinion =  TrueSyncManager.SyncedInstantiate(minion, vec, TSQuaternion.identity);
            minion mi = CreateMinion.GetComponent<minion>();
            mi.Create(gameObject, i, ownerIndex);
        }

        rb = GetComponent<TSRigidBody>();
    }

    public override void OnSyncedInput(){
        //キーボード デバッグ用
        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);

        TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
        TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
        TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
        TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);

        //BLEなんちゃら
        info = BLEControlManager.GetControllerInfo();
    }

    public override void OnSyncedUpdate(){
        bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
        bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
        bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
        bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);

        TSVector vector = TSVector.zero;
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

        TSVector.Normalize(vector);
        rb.AddForce(speed * vector, ForceMode.Force);

        FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, (float)direction, 0.0f);

        for (int i = 0; i < 15; i ++){
            if(respornCount[i] > 0){
                respornCount[i] -= Time.deltaTime;

                if(respornCount[i] <= 0){
                    TSVector vec;
                    vec.x = markerList[i].transform.position.x;
                    vec.y = markerList[i].transform.position.y;
                    vec.z = markerList[i].transform.position.z;
                    GameObject CreateMinion = TrueSyncManager.SyncedInstantiate(minion, vec, TSQuaternion.identity);
                    minion mi = CreateMinion.GetComponent<minion>();
                    mi.Create(gameObject, i, ownerIndex);
                }
            }
        }
    }

    public TSVector GetMarkerPosition(int marker){
        TSVector vec;
        vec.x = markerList[marker].transform.position.x;
        vec.y = markerList[marker].transform.position.y;
        vec.z = markerList[marker].transform.position.z;
        return vec;
    }

    public void SetResporn(float time, int num){
        respornCount[num] = time;
    }
}
