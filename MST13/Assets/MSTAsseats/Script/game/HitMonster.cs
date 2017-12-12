using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class HitMonster : TrueSyncBehaviour {


    public monster monster;     // monsterクラス

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart()
    {
     
    }

 

    public void OnSyncedCollisionEnter(TSCollision c)
    {
       
        if (c.transform.tag == "minion")
        {
            // 弱攻撃
            if (monster.isAttakc == true)
            {
                // ミニオンにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(monster.WeakAttack);
            }
            // 
            else if (monster.isStrAttack == true)
            {
                // ミニオンにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(monster.StrAttack);
            }
            // minionにダメージ

            TrueSyncManager.SyncedDestroy(c.gameObject);
          //  Instantiate(damageEffect, transform.position, Quaternion.identity);
        }



    }

    public override void OnSyncedUpdate()
    {}
}
