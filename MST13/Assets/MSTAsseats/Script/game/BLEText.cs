using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class BLEText : MonoBehaviour
{

    private const byte INPUT_CONTROLLER_STICKX = 0;
    private const byte INPUT_CONTROLLER_STICKY = 1;

    public Text isStickX;
    public Text isStickY;

    public Text stickX;
    public Text stickY;

    private int StX;
    private int StY;


    // Use this for initialization
    void Start()
    { }

    void Update()
    {
        ControllerInfo ci = BLEControlManager.GetControllerInfo();

        StX = ci.stickX;
        StY = ci.stickY;

        TrueSyncInput.SetInt(INPUT_CONTROLLER_STICKX, StX);
        TrueSyncInput.SetInt(INPUT_CONTROLLER_STICKY, StY);

        isStickX.text = ci.stickX.ToString();
        isStickY.text = ci.stickY.ToString();

        int StickX = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKX);
        int StickY = -550 + TrueSyncInput.GetInt(INPUT_CONTROLLER_STICKY);

        stickX.text = StickX.ToString();
        stickY.text = StickY.ToString();

    }

}