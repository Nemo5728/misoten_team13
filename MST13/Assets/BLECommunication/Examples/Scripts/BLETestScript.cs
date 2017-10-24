using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BLETestScript : MonoBehaviour
{

    public Text isButtonText;
    public Text isButtonDownText;
    public Text isButtonUpText;

    public Text isStickText;
    public Text isStickDownText;
    public Text isStickUpText;

    public Text isStickX;
    public Text isStickY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControllerInfo ci = BLEControlManager.GetControllerInfo();
        isButtonText.text = ci.isButton.ToString();
        isButtonUpText.text = ci.isButtonUp.ToString();
        isButtonDownText.text = ci.isButtonDown.ToString();

        isStickText.text = ci.isStick.ToString();
        isStickUpText.text = ci.isStickUp.ToString();
        isStickDownText.text = ci.isStickDown.ToString();

        isStickX.text = ci.stickX.ToString();
        isStickY.text = ci.stickY.ToString();

    }
}
