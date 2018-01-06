using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingManager : MonoBehaviour {

    public GameObject text;
    GameObject obj;
    float alpha;

    private enum State{
        Start,
        Normal,
        End
    }

    State state;

	// Use this for initialization
	void Start () 
    {
        // 生成したオブジェクトを子オブジェクトに変更
        obj = Instantiate(text, transform.position, Quaternion.identity);
        obj.transform.parent = transform;

        alpha = 0.0f;
        state = State.Start;
	}
	
	// Update is called once per frame
	void Update () {
//        GetComponent<ParticleManager>().Play("FX_ResultFireworkP1", transform.position);

        switch(state){
            case State.Start:{
                    alpha += 1f / 60;

                    if (alpha >= 1f){
                        state = State.Normal;
                    }
                    break;
                }
            case State.Normal:{
                    
                    break;
                }
            case State.End:{
                    
                    break;
                }
            default:{
                    break;
                }
        }

        obj.GetComponent<MeshRenderer>().material.SetFloat("_alpha", alpha);
	}
    public void AllDelete()
    {
        Destroy(obj);
        Destroy(gameObject);
    }
}
