using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class HitMonster : TrueSyncBehaviour {

    public float time;
    public int damage;  // 与えるダメージ
    public GameObject monsterPare;


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart()
    {
        monsterPare = transform.parent.gameObject;
    }

 

    public void OnSyncedCollisionEnter(TSCollision c)
    {
        // 弱攻撃中なら
        if(monsterPare.GetComponent<monster>().isAttakc)
        {
            if (c.gameObject.tag == "minion")
            {
                // minionにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(5);
                //  Instantiate(damageEffect, transform.position, Quaternion.identity);
            }
            else if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<monster>().AddDamage(5);
            }
        }
        // 強攻撃中なら
        else if (monsterPare.GetComponent<monster>().isStrAttack)
        {
            if (c.gameObject.tag == "minion" )
            {
                // minionにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(10);
                //  Instantiate(damageEffect, transform.position, Quaternion.identity);
            }
            else if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<monster>().AddDamage(10);
            }
        }
    }

    public void OnCollisionEnter(Collision c)
    {
        // 弱攻撃中なら
        if (monsterPare.GetComponent<monster>().isAttakc)
        {
            if (c.gameObject.tag == "minion")
            {
                // minionにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(5);
                //  Instantiate(damageEffect, transform.position, Quaternion.identity);
            }
            else if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<monster>().AddDamage(5);
            }
        }
        // 強攻撃中なら
        else if (monsterPare.GetComponent<monster>().isStrAttack)
        {
            if (c.gameObject.tag == "minion")
            {
                // minionにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(10);
                //  Instantiate(damageEffect, transform.position, Quaternion.identity);
            }
            else if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<monster>().AddDamage(10);
            }
        }
    }

    public override void OnSyncedUpdate()
    {}
}
