using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class minion : TrueSyncBehaviour {

    private GameObject parentPlayer = null;
    private GameObject isBullet = null;
    private int parentMarker;
    private TSRigidBody rb = null;
    private int ownerNum;
    [AddTracking] private FP coolTime;
    private bool attack;
    private player p;
    private FP range = 1.0f;

    // 2017/12/2 追加
    private Animator anim;  // アニメーター

    private enum STATE
    {
        STATE_NONE,
        STATE_NORMAL,
        STATE_TRANSFORM,
        STATE_DOWN
    };

    private enum MOVESTATE
    {
        MOVE_NONE,
        MOVE_NORMAL,
        MOVE_SEARCE,
        MOVE_ATTACK,
        MOVE_OUT
    };

    STATE state;
    MOVESTATE moveState;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private float attackSpeed;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject bullet;
    [SerializeField, TooltipAttribute("ヒットポイント")] private int health;
    [SerializeField, TooltipAttribute("攻撃範囲")] private float attackRange;
    [SerializeField, TooltipAttribute("索敵範囲")] private float searchRange;
    [SerializeField, TooltipAttribute("この値>索敵範囲>攻撃範囲になるように")] private float outRange;
    [SerializeField, TooltipAttribute("攻撃力")] private int attackValue;
    [SerializeField, TooltipAttribute("パワーアップ")] private int powerUpAttackValue;
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

        //this.tag = "minion" + (ownerNum + 1);
    }

    public override void OnSyncedStart()
    {
        
        rb = GetComponent<TSRigidBody>();

        // 2017/12/1 追加
        anim = GetComponent<Animator>();    // アニメーションの取得

        anim.SetTrigger("minionSpawn");        // 誕生アニメーション
        state = STATE.STATE_NORMAL;

        p = parentPlayer.GetComponent<player>();
    }

    public override void OnSyncedUpdate()
    {
       // Debug.Log("called minion update");
        TSVector markerPos = p.GetMarkerPosition(parentMarker);
        moveState = MOVESTATE.MOVE_NORMAL;

        switch(state)
        {
            case STATE.STATE_NORMAL:
                {
                    minion targetMinion = null;

                    attack = false;
                    
                    foreach (minion mi in FindObjectsOfType<minion>())
                    {
                        if(mi.GetOwner() != ownerNum){
                            if (Vector3.Distance(transform.position, new Vector3((float)markerPos.x, (float)markerPos.y, (float)markerPos.z)) > outRange){
                                moveState = MOVESTATE.MOVE_OUT;
                                break;
                            }
                            else if (Vector3.Distance(transform.position, mi.transform.position) < searchRange){
                                moveState = MOVESTATE.MOVE_SEARCE;
                                break;
                            }

                            if(Vector3.Distance(transform.position, mi.transform.position) < attackRange){
                                moveState = MOVESTATE.MOVE_ATTACK;
                                break;
                            }

                            targetMinion = mi;
                        }
                    }

                    switch(moveState)
                    {
                        case MOVESTATE.MOVE_NORMAL:
                            {
                                TSVector vector = markerPos - tsTransform.position;
                                FP dist = TSVector.Distance(markerPos, tsTransform.position) / speed;
                                vector = TSVector.Normalize(vector);

                                if (!(TSVector.Distance(TSVector.zero, (tsTransform.position + vector * dist)) >= p.GetStageLength())
                                    && !(TSVector.Distance(markerPos, tsTransform.position) < range)){
                                    tsTransform.LookAt(tsTransform.position + vector);
                                    tsTransform.Translate(vector * dist, Space.World);
                                }

                                break;
                            }
                        case MOVESTATE.MOVE_SEARCE:
                            {
                                TSVector vector = targetMinion.tsTransform.position - tsTransform.position;
                                FP dist = TSVector.Distance(targetMinion.tsTransform.position, tsTransform.position) / speed;
                                vector = TSVector.Normalize(vector);

                                tsTransform.LookAt(vector);
                                tsTransform.Translate(vector * dist, Space.World);
                                
                                break;
                            }
                        case MOVESTATE.MOVE_ATTACK:
                            {
                                if (coolTime <= 0)
                                {
                                    TSVector vector = targetMinion.tsTransform.position - tsTransform.position;
                                    vector = TSVector.Normalize(vector);

                                    tsTransform.LookAt(vector);

                                    anim.SetTrigger("minionWeakAttack");
                                    coolTime = attackSpeed;
                                    attack = true;

                                    if (!p.GetPowerUp())
                                    {
                                        targetMinion.AddDamage(attackValue);
                                    }
                                    else
                                    {
                                        targetMinion.AddDamage(powerUpAttackValue);
                                    }

                                    if (isBullet != null)
                                    {
                                        GameObject go = TrueSyncManager.SyncedInstantiate(isBullet, transform.position, Quaternion.identity);
                                        go.GetComponent<Bullet>().Create(targetMinion, ownerNum);
                                    }
                                }

                                break;
                            }
                        case MOVESTATE.MOVE_OUT:
                            {
                                if(targetMinion != null){
                                    TSVector vector = targetMinion.tsTransform.position - tsTransform.position;
                                    FP dist = TSVector.Distance(markerPos, tsTransform.position) / speed;
                                    vector = TSVector.Normalize(vector);

                                    tsTransform.LookAt(tsTransform.position + vector);
                                    tsTransform.Translate(vector * dist, Space.World);
                                }
                                
                                break;
                            }
                        default:
                            {


                                break;
                            }
                    }

                    if (health <= 0)
                    {
                        // downへ
                        state = STATE.STATE_DOWN;
                    }

                    coolTime -= TrueSyncManager.DeltaTime;

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
                    //Debug.Log("responする！!");

                    p.SetResporn(respawnTime, parentMarker);

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
            //Debug.Log("minionDamage!");
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
