using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    public int[] rank;
    public int[] score;
    private string[] text = new string[4];

    // Use this for initialization
    void Start () {
        this.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update() {
        this.GetComponent<Text>().text = "";

        for (int i = 0; i < 4; i++)
        {
            text[i] = "PC" + (i + 1) + ":" + rank[i].ToString() + "位 ";
        }

        for (int i = 0; i < 4; i++)
        {
            this.GetComponent<Text>().text += text[i];
        }
    }
}
