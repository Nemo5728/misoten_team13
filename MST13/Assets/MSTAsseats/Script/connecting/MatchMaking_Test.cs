using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchMaking_Test : Photon.PunBehaviour
{

    private bool serverStatus;
    private Text startText;
    private Text failedText;
    private float startAlpha;
    private float failedAlpha;

    void Start()
    {
        // haro-
        startText = GameObject.Find("Canvas/Start").GetComponent<Text>();
        failedText = GameObject.Find("Canvas/Failed").GetComponent<Text>();

        PhotonNetwork.ConnectUsingSettings("v1.0");
        PhotonNetwork.automaticallySyncScene = true;
    }

    //  public void OnConnectionFail() {
    //      Debug.Log( "接続エラー：　接続失敗." );
    //  }

    public void OnFailedToConnectToPhoton()
    {
        Debug.Log("サーバーエラー：　接続失敗.");

        startAlpha = Mathf.Clamp(0.0f, 0, 1.0f);
        failedAlpha = Mathf.Clamp(1.0f, 0, 1.0f);

        Color sa = startText.color;
        Color fa = failedText.color;

        sa.a = startAlpha;
        fa.a = failedAlpha;

        startText.color = sa;
        failedText.color = fa;
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("room1", null, null);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "players: " + PhotonNetwork.playerList.Length);

        if (PhotonNetwork.playerList.Length > 0)
        {
            if (PhotonNetwork.isMasterClient && GUI.Button(new Rect(10, 40, 100, 30), "login"))
            {
                PhotonNetwork.LoadLevel("Login");

                //SceneManager.LoadScene("Login");

                Debug.Log("Check A.");
            }
            if (PhotonNetwork.isMasterClient && GUI.Button(new Rect(10, 100, 100, 30), "game"))
            {
                PhotonNetwork.LoadLevel("Game");

                //SceneManager.LoadScene("Game");

                Debug.Log("Check A.");
            }
        }
    }
}