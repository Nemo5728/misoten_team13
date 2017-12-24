using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class PlayManager : TrueSyncBehaviour {

    private GameObject PlayerObject;
    private GameObject monsterObject;
    public int playScore;
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public override void OnSyncedStart()
    {
        playScore = 0;
        if (owner.Id != 0)
        {
            Debug.Log("PlayManager:オンラインモードなう");
            Debug.Log(owner.Id);


            for (int i = 0; i < 4; i++)
            {
                if ( (owner.Id - 1) != i )
                {
                    PlayerObject = transform.Find("player" + (i + 1) ).gameObject;
                    PlayerObject.GetComponent<player>().AllDeleteMinion();
                    PlayerObject.SetActive(false);

                    monsterObject = transform.Find("monster" + (i + 1)).gameObject;
                    monsterObject.SetActive(false);
                } 
                else
                {
                    monsterObject = transform.Find("monster" + owner.Id).gameObject;
                    monsterObject.SetActive(false);
                }
            }


        }
        else
        {  //オフラインモードの例外処理
          //  Debug.Log("PlayManager:オフラインモードなう");
           // Debug.Log(owner.Id);

            monsterObject = transform.Find("monster1").gameObject;
            monsterObject.SetActive(false);

            PlayerObject = transform.Find("player2").gameObject;
            monsterObject = transform.Find("monster2").gameObject;

            PlayerObject.SetActive(false);
            monsterObject.SetActive(false);

            PlayerObject = transform.Find("player3").gameObject;
            monsterObject = transform.Find("monster3").gameObject;

            PlayerObject.SetActive(false);
            monsterObject.SetActive(false);

            PlayerObject = transform.Find("player4").gameObject;
            monsterObject = transform.Find("monster4").gameObject;

            PlayerObject.SetActive(false);
            monsterObject.SetActive(false);
        }
    }

    public override void OnSyncedUpdate()
    {
        Debug.Log("PlayerScore" +owner.Id);
        Debug.Log(playScore);
    }

    // 2017/12/6 追加
    public void SertActiveMonster()
    {
        
        monsterObject = transform.Find("monster" + owner.Id).gameObject;
        monsterObject.SetActive(true);
    }

    // 2017/12/6 追加
    public void SertActivePlayer()
    {
        PlayerObject = transform.Find("player" + owner.Id).gameObject;
        PlayerObject.SetActive(true);
    }

    public void AddScore(int score)
    {
        playScore += score;
    }
}
