using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    [SerializeField, TooltipAttribute("Manager群")] private GameObject[] ManagerList;

    private ControllerInfo info = null;
    private bool controllerConnect;

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
            // 選択オブジェクトを削除し、次のステートへ移行
            Destroy(selectObj);
            NextState(state);
            SetState();
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
                    // クローンの削除
                    GameCloneDelete();

                    state = STATE.STATE_GAME;
                    break;
                }
            case STATE.STATE_GAME:
                {
                   

                    state = STATE.STATE_TITLE;
                    break;
                }
            case STATE.STATE_RESULT:
                {
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
       
        GameObject obj = GameObject.Find("TestManager(Clone)");
        Destroy(obj);

        GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("minion");

        foreach (GameObject mi in tagobjs)
        {
            Destroy(mi);
        }
    }
}
