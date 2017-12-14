using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public GameObject logo;
    GameObject obj;

	// Use this for initialization
	void Start ()
    {
       
        obj =  Instantiate(logo,transform.position,Quaternion.identity);
        obj.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {}

    public void AllDelete()
    {
        Destroy(obj);
        Destroy(gameObject);
    }
}
