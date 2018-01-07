using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    [SerializeField, TooltipAttribute("Manager群")] private GameObject[] ManagerList;

    private ControllerInfo info = null;
    private bool controllerConnect;

    private float Timer;
    private float gametimr;
    private const float GAME_TIME = 185;
    private const float LOGIN_TIME = 30;

    public enum STATE
    {
        STATE_TITLE,
        STATE_LOGIN,
        STATE_GAME,
        STATE_RESULT
    };

    public STATE state;    // 状態遷移
    GameObject selectObj;   // 選択中のオブジェクト

    void Awake()
    {
        Application.targetFrameRate = 60; //60FPSに設定
    }

	// Use this for initialization
	void Start () 
    {
        Timer = 0f;
        gametimr = 0f;

        //BLEなんちゃら
        info = BLEControlManager.GetControllerInfo();
        //info = SerialControllManager.GetControllerInfo();

        //if (info != null) controllerConnect = true;

        BgmManager.Instance.Play("select");
        state = STATE.STATE_TITLE;
        SetState();
	}

    // Update is called once per frame
    void Update()
    {
        if (controllerConnect)
        {
            if (info.isButtonDown || info.isStickDown)
            {
                if(state == STATE.STATE_TITLE||
                   state == STATE.STATE_RESULT)
                {
                    //選択オブジェクトを削除し、次のステートへ移行
                    selectObj.GetComponent<FadeSceneManager>().SetFadeStart();
                }
           }
        }
        else
        {
            int touch = Input.touchCount;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                selectObj.GetComponent<FadeSceneManager>().SetFadeStart();
            }
            else if(touch > 0)
            {
                Touch tg = Input.GetTouch(0);
                if (tg.phase == TouchPhase.Began)
                {
                    selectObj.GetComponent<FadeSceneManager>().SetFadeStart();
                }
            }

           

        }

        switch (state)
        {

            case STATE.STATE_LOGIN:
                {
                    Timer += Time.deltaTime;

                    if (Timer >= LOGIN_TIME)
                    {
                        Timer = 0f;
                        selectObj.GetComponent<FadeSceneManager>().SetFadeStart();
                    }
                    break;
                }
            case STATE.STATE_GAME:
                {

                    gametimr += Time.deltaTime;

              
                    if (gametimr >= GAME_TIME)
                    {
                        gametimr = 0f;
                        selectObj.GetComponent<FadeSceneManager>().SetFadeStart();
                    }
                    break;
                }
        }

        if (selectObj.GetComponent<FadeSceneManager>().GetFadeState() == FadeSceneManager.Fade.End)
        {
            NextState();
        }
	}

    private void NextState()
    {
        Destroy(selectObj);
        NextState(state);
        SetState();
    }

    // ステート管理
    void SetState()
    {
        //生成したオブジェクトを子オブジェクト化
        selectObj = Instantiate(ManagerList[(int)state], transform.position, Quaternion.identity);
        selectObj.transform.parent = transform;
    }

    // ステート遷移
    void NextState(STATE ste)
    {
        switch (ste)
        {
            case STATE.STATE_TITLE:
                {
                    BgmManager.Instance.Stop();
                    BgmManager.Instance.Play("gameBGM1");
                    state = STATE.STATE_LOGIN;

                    break;
                }
            case STATE.STATE_LOGIN:
                {
                  //LoginCloneDelete();
                    BgmManager.Instance.Stop();
                    BgmManager.Instance.Play("gameBGM2");
                    state = STATE.STATE_GAME;
                    break;
                }
            case STATE.STATE_GAME:
                {
                    // クローンの削除
                    GameCloneDelete();
                    BgmManager.Instance.Stop();
                    BgmManager.Instance.Play("result");
                    state = STATE.STATE_RESULT;
                    break;
                }
            case STATE.STATE_RESULT:
                {
                    ResultCloneDelete();
                    BgmManager.Instance.Stop();
                    BgmManager.Instance.Play("select");
                    state = STATE.STATE_TITLE;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void GameCloneDelete()
    {
       
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayManager");

        foreach (GameObject play in objs)
        {
            Destroy(play);
        }


        GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("minion");

        foreach (GameObject mi in tagobjs)
        {
            Destroy(mi);
        }

        GameObject[] Love = GameObject.FindGameObjectsWithTag("ItemLoveUp");

        foreach (GameObject mi in Love)
        {
            Destroy(mi);
        }

        GameObject[] power = GameObject.FindGameObjectsWithTag("ItemPower");

        foreach (GameObject mi in power)
        {
            Destroy(mi);
        }

        GameObject[] miniUp = GameObject.FindGameObjectsWithTag("ItemMiniUp");

        foreach (GameObject mi in miniUp)
        {
            Destroy(mi);
        }

        GameObject[] speed = GameObject.FindGameObjectsWithTag("ItemSpeed");

        foreach (GameObject mi in speed)
        {
            Destroy(mi);
        }

        WebAPIClient.reset();
    }

    void LoginCloneDelete()
    {
        GameObject[] logs = GameObject.FindGameObjectsWithTag("PlayManager");

        foreach (GameObject play in logs)
        {
            Destroy(play);
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in objs)
        {
            Destroy(player);
        }
    }

    void ResultCloneDelete()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject play in objs)
        {
            Destroy(play);
        }
    }
}
