using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BLEText : MonoBehaviour {

    public Text isStickX;
    public Text isStickY;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        ControllerInfo ci = BLEControlManager.GetControllerInfo();

        isStickX.text = ci.stickX.ToString();
        isStickY.text = ci.stickY.ToString();
	}
}
