﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class minion : TrueSyncBehaviour {

    private GameObject parentPlayer = null;
    private int parentMarker;
    private TSRigidBody rb = null;
    private int ownerNum;
    private float coolTime;
    private bool attack;

    private enum STATE
    {
        STATE_NONE,
        STATE_NORMAL,
        STATE_TRANSFORM
    };
    private STATE state;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private float attackSpeed;
    [SerializeField, TooltipAttribute("ヒットポイント")] private int health;
    [SerializeField, TooltipAttribute("攻撃範囲")] private float range;
    [SerializeField, TooltipAttribute("攻撃力")] private int attackValue;
    [SerializeField, TooltipAttribute("スピード")] private float speed = 10;
    [SerializeField, TooltipAttribute("リスポーン時間(sec)")] private float respawnTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Create(GameObject player, int marker, int owner){
        parentPlayer = player;
        parentMarker = marker;
        ownerNum = owner;
    }

    public override void OnSyncedStart(){
        rb = GetComponent<TSRigidBody>();
        state = STATE.STATE_NORMAL;
    }

    public override void OnSyncedUpdate(){
        Debug.Log("called minion update");
        player p = parentPlayer.GetComponent<player>();
        TSVector markerPos = p.GetMarkerPosition(parentMarker);

        switch(state){
            case STATE.STATE_NORMAL:
                {
                    TSVector vector = markerPos - tsTransform.position;
                    FP dist = TSVector.Distance(markerPos, tsTransform.position) / speed;
                    vector = TSVector.Normalize(vector);

                    attack = false;

                    if (!(TSVector.Distance(TSVector.zero, (tsTransform.position + vector * dist)) >= p.GetStageLength()))
                        tsTransform.Translate(vector * dist, Space.World);

                    if (coolTime <= 0)
                    {
                        foreach (minion mi in FindObjectsOfType<minion>())
                        {
                            Vector3 targetPos = mi.GetPositon();
                            int targetOwner = mi.GetOwner();

                            float distance = Vector3.Distance(targetPos, transform.position);
                            if (distance < range && ownerNum != mi.GetOwner())
                            {
                                mi.AddDamage(attackValue);
                                coolTime = attackSpeed;
                                attack = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        coolTime -= Time.deltaTime;
                    }

                    break;
                }
            case STATE.STATE_TRANSFORM:
                {
                    FP dist = TSVector.Distance(p.GetPosition(), tsTransform.position) / speed;
                    tsTransform.Translate(p.GetPosition() * dist, Space.World);
                    break;
                }
            default:
                {
                    break;
                }
        };
    }

    public void AddDamage(int damage){
        health -= damage;

        if(health <= 0){
            player p = parentPlayer.GetComponent<player>();
            p.SetResporn(respawnTime, parentMarker);

            Debug.Log("じゃあな");
            TrueSyncManager.SyncedDestroy(gameObject);
        }
    }

    public int GetOwner(){
        return ownerNum;
    }

    public Vector3 GetPositon(){
        return transform.position;
    }

    public void Destroy(){
        TrueSyncManager.SyncedDestroy(gameObject);
    }

    public bool GetAttack(){
        return attack;
    }

    public void SetTransform(){
        state = STATE.STATE_TRANSFORM;
    }
}
