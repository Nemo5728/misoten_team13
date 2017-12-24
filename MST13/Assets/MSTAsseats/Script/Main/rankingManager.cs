using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rankingManager : MonoBehaviour {

    public scoreManager score;
    public int[] pr_score = new int[4];
	// Use this for initialization
	void Start () {

       

	}
	
	// Update is called once per frame
	void Update ()
    {

        for (int i = 0; i < 4; i++)
        {
            pr_score[i] = score.pr_score[i];
        }
    }
}
