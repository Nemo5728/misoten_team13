using System.Collections;
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

    // 2017/12/2 追加
    private Animator anim;  // アニメーター

    private enum STATE
    {
        STATE_NONE,
        STATE_NORMAL,
        STATE_TRANSFORM,
        STATE_DOWN
    };

    STATE state;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private float attackSpeed;

    [AddTracking]
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
        health = 10;
        GetComponent<TSSphereCollider>().radius = range;

        //this.tag = "minion" + (ownerNum + 1);
    }

    public override void OnSyncedStart()
    {
        
        rb = GetComponent<TSRigidBody>();

        // 2017/12/1 追加
        anim = GetComponent<Animator>();    // アニメーションの取得

        anim.SetTrigger("minionSpawn");        // 誕生アニメーション
        state = STATE.STATE_NORMAL;
    }

    public override void OnSyncedUpdate()
    {
       // Debug.Log("called minion update");
        player p = parentPlayer.GetComponent<player>();
        TSVector markerPos = p.GetMarkerPosition(parentMarker);

        switch(state)
        {
            case STATE.STATE_NORMAL:
                {
                    TSVector vector = markerPos - tsTransform.position;
                    FP dist = TSVector.Distance(markerPos, tsTransform.position) / speed;
                    vector = TSVector.Normalize(vector);

                    attack = false;

                    if (!(TSVector.Distance(TSVector.zero, (tsTransform.position + vector * dist)) >= p.GetStageLength()))
                        tsTransform.Translate(vector * dist, Space.World);
                    
                    if(coolTime <= 0)
                    {
                        foreach (minion mi in FindObjectsOfType<minion>())
                        {
                            if(Vector3.Distance(transform.position, mi.transform.position) < range && mi.GetOwner() != ownerNum)
                            {
                                anim.SetTrigger("minionWeakAttack");
                                mi.AddDamage(3);
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


                    if (health <= 0)
                    {
                        // downへ
                        state = STATE.STATE_DOWN;
                    }

                    break;
                }
            case STATE.STATE_TRANSFORM:
                {
                    // 2017/12/6 変更
                    tsTransform.rotation = TSQuaternion.Slerp(tsTransform.rotation,
                                                              TSQuaternion.LookRotation(p.GetPosition() - tsTransform.position),
                                                              0.1f);
                    
                    tsTransform.position += Time.deltaTime * tsTransform.forward * 1.5f +Time.deltaTime * tsTransform.up * 0.3f ;
                

                    break;
                }

            case STATE.STATE_DOWN:
                {
                    // 2017/12/2 追記
                    anim.SetTrigger("minionDown"); // ダウン

                    bool isRespon = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.minion_down");

                    // ダウンモーションが終了したら

                    TrueSyncManager.SyncedDestroy(gameObject);
                    Debug.Log("responする！!");

                    player pl = parentPlayer.GetComponent<player>();
                    pl.SetResporn(respawnTime, parentMarker);

                    if (isRespon == true)
                    {
                      
                       
                    }
                    break;
                }
            default:
                {
                    break;
                }
        };
    }

    public void AddDamage(int damage)
    {
        if (state != STATE.STATE_NORMAL) return;

        health -= damage;

        if(health <= 0)
        {
            // downへ
            state = STATE.STATE_DOWN;
        }else
        {
            Debug.Log("minionDamage!");
            // 2017/12/2 追記
            anim.SetTrigger("minionDamage"); // ダメージアニメーション
        }
    }

    public int GetOwner()
    {
        return ownerNum;
    }

    public Vector3 GetPositon(){
        return transform.position;
    }

    public void Destroy()
    {
        TrueSyncManager.SyncedDestroy(gameObject);
    }

    public bool GetAttack()
    {
        return attack;
    }

    public void SetTransform()
    {
        state = STATE.STATE_TRANSFORM;
    }
}
