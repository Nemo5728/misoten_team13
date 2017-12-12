using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class follCollider : MonoBehaviour {

    public GameObject objTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = objTarget.transform.position;
        transform.position = pos;
	}
}
