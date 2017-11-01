using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class ranking : MonoBehaviour {

    public int[] score;
    public GameObject players;
    public GameObject texts;

    private int[] pr_score = new int[4];
    private int[] pr_rank = new int[4];

    private float y = 0.0f;
    private int a = 0;

    // Use this for initialization
    void Start () {
        TrueSyncManager.EndSimulation();

        score.CopyTo(pr_score, 0);

        System.Array.Sort(pr_score);

        //ランキングソート
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (pr_score[i] == score[j])
                {
                    pr_rank[j] = 4 - i;
                    break;
                }
            }
        }


        for (int i = 0; i < players.transform.childCount; i++)
        {
            Transform text;

            //プレイヤー達の高さの設定
            y = ( 4 - pr_rank[i] ) * 0.75f;
            players.transform.GetChild(i).transform.Translate ( 0.0f, y, 0.0f );

            //ランキングのテキスト表記
            text = texts.transform.GetChild(i);
            text.transform.Translate(0.0f, (y+2.5f), 0.0f);
            text.GetComponent<TextMesh>().text = "PC" + (i + 1) + ":" + pr_rank[i].ToString() + "位";
            text.transform.localScale = new Vector3 ( -1, 1, 1 );

            //もし1＝一位なら
            if (pr_rank[i] == 1)
            {
                //一位固有演出をするプレイヤーの設定
                a = i;

                text.GetComponent<TextMesh>().fontSize = 30;
            }
            else
            {
                text.GetComponent<TextMesh>().fontSize = 20;
                text.GetComponent<TextMesh>().color *= new Color ( 0.5f, 0.5f, 0.5f, 0.8f );
            }
        }

    }

    // Update is called once per frame
    void Update () {
        players.transform.GetChild(a).transform.Rotate( 0.0f, 0.5f, 0.0f );
        for (int i = 0; i < 4; i++)
        {
            texts.transform.GetChild(i).transform.LookAt( Camera.main.transform );
        }
    }
}
