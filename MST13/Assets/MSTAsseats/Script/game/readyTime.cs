using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class readyTime : TrueSyncBehaviour {

    public int MAX_TIME = 4; // カウントダウンの開始値

    [AddTracking]
    public float timeCounter = 4;
    public bool isTimerCount;

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
        if (timeCounter <= -1f)
        {
            GetComponent<UnityEngine.UI.Text>().text = " ";
            isTimerCount = true;    // タイマーカウント終了

        }

        if (isTimerCount == false)
        {
            timeCounter -= Time.deltaTime;

            // マイナス値にならないようにしている
            if (timeCounter <= 1f)
            {
                // ゲームスタートを描画
                GetComponent<UnityEngine.UI.Text>().text = "GameStart!";
            }

            else
            {
                GetComponent<UnityEngine.UI.Text>().text = ((int)timeCounter).ToString();
            }
        }

     }
}
