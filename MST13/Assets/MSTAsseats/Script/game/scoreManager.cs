using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class scoreManager : TrueSyncBehaviour {

    public int[] TestScore = { 0, 0, 0, 0 };
    public int[] pr_score = new int[4];
    public int[] score = new int[4];
    int lenght;
    int hiscore;
    int playerNum;
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart()
    {
        hiscore = 0;
        lenght = 0;
        playerNum = 1;
        foreach (PlayManager play in FindObjectsOfType<PlayManager>())
        {
            lenght++;
        }

        TestScore.CopyTo(score, 0);

        score.CopyTo(pr_score, 0);

        System.Array.Sort(pr_score);
    }

    public override void OnSyncedUpdate()
    {
        int cnt = 0;
        foreach (PlayManager play in FindObjectsOfType<PlayManager>())
        {
            // playScoreをソートする
            score[cnt] = play.playScore;
            cnt++;
        }

        //ランキングソート
        for (int i = 0; i < cnt; i++)
        {
            pr_score[i] = score[i];
           // Debug.Log("pr_score" + pr_score[i]);

        }

        for (int i = 0; i < cnt; i++)
        {
            hiscore = pr_score[i];
            for (int j = 0; j < cnt; j++)
            {
                if(hiscore < pr_score[j])
                {
                    hiscore = pr_score[j];
                    playerNum = j;
                }
            }
        }

       
 
        WebAPIClient.send(playerNum, hiscore);
    }
}
