using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTime : MonoBehaviour {
    private int MAX_TIME = 180; // カウントダウンの開始値
    float timeCounter;
    int threeCount;

    int width = Screen.width;//画面横幅
    int height = Screen.height;//画面縦幅
    private Vector3 pos;
    private Vector3 scl;
    private Color col;

    int i;
    int oldTime;

    void Start()
    {
        oldTime = 0;
        timeCounter = MAX_TIME;
        GetComponent<UnityEngine.UI.Text>().text = MAX_TIME.ToString();

        pos = new Vector3(-150.0f + 246.0f, -100.0f + 96.0f, 150.0f);//位置の座標
        scl = new Vector3(2.0f, 2.0f, 2.0f);//サイズの大きさ
        col = new Color(0.1f, 1.0f, 0.8f);//色の値

        GetComponent<UnityEngine.UI.Text>().transform.position = pos;//位置変更
        GetComponent<UnityEngine.UI.Text>().transform.localScale = scl;//サイズ変更
        GetComponent<UnityEngine.UI.Text>().color = col;//色変更
    }

    void Update()
    {
        timeCounter -= Time.deltaTime;//カウントをマイナス
        i++;
        if ((int)timeCounter % 3 == 0 && oldTime != (int)timeCounter)//3で割り切れるなら
        {
            oldTime = (int)timeCounter;
            i = 0;
            threeCount++;
        }
         

        // マイナス値にならないようにしている
        timeCounter = Mathf.Max(timeCounter, 0.0f);
        //GetComponent<UnityEngine.UI.Text>().text = ((int)timeCounter).ToString();
        GetComponent<UnityEngine.UI.Text>().text = threeCount.ToString(); 
        Debug.Log(timeCounter);

    }
}
