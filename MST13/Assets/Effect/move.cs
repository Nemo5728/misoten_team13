using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
    private Vector3 pos;
    public GameObject player;
    float Speed;

    // Use this for initialization
    void Start () {
        pos = new Vector3(0.0f, 0.0f, 0.0f);
        Speed = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("q"))
        {
            Speed += 0.1f;
        }
        if (Input.GetKey("e"))
        {
            Speed -= 0.1f;
        }
        if(Speed < 0)
        {
            Speed = 0.0f;
        }

        if (Input.GetKey("w"))
        {
            pos.z += Speed;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= Speed;
        }
        if (Input.GetKey("d"))
        {
            pos.x += Speed;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= Speed;
        }
        transform.position += pos;//位置変更
        pos = new Vector3(0.0f, 0.0f, 0.0f);

    }
}
