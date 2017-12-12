using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class HitMonsterWeakAttack : TrueSyncBehaviour {

    public monster monster;     // monsterクラス

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart(){}

    public override void OnSyncedUpdate(){}
   
    public void OnCollisionEnter(Collision c)
    {
        Debug.Log("コリジョン(normal)");

        monster mons = monster.GetComponent<monster>();

        if (c.gameObject.tag == "minion")
        {
            Debug.Log("minionアタック");
          
            // 弱攻撃
            if (mons.isAttakc == true)
            {
                // ミニオンにダメージ
                Debug.Log("minionダメージ");
                c.gameObject.GetComponent<minion>().AddDamage(monster.WeakAttack);
            }
        }
        else if (c.gameObject.tag == "Player")
        {
            // 弱攻撃
            if (mons.isAttakc == true)
            {
                // ミニオンにダメージ
                Debug.Log("Playerダメージ");
                c.gameObject.GetComponent<monster>().AddDamage(monster.WeakAttack);
            }
        }
    }

    
}
