using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public GameObject logo;
    GameObject obj;
    GameObject conect;
    public GameObject text;
    float alpha;

    private enum STATE
    {
        STATE_START,
        STATE_NORMAL,
        STATE_END
    };

    STATE state;
	// Use this for initialization
	void Start ()
    {
        alpha = 0f;
        obj = Instantiate(logo,transform.position,new Quaternion(0f, 180f, 0f, 1f));
        obj.transform.parent = transform;

        conect = Instantiate(text, transform.position, Quaternion.identity);
        conect.transform.parent = transform;
        state = STATE.STATE_START;
	}

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.STATE_START:
                {
                    alpha += 1f / 60;

                    if (alpha >= 1f)
                    {
                        state = STATE.STATE_NORMAL;
                    }
                    break;
                }
            case STATE.STATE_NORMAL:
                {
                    break;
                }
            case STATE.STATE_END:
                {
                    break;
                }
            default:
                {
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

    public void AlphaState()
    {
        switch (state)
        {
            case STATE.STATE_START:
                {
                    state = STATE.STATE_NORMAL;
                    break;
                }
            case STATE.STATE_NORMAL:
                {
                    state = STATE.STATE_END;
                    break;
                }
            case STATE.STATE_END:
                {
                    state = STATE.STATE_START;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
