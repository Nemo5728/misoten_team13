using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class Bullet : TrueSyncBehaviour {

    private minion target;
    private float speed;
    private int shootOwner;

    [SerializeField, TooltipAttribute("消える範囲")] private float range;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public override void OnSyncedUpdate(){
        transform.LookAt(target.transform.position);
        transform.Translate(transform.forward * speed);

        if(Vector3.Distance(transform.position, target.transform.position) < range){
            TrueSyncManager.SyncedDestroy(gameObject);
        }
    }

    public void Create(minion target, int shootOwner){
        this.target = target;
        this.shootOwner = shootOwner;
    }

    public int GetOwner(){
        return shootOwner;
    }
}
