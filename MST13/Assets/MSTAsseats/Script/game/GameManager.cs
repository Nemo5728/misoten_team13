using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class GameManager : TrueSyncBehaviour {

    public bool isGamePlay; // ゲーム管理
    public readyTime ready;
    public Text FinishText; // ゲーム終了テキスト

	// Use this for initialization
	void Start ()
    {
        

	}
	
	// Update is called once per frame
	void Update () 
    {  }
    public override void OnSyncedStart()
    {
        isGamePlay = false;
    }

    public override void OnSyncedUpdate()
    {
        if (ready.isTimerCount == true)
        {
            // ゲームプレイ開始
            isGamePlay = true;
        }

        // ゲームプレイ中
        if (isGamePlay == true)
        {
            // GameTimerが0になったら
            // isGamePlay = false
            // FinishText.text = "Finish!";

        }
    }
}
