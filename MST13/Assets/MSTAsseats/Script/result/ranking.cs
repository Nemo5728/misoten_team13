using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;
using UnityEngine.SceneManagement;

public class ranking : MonoBehaviour
{

    //  public int[] TestScore = { 10, 20, 15, 40 };
    public GameObject scoreManager;
    public GameObject players;
    public GameObject texts;
    public GameObject pillers;
    public float HeightRate = 10.0f;
    public float DifPltoBar = 0.5f;

    public static int[] score = new int[4];
    public static int playerNumber = 0;
    private  Color[] color = { new Color (1.0f, 0.0f, 0.0f, 1.0f),
                                    new Color (0.0f, 0.0f, 1.0f, 1.0f),
                                    new Color (0.0f, 1.0f, 0.0f, 1.0f),
                                    new Color (1.0f, 1.0f, 0.0f, 1.0f) };

    public GameObject[] monster;
    public RuntimeAnimatorController monsAnim;

    private int[] pr_score = new int[4];
    private int[] pr_rank = new int[4];

    private int a = 0;
    private float[] targetY = new float[4];

    private GameObject[] pr_BarCol = new GameObject[4];
    private GameObject[] pr_BarTop = new GameObject[4];
    private GameObject pr_Monster;

    // Use this for initialization
    void Start()
    {
        TrueSyncManager.EndSimulation();

        Transform text;
        float addy = 0.0f;
        int minscore = 9999999;
        int maxscore = 0;
        scoreManager = GameObject.Find("ScoreManager");
        scoreManager.GetComponent<scoreManager>().pr_score.CopyTo(score, 0);
        //通信なしでの動作確認用

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
            if (minscore / (float)maxscore > 0.80f)
            {
                float set = (score[i] / addy) * (score[i] / addy) * 2.5f;
                targetY[i] = set * HeightRate + 0.45f;
            }
            else
            {
                targetY[i] = (score[i] / addy) * HeightRate + 0.45f;
            }


            //ランキングのテキスト表記
            text = texts.transform.GetChild(i);
            text.transform.Translate(0.0f, (targetY[i] + 2.5f), 0.0f);
            text.transform.localScale = new Vector3(-1, 1, 1);
            text.GetComponent<MeshRenderer>().enabled = false;
            text.GetComponent<Renderer>().material.SetFloat("_barValue", (float)( 5 - pr_rank[i] ) * 0.25f);

            //柱の設定
            pr_BarCol[i] = pillers.transform.GetChild(i).transform.Find("rankingBar/rankingBar").gameObject;
            pr_BarTop[i] = pillers.transform.GetChild(i).transform.Find("rankingBar/root/top").gameObject;

            //もし1＝一位なら
            if (pr_rank[i] == 1)
            {
                //一位固有演出をするプレイヤーの設定
                a = i;
                text.GetComponent<Renderer>().material.SetFloat("_Crown", 1.0f);
         
            }
        }
        GetComponent<ParticleManager>().Play("FX_ResultFireworkP1", transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < 4; i++)
        {
            texts.transform.GetChild(i).transform.LookAt(Camera.main.transform);
            texts.transform.GetChild(i).transform.Rotate( 0.0f, 180.0f, 0.0f );

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

                    //一位か否かでモーション分岐
                    Animator anim = players.transform.GetChild(i).GetComponent<Animator>();
                    if (a == i)
                    {
                        anim.SetTrigger("win");

                        //モンスターの召喚
                        pr_Monster = Instantiate(monster[a], new Vector3(0.0f, 0.0f, 0.0f), new Quaternion (0.0f, 180.0f, 0.0f, 0.0f) );
                        pr_Monster.GetComponent<Animator>().runtimeAnimatorController = monsAnim;
                    }
                    else
                    {
                        anim.SetTrigger("lose");
                    }
                }
            }
            else
            {
                //光の消灯化処理
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

        //1位発表後操作でタイトルに移る。
        if (texts.transform.GetChild(a).GetComponent<MeshRenderer>().enabled == true)
        {
            if (Input.GetKey("a"))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    //各プレイヤーのスコアを受け取る。
    public static void SetScore(int PlayerScore, int PlayerNomber0to3)
    {
        score[PlayerNomber0to3] = PlayerScore;
    }

    //この端末のプレイヤーナンバーを受け取る。
    public static void SetPlayerNumber(int PlayerNomber0to3)
    {
        playerNumber = PlayerNomber0to3;
    }

    //この端末のプレイヤーナンバーを送信する。
    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    //一位のプレイヤーのナンバーを送信する。
    public int GetRank()
    {
        return a;
    }
}
