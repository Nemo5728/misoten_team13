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
    private TSVector move;

    [SerializeField, TooltipAttribute("攻撃速度(sec)")] private float attackSpeed;
    [SerializeField, TooltipAttribute("ヒットポイント")] private int health;
    [SerializeField, TooltipAttribute("移動速度")] private float speed;
    [SerializeField, TooltipAttribute("攻撃範囲")] private float range;
    [SerializeField, TooltipAttribute("攻撃力")] private int attack;
    [SerializeField, TooltipAttribute("リスポーン時間(sec)")] private float respawnTime;
    [SerializeField, TooltipAttribute("移動減衰係数"), Range(0.0f, 1.0f)] private float drag;

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
    }

    public override void OnSyncedUpdate(){
        player p = parentPlayer.GetComponent<player>();
        TSVector markerPos = p.GetMarkerPosition(parentMarker);

        TSVector vector = markerPos - tsTransform.position;
        vector = TSVector.Normalize(vector);

        tsTransform.Translate(vector * speed, Space.World);

        //move += vector * speed;
        //tsTransform.Translate(move, Space.World);
        //move.x += (0.0f - move.x) / drag;
        //move.z += (0.0f - move.z) / drag;

        if (coolTime <= 0)
        {
            foreach (minion mi in FindObjectsOfType<minion>())
            {
                Vector3 targetPos = mi.GetPositon();
                int targetOwner = mi.GetOwner();

                float distance = Vector3.Distance(targetPos, transform.position);
                if(distance < range && ownerNum != mi.GetOwner()){
                    mi.AddDamage(attack);
                    coolTime = attackSpeed;
                    break;
                }
            }
        }else{
            coolTime -= Time.deltaTime;
        }
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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
