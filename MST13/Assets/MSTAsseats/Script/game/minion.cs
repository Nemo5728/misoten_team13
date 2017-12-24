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

    private enum TARGETTYPE
    {
        TYPE_NONE,
        TYPE_RANDOM,
        TYPE_DIST
    };

    STATE state;
    MOVESTATE moveState;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private float attackSpeed;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject bullet;

    [AddTracking]
    [SerializeField, TooltipAttribute("ヒットポイント")] private int health;
    [SerializeField, TooltipAttribute("攻撃範囲")] private float attackRange;
    [SerializeField, TooltipAttribute("索敵範囲")] private float searchRange;
    [SerializeField, TooltipAttribute("この値>索敵範囲>攻撃範囲になるように")] private float outRange;
    [SerializeField, TooltipAttribute("攻撃力")] private int attackValue;
    [SerializeField, TooltipAttribute("パワーアップ")] private int powerUpAttackValue;
    [SerializeField, TooltipAttribute("スピード")] private float speed = 10;
    [SerializeField, TooltipAttribute("リスポーン時間(sec)")] private float respawnTime;
    [SerializeField, TooltipAttribute("撤退完了距離")] private FP boidRange;
    [SerializeField, TooltipAttribute("巡航速度範囲"), Range(0, 5)] private float cruiseSpeedRange;
    [SerializeField, TooltipAttribute("ターゲットタイプ")] private TARGETTYPE targetType;

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

        isBullet = bullet;
        p = parentPlayer.GetComponent<player>();
    }

    public override void OnSyncedUpdate()
    {
       // Debug.Log("called minion update");
        TSVector markerPos = p.GetMarkerPosition(parentMarker);

        switch(state)
        {
            case STATE.STATE_NORMAL:
                {
                    minion targetMinion = null;
                    attack = false;
                    
                    if (targetType == TARGETTYPE.TYPE_NONE)
                    {
                        targetMinion = TargetSelectTypeNone(markerPos);
                    }
                    else if (targetType == TARGETTYPE.TYPE_DIST)
                    {
                        targetMinion = TargetSelectTypeDistance(markerPos);
                    }
                    else if (targetType == TARGETTYPE.TYPE_RANDOM)
                    {
                        targetMinion = TargetSelectTypeRandom(markerPos);
                    }
                    else
                    {
                        targetMinion = TargetSelectTypeNone(markerPos);
                    }

                    switch(moveState)
                    {
                        case MOVESTATE.MOVE_NORMAL:
                            {
                                FP cruiseSpeed = speed + Random.Range(-cruiseSpeedRange, cruiseSpeedRange); 
                                TSVector vector = markerPos - tsTransform.position;
                                FP dist = TSVector.Distance(markerPos, tsTransform.position) / cruiseSpeed;
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
                                    p = parentPlayer.GetComponent<player>();
                                    p.AddScoreNum(5);   // スコア追加
                                    if (isBullet != null)
                                    {
                                        GameObject go = Instantiate(isBullet, transform.position, Quaternion.identity);

                                        if (!p.GetPowerUp())
                                        {
                                            go.GetComponent<Bullet>().Create(targetMinion, ownerNum, attackValue);
                                        }
                                        else
                                        {
                                            go.GetComponent<Bullet>().Create(targetMinion, ownerNum, powerUpAttackValue);
                                        }

                                        go.transform.parent = targetMinion.transform;
                                    }
                                    else
                                    {
                                        if (!p.GetPowerUp())
                                        {
                                            targetMinion.AddDamage(attackValue);
                                        }
                                        else
                                        {
                                            targetMinion.AddDamage(powerUpAttackValue);
                                        }
                                    }
                                }

                                break;
                            }
                        case MOVESTATE.MOVE_OUT:
                            {
                                TSVector vector = markerPos - tsTransform.position;
                                FP dist = TSVector.Distance(markerPos, tsTransform.position) / speed;
                                vector = TSVector.Normalize(vector);

                                tsTransform.LookAt(tsTransform.position + vector);
                                tsTransform.Translate(vector * dist, Space.World);
                                
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

    private minion TargetSelectTypeNone(TSVector markerPos)
    {
        minion targetMinion = null;

        foreach (minion mi in FindObjectsOfType<minion>())
        {
            if (mi.GetOwner() != ownerNum && moveState != MOVESTATE.MOVE_OUT)
            {
                targetMinion = mi;
                if (TSMath.Abs(TSVector.Distance(tsTransform.position, markerPos)) > outRange)
                {
                    moveState = MOVESTATE.MOVE_OUT;
                }
                else if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < searchRange && moveState != MOVESTATE.MOVE_OUT)
                {
                    moveState = MOVESTATE.MOVE_SEARCE;
                }
                else
                {
                    moveState = MOVESTATE.MOVE_NORMAL;
                }

                if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < attackRange && moveState != MOVESTATE.MOVE_OUT)
                {
                    moveState = MOVESTATE.MOVE_ATTACK;
                }


                if (moveState != MOVESTATE.MOVE_NORMAL)
                {
                    break;
                }
            }
            else if (moveState == MOVESTATE.MOVE_OUT)
            {
                if (TSMath.Abs(TSVector.Distance(tsTransform.position, markerPos)) <= boidRange)
                {
                    moveState = MOVESTATE.MOVE_NORMAL;
                }
            }
        }

        //オフラインモード例外処理
        if (moveState == MOVESTATE.MOVE_NONE) moveState = MOVESTATE.MOVE_NORMAL;

        return targetMinion;
    }

    private minion TargetSelectTypeRandom(TSVector markerPos)
    {
        minion targetMinion = null;
        List<minion> minionList = new List<minion>();

        foreach (minion mi in FindObjectsOfType<minion>())
        {
            if (mi.GetOwner() != ownerNum && moveState != MOVESTATE.MOVE_OUT)
            {
                targetMinion = mi;
                if (TSMath.Abs(TSVector.Distance(tsTransform.position, markerPos)) > outRange)
                {
                    moveState = MOVESTATE.MOVE_OUT;
                }
                else if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < searchRange && moveState != MOVESTATE.MOVE_OUT)
                {
                    moveState = MOVESTATE.MOVE_SEARCE;
                    return targetMinion;
                }
                else
                {
                    moveState = MOVESTATE.MOVE_NORMAL;
                }

                if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < attackRange && moveState != MOVESTATE.MOVE_OUT)
                {
                    moveState = MOVESTATE.MOVE_ATTACK;

                    minionList.Add(mi);
                }


                if (moveState != MOVESTATE.MOVE_NORMAL || moveState != MOVESTATE.MOVE_ATTACK)
                {
                    break;
                }
            }
            else if (moveState == MOVESTATE.MOVE_OUT)
            {
                if (TSMath.Abs(TSVector.Distance(tsTransform.position, markerPos)) <= boidRange)
                {
                    moveState = MOVESTATE.MOVE_NORMAL;
                }
            }
        }

        //オフラインモード例外処理
        if (moveState == MOVESTATE.MOVE_NONE) moveState = MOVESTATE.MOVE_NORMAL;

        if (minionList.Count == 0)
        {
            return null;
        }
        else
        {
            return minionList[(int)Random.Range(0, minionList.Count)];
        }
    }

    private minion TargetSelectTypeDistance(TSVector markerPos)
    {
        minion targetMinion = null;
        FP targetDist = 0;

        foreach (minion mi in FindObjectsOfType<minion>())
        {
            if (mi.GetOwner() != ownerNum && moveState != MOVESTATE.MOVE_OUT)
            {
                if (TSMath.Abs(TSVector.Distance(tsTransform.position, markerPos)) > outRange)
                {
                    moveState = MOVESTATE.MOVE_OUT;
                }
                else if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < searchRange && moveState != MOVESTATE.MOVE_OUT)
                {
                    FP distBuff = TSVector.Distance(tsTransform.position, mi.tsTransform.position);
                    if (targetMinion == null || distBuff < targetDist)
                    {
                        targetDist = distBuff;
                        targetMinion = mi;
                    }

                    moveState = MOVESTATE.MOVE_SEARCE;
                }
                else
                {
                    moveState = MOVESTATE.MOVE_NORMAL;
                }

                if (TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position)) < attackRange && moveState != MOVESTATE.MOVE_OUT)
                {
                    FP distBuff = TSMath.Abs(TSVector.Distance(tsTransform.position, mi.tsTransform.position));
                    if (targetMinion == null || distBuff < targetDist)
                    {
                        targetDist = distBuff;
                        targetMinion = mi;
                    }

                    moveState = MOVESTATE.MOVE_ATTACK;
                }


                if (moveState != MOVESTATE.MOVE_OUT)
                {
                    break;
                }
            }
            else if (moveState == MOVESTATE.MOVE_OUT)
            {
                if (TSMath.Abs(TSVector.Distance(tsTransform.position, markerPos)) <= boidRange)
                {
                    moveState = MOVESTATE.MOVE_NORMAL;
                }
            }
        }

        //オフラインモード例外処理
        if (moveState == MOVESTATE.MOVE_NONE) moveState = MOVESTATE.MOVE_NORMAL;

        return targetMinion;
    }
}
