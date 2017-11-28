using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class HitMonster : TrueSyncBehaviour {

    public float time;
    public int damage;  // 与えるダメージ
    //public GameObject damageEffect;


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart()
    {
     
    }

 

    public void OnSyncedCollisionEnter(TSCollision c)
    {
        if (c.gameObject.name == "minion")
        {
            // minionにダメージ
            //  c.gameObject.GetComponent<minion>().AddDamage(damage);
            TrueSyncManager.SyncedDestroy(c.gameObject);
          //  Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

    }

    public override void OnSyncedUpdate()
    {}
}
