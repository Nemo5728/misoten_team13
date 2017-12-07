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
   

    // Use this for initialization
    void Start () 
	{
       
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKey("a"))
        {
            SceneManager.LoadScene("Connecting");
        }

        if (Input.GetKey("p"))
        {
            Vector3  pos = new Vector3(10.0f, 0.0f, 10.0f);
         //   ParticleManager.PlayParticle("PStest", pos);
        }
    }
   
}
