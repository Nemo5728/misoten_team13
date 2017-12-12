using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 作成者　：　関谷富明
/// 更新日　：
/// 内容　　；　titlesceneの制御
/// </summary>
/// 

public class title : MonoBehaviour 
{
    private ControllerInfo info = null;
    private bool controllerConnect;

    // Use this for initialization
    void Start () 
	{
        info = BLEControlManager.GetControllerInfo();
        //info = SerialControllManager.GetControllerInfo();

        if (info != null) controllerConnect = true;

        if (controllerConnect)
        {
            int stickX = info.stickX;
            int stickY = info.stickY;
            bool button = info.isButtonDown;
            bool stickBtn = info.isStickDown;
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (controllerConnect)
        {
            if (info.isButtonDown)
            {
                SceneManager.LoadScene("Connecting");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Connecting");
            }

        }

        if (Input.GetKey("p"))
        {
            Vector3  pos = new Vector3(10.0f, 0.0f, 10.0f);
            ParticleManager.PlayParticle("PStest", pos);
        }
    }
   
}
