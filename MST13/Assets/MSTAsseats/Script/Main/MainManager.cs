﻿using System.Collections;
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

    private enum STATE
    {
        STATE_TITLE,
        STATE_CONECTING,
        STATE_LOGIN,
        STATE_GAME,
        STATE_RESULT
    };

    private STATE state;    // 状態遷移
    GameObject selectObj;   // 選択中のオブジェクト

	// Use this for initialization
	void Start () 
    {
        //BLEなんちゃら
        info = BLEControlManager.GetControllerInfo();
        // info = SerialControllManager.GetControllerInfo();

        if (info != null) controllerConnect = true;

        state = STATE.STATE_TITLE;
        SetState();
	}

    // Update is called once per frame
    void Update()
    {
        if (controllerConnect)
        {
            if(info.isButtonDown)
            {
                if(state != STATE.STATE_LOGIN  ||
                   state != STATE.STATE_GAME )
                // 選択オブジェクトを削除し、次のステートへ移行
                Destroy(selectObj);
                NextState(state);
                SetState();
            }
           
        }
        else
        {
            int touch = Input.touchCount;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(selectObj);
                NextState(state);
                SetState();
            }
            else if( touch > 0)
            {
                Touch tg = Input.GetTouch(0);
                if (tg.phase == TouchPhase.Began)
                {
                    Destroy(selectObj);
                    NextState(state);
                    SetState();
                }
            }

            switch (state)
            {
                case STATE.STATE_LOGIN:
                {
                        Timer += Time.deltaTime;

                        if (Timer >= LOGIN_TIME)
                        {
                            Destroy(selectObj);
                            NextState(state);
                            SetState();
                        }
                    break;
                }
                case STATE.STATE_GAME:
                {

                        gametimr += Time.deltaTime;

                        if (gametimr >= GAME_TIME)
                        {
                            Destroy(selectObj);
                            NextState(state);
                            SetState();
                        }
                    break;
                }
            }

        }

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
                    state = STATE.STATE_CONECTING;
                    break;
                }
            case STATE.STATE_CONECTING:
                {
                    state = STATE.STATE_LOGIN;
                    break;
                }
            case STATE.STATE_LOGIN:
                {
                    LoginCloneDelete();
                    state = STATE.STATE_GAME;
                    break;
                }
            case STATE.STATE_GAME:
                {
                    // クローンの削除
                    GameCloneDelete();
                    state = STATE.STATE_RESULT;
                    break;
                }
            case STATE.STATE_RESULT:
                {
                    ResultCloneDelete();
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
    }

    void LoginCloneDelete()
    {
        GameObject[] logs = GameObject.FindGameObjectsWithTag("PlayManager");

        foreach (GameObject play in logs)
        {
            Destroy(play);
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject play in objs)
        {
            Destroy(play);
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