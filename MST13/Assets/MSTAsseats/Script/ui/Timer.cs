
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int MAX_TIME = 60; // カウントダウンの開始値
    float timeCounter;

    int width = Screen.width;//画面横幅
    int height = Screen.height;//画面縦幅
    private Vector3 pos;
    private Vector3 scl;
    private Color col;
    void Start()
    {
        timeCounter = MAX_TIME;
        GetComponent<UnityEngine.UI.Text>().text = MAX_TIME.ToString();

        pos = new Vector3(width / 1.3f, height / 1.3f, 0.0f);//位置の座標
        scl = new Vector3(2.0f, 2.0f, 2.0f);//サイズの大きさ
        col = new Color(0.1f, 1.0f, 0.8f);//色の値

        GetComponent<UnityEngine.UI.Text>().transform.position = pos;//位置変更
        GetComponent<UnityEngine.UI.Text>().transform.localScale = scl;//サイズ変更
        GetComponent<UnityEngine.UI.Text>().color = col;//色変更
    }

    void Update()
    {
        timeCounter -= Time.deltaTime;

        // マイナス値にならないようにしている
        timeCounter = Mathf.Max(timeCounter, 0.0f);
        GetComponent<UnityEngine.UI.Text>().text = ((int)timeCounter).ToString();

        //timeCounter += Time.deltaTime; //スタートしてからの秒数を格納
        //GetComponent<Text>().text = countTime.ToString("F2"); //小数2桁にして表示
    }
}

