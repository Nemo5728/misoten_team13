using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class Bullet : TrueSyncBehaviour {

    private minion targetMinion = null;
    private monster targetMonster = null;
    private int shootOwner;
    private int damage;

    [SerializeField, TooltipAttribute("速度")] private FP speed;
    [SerializeField, TooltipAttribute("消える範囲")] private FP range;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public override void OnSyncedUpdate(){
        transform.localScale = Vector3.one;
        TSVector vec = TSVector.zero;

        if(targetMinion != null){
            vec = targetMinion.tsTransform.position - tsTransform.position;
            vec = TSVector.Normalize(vec);
            tsTransform.Translate(vec * speed, Space.World);

            if (TSMath.Abs(TSVector.Distance(tsTransform.position, targetMinion.tsTransform.position)) < range)
            {
                Destroy(gameObject);
                TrueSyncManager.SyncedDestroy(gameObject);
                targetMinion.AddDamage(damage);
            }
        }

        if (targetMonster != null){
            vec = targetMonster.tsTransform.position - tsTransform.position;
            vec = TSVector.Normalize(vec);
            tsTransform.Translate(vec * speed, Space.World);

            if (TSMath.Abs(TSVector.Distance(tsTransform.position, targetMonster.tsTransform.position)) < range)
            {
                TrueSyncManager.SyncedDestroy(gameObject);
                targetMonster.AddDamage(damage);
            }
        }

    }

    public void CreateBulletMinion(minion target, int shootOwner, int damage){
        this.targetMinion = target;
        this.shootOwner = shootOwner;
        this.damage = damage;
    }

    public void CreateBulletMonster(monster target, int shootOwner, int damage){
        this.targetMonster = target;
        this.shootOwner = shootOwner;
        this.damage = damage;
    }

    public int GetOwner(){
        return shootOwner;
    }
}
