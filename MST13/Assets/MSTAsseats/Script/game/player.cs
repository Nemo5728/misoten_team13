using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class player : TrueSyncBehaviour {

    private const byte INPUT_KEY_FORWARD = 0;
    private const byte INPUT_KEY_BACK = 1;
    private const byte INPUT_KEY_RIGHT = 2;
    private const byte INPUT_KEY_LEFT = 3;

    public float speed;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        /*
        TSRigidBody rb = GetComponent<TSRigidBody>();
        TSVector vec = TSVector.zero, rot = TSVector.zero;

        if (Input.GetKey(KeyCode.W)){
            vec += TSVector.forward;
        }
        if (Input.GetKey(KeyCode.S)){
            vec += TSVector.back;
        }
        if (Input.GetKey(KeyCode.A)){
            vec += TSVector.left;
        }
        if (Input.GetKey(KeyCode.D)){
            vec += TSVector.right;
        }

        TSVector.Normalize(vec);
        rot.y = TSMath.Atan(vec.z / vec.x) * TSMath.Rad2Deg;
        Debug.Log("rot:" + rot.y);
        rb.AddForce(speed * vec, ForceMode.Acceleration);
        */
	}

    public override void OnSyncedInput(){
        /*
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        bool space = Input.GetKey(KeyCode.Space);
        */

        bool forward = Input.GetKey(KeyCode.W);
        bool back = Input.GetKey(KeyCode.S);
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);

        /*
        TrueSyncInput.SetInt(INPUT_KEY_HORIZONTAL, (int)(hor * 100));
        TrueSyncInput.SetInt(INPUT_KEY_VERTICAL, (int)(ver * 100));
        TrueSyncInput.SetBool(INPUT_KEY_CREATE, space);
        */

        TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
        TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
        TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
        TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);
    }

    public override void OnSyncedUpdate(){
        /*
        FP hor = (FP)TrueSyncInput.GetInt(INPUT_KEY_HORIZONTAL) / 100;
        FP ver = (FP)TrueSyncInput.GetInt(INPUT_KEY_VERTICAL) / 100;
        bool currentCreateState = TrueSyncInput.GetBool(INPUT_KEY_CREATE);

        TSVector forceToApply = TSVector.zero;

        if (FP.Abs(hor) > FP.Zero)
        {
            forceToApply.x = hor / 3;
        }

        if (FP.Abs(ver) > FP.Zero)
        {
            forceToApply.z = ver / 3;
        }

        tsRigidBody.AddForce(forceToApply, ForceMode.Impulse);
        */

        bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
        bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
        bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
        bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);

        TSRigidBody rb = GetComponent<TSRigidBody>();
        TSVector vec = TSVector.zero;

        if (forward)
        {
            vec += TSVector.forward;
            Debug.Log("hoge");
        }
        if (back)
        {
            vec += TSVector.back;
        }
        if (left)
        {
            vec += TSVector.left;
        }
        if (right)
        {
            vec += TSVector.right;
        }

        TSVector.Normalize(vec);
        rb.AddForce(speed * vec, ForceMode.Impulse);
    }
}
