using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginNetwork : Photon.PunBehaviour
{

    bool bNextScene;
    // Use this for initialization
    void Start()
    {
        bNextScene = false;

        PhotonNetwork.ConnectUsingSettings("v10");
        PhotonNetwork.automaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("room1", null, null);
        //
        

    }

    //void OnGUI()
    //{
    //    //GUI.Label(new Rect(10, 10, 100, 30), "players: " + PhotonNetwork.playerList.Length);

    //    //if (PhotonNetwork.isMasterClient && GUI.Button(new Rect(10, 40, 100, 30), "start"))
    //    //{
    //    //    PhotonNetwork.LoadLevel("シーン遷移先");
    //    //}

    //    //プレイヤーが全員サークルに入ったら
    //    if(bNextScene == true)
    //    {
    //        //シーン遷移先
    //        //PhotonNetwork.LoadLevel("Game");
    //        Debug.Log("準備OK！！！！！！！");
    //    }
    //}

    //プレイヤーの位置がサークルに入ったかどうかを取得
    public void GetReadyOK(bool b)
    {
        bNextScene = b;
    }

}
