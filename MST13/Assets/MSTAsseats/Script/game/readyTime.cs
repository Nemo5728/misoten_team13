﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class readyTime : TrueSyncBehaviour {

    public int MAX_TIME = 4; // カウントダウンの開始値
    [AddTracking]
    private float timeCounter = 4;

	// Use this for initialization
	void Start () {}

    void Update() {}
	
    public override void OnSyncedStart()
    {
        GetComponent<UnityEngine.UI.Text>().text = MAX_TIME.ToString();
    }

	// Update is called once per frame
    public override void OnSyncedUpdate()
    {
        timeCounter -= Time.deltaTime;

        if (timeCounter <= 0f)
        {
            timeCounter = 0f;
            // Canvasごと削除する
            // Destroy(gameObject);
           // TrueSyncManager.Destroy(transform.parent.gameObject);
            //TrueSyncManager.SyncedDestroy(transform.parent.gameObject);
            GetComponent<UnityEngine.UI.Text>().text = " ";

        }


        // マイナス値にならないようにしている
        else if (timeCounter <= 1f)
        {
            // ゲームスタートを描画
            GetComponent<UnityEngine.UI.Text>().text = "GameStart!";
        }

        else
        {
            GetComponent<UnityEngine.UI.Text>().text = ((int)timeCounter).ToString();
        }

           

/*
        timeCounter -= Time.deltaTime;

        if (timeCounter <= -1f)
        {
            // Canvasごと削除する
            Destroy(transform.parent.gameObject);
        }
        // マイナス値にならないようにしている
        else if (timeCounter <= 1f)
        {
            // ゲームスタートを描画
            GetComponent<UnityEngine.UI.Text>().text = "GameStart!";
        }
        else
        {
            GetComponent<UnityEngine.UI.Text>().text = ((int)timeCounter).ToString();
        }
*/

	}
}
