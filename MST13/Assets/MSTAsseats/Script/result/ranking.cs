using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class ranking : MonoBehaviour
{

    public int[] score;
    public GameObject players;
    public GameObject texts;
    public GameObject pillers;
    public float HeightRate = 10.0f;
    public float DifPltoBar = 0.5f;

    private int[] pr_score = new int[4];
    private int[] pr_rank = new int[4];

    private int a = 0;
    private float[] targetY = new float[4];

    private GameObject[] pr_BarCol = new GameObject[4];
    private GameObject[] pr_BarTop = new GameObject[4];

    // Use this for initialization
    void Start()
    {
        TrueSyncManager.EndSimulation();

        Transform text;
        float y = 0.0f;
        float addy = 0.0f;
        int minscore = 999999999;
        int maxscore = 0;

        score.CopyTo(pr_score, 0);

        System.Array.Sort(pr_score);

        for (int i = 0; i < 4; i++)
        {
            addy += (float)pr_score[i];
            if (minscore > pr_score[i])
            {
                minscore = pr_score[i];
            }

            if (maxscore < pr_score[i])
            {
                maxscore = pr_score[i];
            }
        }

        if (maxscore / addy < 0.35f)
        {
            HeightRate *= 1.5f;
        }

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
            //プレイヤー達の高さの設定
            //y = ( 4 - pr_rank[i] ) * 0.75f;
            targetY[i] = (score[i] / addy) * HeightRate + 0.45f;
            y = (score[i] / addy) * HeightRate + 0.45f;
            // players.transform.GetChild(i).transform.Translate ( 0.0f, y, 0.0f );

            //ランキングのテキスト表記
            text = texts.transform.GetChild(i);
            text.transform.Translate(0.0f, (y + 2.5f), 0.0f);
            text.GetComponent<TextMesh>().text = "PC" + (i + 1) + ":" + pr_rank[i].ToString() + "位";
            text.transform.localScale = new Vector3(-1, 1, 1);
            text.GetComponent<MeshRenderer>().enabled = false;

            //柱の設定
            pr_BarCol[i] = pillers.transform.GetChild(i).transform.Find("rankingBar/rankingBar").gameObject;
            pr_BarTop[i] = pillers.transform.GetChild(i).transform.Find("rankingBar/root/top").gameObject;

            //もし1＝一位なら
            if (pr_rank[i] == 1)
            {
                //一位固有演出をするプレイヤーの設定
                a = i;

                text.GetComponent<TextMesh>().fontSize += 40;
            }
            else
            {
                text.GetComponent<TextMesh>().fontSize += 10;
                text.GetComponent<TextMesh>().color *= new Color(0.85f, 0.85f, 0.85f, 0.8f);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            texts.transform.GetChild(i).transform.LookAt(Camera.main.transform);

            if (texts.transform.GetChild(i).GetComponent<MeshRenderer>().enabled == false)
            {
                //プレイヤー、バーの上昇処理
                pr_BarTop[i].transform.position += new Vector3(0.0f, 0.035f, 0.0f);
                players.transform.GetChild(i).transform.position = pr_BarTop[i].transform.position + new Vector3(0.0f, DifPltoBar, 0.0f);

                //目標の高さ以上になったか
                if (pr_BarTop[i].transform.position.y >= targetY[i])
                {
                    //ランキングの表示
                    texts.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;

                    //高さの修正処理
                    Vector3 plPos = players.transform.GetChild(i).transform.position;
                    Vector3 baPos = pr_BarTop[i].transform.position;
                    plPos = new Vector3(plPos.x, targetY[i] + DifPltoBar, plPos.z);
                    baPos = new Vector3(baPos.x, targetY[i], baPos.z);

                    //到達時の発光処理
                    pr_BarCol[i].GetComponent<Renderer>().material.SetFloat("_glow", 0.45f);
                    pr_BarCol[i].GetComponent<Renderer>().material.SetFloat("_lighten", 0.60f);
                }
            }
            else
            {
                float f = 0.0f;
                f = pr_BarCol[i].GetComponent<Renderer>().material.GetFloat("_glow") * 0.90f;
                pr_BarCol[i].GetComponent<Renderer>().material.SetFloat("_glow", f);
                f = pr_BarCol[i].GetComponent<Renderer>().material.GetFloat("_lighten") * 0.90f;
                pr_BarCol[i].GetComponent<Renderer>().material.SetFloat("_lighten", f);
                if (i == a)
                {
                    players.transform.GetChild(a).transform.Rotate(0.0f, 0.5f, 0.0f);
                }
            }
        }
    }

    static public void SetScore(int PlayerScore, int PlayerNomber0to3)
    {

    }
}
