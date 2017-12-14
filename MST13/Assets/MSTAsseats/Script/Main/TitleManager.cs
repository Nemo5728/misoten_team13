using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public GameObject logo;
    GameObject obj;

    private enum STATE
    {
        STATE_TITLE,
        STATE_CONECTING,
        STATE_LOGIN,
        STATE_GAME,
        STATE_RESULT
    };

    private STATE state;


	// Use this for initialization
	void Start ()
    {
        // 生成したオブジェクトを子オブジェクトに変更
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
