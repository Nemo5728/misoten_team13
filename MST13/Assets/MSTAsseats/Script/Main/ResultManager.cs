using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour {

    public GameObject ResultObject;
    GameObject obj;
    private GameObject[] particleList;
	// Use this for initialization
	void Start () 
    {
        // 生成したオブジェクトを子オブジェクトに変更
        //obj = Instantiate(ResultObject, transform.position, Quaternion.identity);
        //obj.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void AllDelete()
    {
        Destroy(obj);
        Destroy(gameObject);
    }
}
