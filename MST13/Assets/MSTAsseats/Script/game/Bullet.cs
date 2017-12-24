using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class Bullet : TrueSyncBehaviour {

    private minion target;
    private int shootOwner;
    private int damage;

    [SerializeField, TooltipAttribute("速度")] private FP speed;
    [SerializeField, TooltipAttribute("消える範囲")] private float range;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public override void OnSyncedUpdate(){
        transform.localScale = Vector3.one;
        TSVector vec = target.tsTransform.position - tsTransform.position;
        vec = TSVector.Normalize(vec);
        tsTransform.Translate(vec * speed, Space.World);

        if (TSMath.Abs(TSVector.Distance(tsTransform.position, target.tsTransform.position)) < range)
        {
            Destroy(gameObject);
            target.AddDamage(damage);
        }
    }

    public void Create(minion target, int shootOwner, int damage){
        this.target = target;
        this.shootOwner = shootOwner;
        this.damage = damage;
    }

    public int GetOwner(){
        return shootOwner;
    }
}
