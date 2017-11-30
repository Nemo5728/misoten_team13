using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WebAPIClient : MonoBehaviour {

    static string serverAddres;

    [RuntimeInitializeOnLoadMethod()]
    static void loadServerConfig(){
        WebAPIConfigs config = Resources.Load<WebAPIConfigs>("WebAPIConfigs");
        serverAddres = config.apiUrl;
        Debug.Log(serverAddres);
    }

    public static void send(int player, int score){
        WWW www = new WWW(serverAddres + player.ToString() + "/" + score.ToString());
    }

    public static void reset(){
        WWW www = new WWW(serverAddres + "reset/");

    }
}
