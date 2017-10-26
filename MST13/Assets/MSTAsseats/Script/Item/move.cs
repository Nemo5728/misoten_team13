using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
    private Vector3 pos;
    public GameObject player;

    // Use this for initialization
    void Start () {
        pos = new Vector3(0.0f, 0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
       
        
        //if (Input.GetKey("w"))
        //{
        //    pos.z += 0.1f;
        //}
        //if (Input.GetKey("s"))
        //{
        //    pos.z -= 0.1f;
        //}
        //if (Input.GetKey("d"))
        //{
        //    pos.x += 0.1f;
        //}
        //if (Input.GetKey("a"))
        //{
        //    pos.x -= 0.1f;
        //}
        //transform.position += pos;//位置変更
        //pos = new Vector3(0.0f, 0.0f, 0.0f);

    }
}
