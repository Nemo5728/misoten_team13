using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class inputController : MonoBehaviour 
{
	ControllerInfo info;
	TSRigidBody body;
	Vector3 len;
	float def = 540f;
	// Use this for initialization
	void Start () 
	{
		info = SerialControllManager.GetControllerInfo ();
		body = GetComponent<TSRigidBody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float h1 = info.stickX - def;
		float v1 = info.stickY - def;

		float ang = Mathf.Atan2 (v1, h1);
		if (h1 != 0 || v1 != 0) 
		{
			Debug.Log ("スティック動いてるよ！");
		}
		Debug.Log (h1);


		transform.position = new Vector3 (transform.position.x + (h1 * 0.02f), 0 ,transform.position.z + (v1 * 0.02f) );
	}
}
