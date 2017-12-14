using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingManager : MonoBehaviour {

    public GameObject text;
    GameObject obj;

	// Use this for initialization
	void Start () 
    {
        // 生成したオブジェクトを子オブジェクトに変更
        obj = Instantiate(text, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AllDelete()
    {
        Destroy(obj);
        Destroy(gameObject);
    }
}
