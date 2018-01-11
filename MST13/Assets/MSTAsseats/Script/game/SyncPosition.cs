using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class SyncPosition : TrueSyncBehaviour {

    private GameObject imageTarget;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public override void OnSyncedStart(){
        imageTarget = GameObject.Find("ImageTarget");
    }

    public override void OnSyncedUpdate(){
        FP posY = imageTarget.GetComponent<TSTransform>().position.y;
        tsTransform.position = new TSVector(tsTransform.position.x, posY, tsTransform.position.z);
    }
}
