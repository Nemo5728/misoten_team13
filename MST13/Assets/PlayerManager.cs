using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class PlayerManager : TrueSyncBehaviour {

    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] playerArray;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] monsterArray;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] signArray;

    private GameObject createPlayer;
    private GameObject createMonster;

	// Use this for initialization
	void Start () 
    {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart(){
      

        if(owner.Id != 0)
        {
            Debug.Log("PlayerManager:オンラインモードなう");
            Debug.Log(owner.Id);
            createPlayer = TrueSyncManager.SyncedInstantiate(playerArray[owner.Id - 1], TSVector.zero, TSQuaternion.identity);
            createMonster = TrueSyncManager.SyncedInstantiate(monsterArray[owner.Id - 1], TSVector.zero, TSQuaternion.identity);
        }else{  //オフラインモードの例外処理
            Debug.Log("PlayerManager:オフラインモードなう");
            Debug.Log(owner.Id);
            createPlayer = TrueSyncManager.SyncedInstantiate(playerArray[owner.Id], TSVector.zero, TSQuaternion.identity);
            createMonster = TrueSyncManager.SyncedInstantiate(monsterArray[owner.Id], TSVector.zero, TSQuaternion.identity);
        }

        createPlayer.transform.parent = transform;
        createMonster.transform.parent = transform;
        createMonster.SetActive(false);
    }
    public override void OnSyncedInput()
    { 
        Debug.Log("PlayerManager:onSyncedInputなう");
       //  player child = GetComponentInChildren<player>();
       // child.GetComponent<player>().OnSyncedInput();
    }

    public override void OnSyncedUpdate()
    {
        
    }

    // 2017/12/6 追加
    public void SertActiveMonster()
    {
        createMonster.SetActive(true);
    }

    // 2017/12/6 追加
    public void SertActivePlayer()
    {
        createPlayer.SetActive(true);
    }
}
