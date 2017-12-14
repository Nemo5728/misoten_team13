using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class LoginPlayManager : TrueSyncBehaviour
{

    private GameObject PlayerObject;


    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public override void OnSyncedStart()
    {

        if (owner.Id != 0)
        {
            Debug.Log("LoginPlayManager:オンラインモードなう");
            Debug.Log(owner.Id);


            for (int i = 0; i < 4; i++)
            {
                if ((owner.Id - 1) != i)
                {
                    Debug.Log("loginplayer" + (i + 1));
                    PlayerObject = transform.Find("loginplayer" + (i + 1)).gameObject;
                    PlayerObject.SetActive(false);

                }
            }


        }
        else
        {  //オフラインモードの例外処理
            Debug.Log("LoginPlayManager:オフラインモードなう");
            Debug.Log(owner.Id);

            PlayerObject = transform.Find("loginplayer2").gameObject;
            PlayerObject.SetActive(false);

            PlayerObject = transform.Find("loginplayer3").gameObject;
            PlayerObject.SetActive(false);

            PlayerObject = transform.Find("loginplayer4").gameObject;
            PlayerObject.SetActive(false);
        }
    }

    public override void OnSyncedUpdate()
    { }

}
