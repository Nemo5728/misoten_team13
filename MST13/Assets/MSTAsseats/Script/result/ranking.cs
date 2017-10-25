using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ranking : MonoBehaviour {

    public int[] score;
    public GameObject players;

    private int[] pr_score = new int[4];
    private int[] pr_rank = new int[4];

    private float y = 0.0f;
    private int a = 0;

    // Use this for initialization
    void Start () {
        score.CopyTo(pr_score, 0);

        System.Array.Sort(pr_score);

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (pr_score[i] == score[j])
                {
                    pr_rank[j] = i;
                    break;
                }
            }
        }

        for (int i = 0; i < players.transform.childCount; i++)
        {
            y = pr_rank[i] * 0.75f;
            players.transform.GetChild(i).transform.Translate ( 0.0f, y, 0.0f );

            if (pr_rank[i] == 3)
            {
                a = i;
            }
        }

        Debug.Log(pr_score[0]);
        Debug.Log(pr_score[1]);
        Debug.Log(pr_score[2]);
        Debug.Log(pr_score[3]);
        Debug.Log(a);
    }

    // Update is called once per frame
    void Update () {
        players.transform.GetChild(a).transform.Rotate( 0.0f, 0.5f, 0.0f );
    }
}
