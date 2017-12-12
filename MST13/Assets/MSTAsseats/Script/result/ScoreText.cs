using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public ranking rank;
    public int player0to3;
    private Text date;
    private int a;

    // Use this for initialization
    void Start () {
        date = GetComponent<Text>();
        date.text = "";
        date.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update() {
        date.text = "";
        a = rank.GetRank();
        player0to3 = rank.GetPlayerNumber();
        date.color += new Color(0.0f, 0.0f, 0.0f, 0.01f);
        
        date.text = "Thank You For Playing!!\n";
        if (a == player0to3)
        {
            date.text += "You are Number 1!";
        }
    }
}
