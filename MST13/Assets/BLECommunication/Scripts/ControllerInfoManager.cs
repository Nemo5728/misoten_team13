using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerInfoManager : MonoBehaviour {

    public string playerString;

	// Use this for initialization
	public void Start () {
        DontDestroyOnLoad(gameObject);
	}
    public void DecideOne(){
        playerString = "MST1301";
        SceneManager.LoadScene("Main");
    }
    public void DecideTwo(){
        playerString = "MST1302";
        SceneManager.LoadScene("Main");
    }
    public void DecideThree(){
        playerString = "MST1303";
        SceneManager.LoadScene("Main");
    }
    public void DecideFour(){
        playerString = "MST1304";
        SceneManager.LoadScene("Main");
    }
}
