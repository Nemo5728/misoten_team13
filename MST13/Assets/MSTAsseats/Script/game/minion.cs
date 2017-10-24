using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class minion : TrueSyncBehaviour {

    private GameObject parentPlayer = null;
    private int parentMarker;
    private TSRigidBody rb = null;

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Create(GameObject player, int marker){
        parentPlayer = player;
        parentMarker = marker;

        if (parentPlayer == null) Debug.Log("Createでnull");
    }

    public override void OnSyncedStart(){
        if (parentPlayer == null) Debug.Log("OnsyncedStartでnull");
        rb = GetComponent<TSRigidBody>();
    }

    public override void OnSyncedUpdate(){
        if (parentPlayer == null) Debug.Log("OnsyncedUpdateでnull");
        player p = parentPlayer.GetComponent<player>();
        TSVector markerPos = p.GetMarkerPosition(parentMarker);

        TSVector pos;
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = transform.position.z;

        TSVector vector = markerPos - pos;
        TSVector.Normalize(vector);

        rb.AddForce(vector * speed, ForceMode.Force);
    }
}
