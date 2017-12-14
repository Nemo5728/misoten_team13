using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginTimer : MonoBehaviour {

    public GameObject[] stageTex;

    private int MAX_TIME = 180; // カウントダウンの開始値
    private int MAX_TIMEPOINT = 60; // 秒数の最大値
    //private int OBJ_MAX = 5; // 秒数の最大値

    float timeCounter;
    int threeCount;

    int i;
    int old_i;
    float ClockLoghtCount;
    int oldTime;

    void Start()
    {
        i = 0;
        old_i = MAX_TIME;
        oldTime = 0;
        timeCounter = MAX_TIME;
        threeCount = MAX_TIMEPOINT;

        //ステージ上の各オブジェクトのシェーダの初期数値を設定
        MeshRenderer floor = stageTex[0].GetComponent<MeshRenderer>();
        MeshRenderer stage = stageTex[1].GetComponent<MeshRenderer>();
        MeshRenderer timerClock = stageTex[2].GetComponent<MeshRenderer>();
        MeshRenderer timeRing = stageTex[3].GetComponent<MeshRenderer>();

        //floor
        floor.material.SetFloat("_ChangeColor", 0.0f);//青から赤に変わるカラー変更

        //stage
        stage.material.SetFloat("_ChangeColor", 0.0f);//青から赤に変わるカラー変更

        //timerClock
        timerClock.material.SetFloat("_ClockCount", 60.0f);//タイムカウント
        timerClock.material.SetFloat("_ClockLoght", 0.0f);//タイムの一瞬光るライト
        timerClock.material.SetFloat("_ChangeColor", 0.0f);//青から赤に変わるカラー変更

        //timeRing
        timeRing.material.SetFloat("_ChangeColor", 0.0f);//青から赤に変わるカラー変更

        //r.material.EnableKeyword("_NORMALMAP");//ノーマルマップの有効化
        //r.material.EnableKeyword("_ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON");//透明度の有効化
        //r.material.EnableKeyword("_EMISSION"); //Emission カラー か Emission マップの有効化


    }

    void Update()
    {

        MeshRenderer floor = stageTex[0].GetComponent<MeshRenderer>();
        MeshRenderer stage = stageTex[1].GetComponent<MeshRenderer>();
        MeshRenderer timerClock = stageTex[2].GetComponent<MeshRenderer>();
        MeshRenderer timeRing = stageTex[3].GetComponent<MeshRenderer>();
        timeCounter -= Time.deltaTime;//カウントをマイナス

        if ((int)timeCounter % 1 == 0 && oldTime != (int)timeCounter)//3で割り切れるなら
        {
            oldTime = (int)timeCounter;
            threeCount -= 2;
        }

        if (((int)timeCounter + 1) % 1 == 0)//タイマーとステージの光波動のタイミングを合わせる
        {
            timerClock.material.SetFloat("_ClockCount", threeCount);//タイムカウントを減らしていく
        }

        if (threeCount == (MAX_TIMEPOINT / 4))//カウントが４分の１になったら
        {
            floor.material.SetFloat("_ChangeColor", 1.0f);//赤にカラー変更
            stage.material.SetFloat("_ChangeColor", 1.0f);//赤にカラー変更
            timerClock.material.SetFloat("_ChangeColor", 1.0f);//赤にカラー変更
            timeRing.material.SetFloat("_ChangeColor", 1.0f);//赤にカラー変更
        }


        //i = (int)timeCounter;
        //if (i != old_i)//１秒ごとに
        //{
        //    old_i = i;
        //    ClockLoghtCount = 0.15f;
        //}

        //if (ClockLoghtCount != 0.0f && ClockLoghtCount > 0)
        //{
        //    ClockLoghtCount -= 0.01f;
        //    timerClock.material.SetFloat("_ClockLight", ClockLoghtCount);//一瞬薄く光る
        //}



        // マイナス値にならないようにしている
        timeCounter = Mathf.Max(timeCounter, 0.0f);
        //GetComponent<UnityEngine.UI.Text>().text = threeCount.ToString(); 
        //Debug.Log(timeCounter);

    }
}
