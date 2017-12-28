using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialControllManager : SingletonMonoBehaviour<SerialControllManager> {

    [SerializeField]SerialHandler   sh;
    static ControllerInfo controllerInfo;
    bool oldButtonPush;
    bool oldStickPush;

    // Use this for initialization
    void Awake()
    {
        controllerInfo = new ControllerInfo();
        if(this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    } 
	
	// Update is called once per frame
	void Update () {
        string message;
        message = sh.GetMessage();

        if (message != null)
        {

            Debug.Log(message);

            controllerInfo.isStickDown = false;
            controllerInfo.isStickUp = false;
            controllerInfo.isButtonDown = false;
            controllerInfo.isButtonUp = false;

            oldStickPush = controllerInfo.isStick;
            oldButtonPush = controllerInfo.isButton;

            controllerInfo.isStick = (int.Parse(message.Substring(0, 1)) == 0) ? true : false;

            if (oldStickPush == false && controllerInfo.isStick == true)
            {
                controllerInfo.isStickUp = true;
            }
            else if (oldStickPush == true && controllerInfo.isStick == false)
            {
                controllerInfo.isStickDown = true;
            }

            controllerInfo.isButton = (int.Parse(message.Substring(1, 1)) == 0) ? true : false;

            if (oldButtonPush == false && controllerInfo.isButton == true)
            {
                controllerInfo.isButtonUp = true;
            }
            else if (oldButtonPush == true && controllerInfo.isButton == false)
            {
                controllerInfo.isButtonDown = true;
            }

            controllerInfo.stickX = System.Convert.ToInt32("0" + message.Substring(2, 3), 16);
            controllerInfo.stickY = System.Convert.ToInt32("0" + message.Substring(5, 3), 16);
        } else{
            controllerInfo.stickX = -1;
            controllerInfo.stickY = -1;
        }

	}

    public static ControllerInfo GetControllerInfo(){
        return controllerInfo;
    }
}
