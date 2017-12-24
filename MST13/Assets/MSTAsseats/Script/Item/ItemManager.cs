using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class ItemManager : TrueSyncBehaviour {

    [SerializeField, TooltipAttribute("アイテムリスト")] private GameObject[] itemList;

    FP timer;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart()
    {
        timer = 0f;
    }

    public override void OnSyncedUpdate()
    {
        timer += TrueSyncManager.DeltaTime;
       

        if (timer >= 1f)
        {
            
            int num = TSRandom.Range(0, 3);
            TrueSyncManager.SyncedInstantiate(itemList[num], new TSVector(TSRandom.Range(-30f, 30f),
                                                                  -0.5f,
                                                                  TSRandom.Range(-30f, 30f)),
                                                                  TSQuaternion.identity);
            timer = 0f;
        }
    }
}
