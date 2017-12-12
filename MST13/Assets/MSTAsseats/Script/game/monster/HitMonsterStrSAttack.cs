using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class HitMonsterStrSAttack : TrueSyncBehaviour {

    public monster monster;     // monsterクラス

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public override void OnSyncedStart() { }

    public override void OnSyncedUpdate() { }

    public void OnSyncedCollisionEnter(TSCollision c)
    {
        Debug.Log("Strチェック");

        if (c.gameObject.tag == "minion")
        {
            if (monster.isStrAttack == true)
            {
                // ミニオンにダメージ
                c.gameObject.GetComponent<minion>().AddDamage(monster.StrAttack);
            }

        }
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("当たったよ");

        if (col.gameObject.tag == "Player")
        {
            if (monster.isStrAttack == true)
            {
                Debug.Log("ダメージ");
            }

        }
    }

}
