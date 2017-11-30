using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class PlayerManager : TrueSyncBehaviour {

    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] playerArray;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] monsterArray;
    [SerializeField, TooltipAttribute("触るな危険")] private GameObject[] signArray;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnSyncedStart(){
        GameObject createPlayer;
        GameObject createMonster;

        if(owner.Id != 0){
            createPlayer = TrueSyncManager.SyncedInstantiate(playerArray[owner.Id - 1], TSVector.zero, TSQuaternion.identity);
            createMonster = TrueSyncManager.SyncedInstantiate(monsterArray[owner.Id - 1], TSVector.zero, TSQuaternion.identity);
        }else{  //オフラインモードの例外処理
            createPlayer = TrueSyncManager.SyncedInstantiate(playerArray[owner.Id], TSVector.zero, TSQuaternion.identity);
            createMonster = TrueSyncManager.SyncedInstantiate(monsterArray[owner.Id], TSVector.zero, TSQuaternion.identity);
        }

        createPlayer.transform.parent = transform;
        createMonster.transform.parent = transform;
        createMonster.SetActive(false);
    }
}
