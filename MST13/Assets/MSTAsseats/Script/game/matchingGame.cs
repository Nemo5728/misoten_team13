using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class matchingGame : Photon.PunBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnGUI()
    {

        if (PhotonNetwork.playerList.Length > 0)
        {
            if (PhotonNetwork.isMasterClient && GUI.Button(new Rect(10, 40, 100, 30), "start"))
            {
                Debug.Log("Check A.");

            }
        }
    }
}
