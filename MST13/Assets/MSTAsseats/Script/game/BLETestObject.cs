using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLETestObject : MonoBehaviour {

    private ControllerInfo info = null;
    float speed = 3f;
    private Vector3 directionVector = Vector3.zero;
    // Use this for initialization

    float timmer;
	void Start ()
    {
        // info = BLEControlManager.GetControllerInfo();
        timmer = 0f;
        

	}
	
	// Update is called once per frame
	void Update ()
    {
        timmer += Time.deltaTime;

        if (timmer >= 10f)
        {
            timmer = 0f;
            GameObject particle = GameObject.Find("Particle");
            particle.GetComponent<ParticleManager>().Play("FX_BannerTransP1", transform.position);
        }
        /*
        Vector3 vector = Vector3.zero;
        directionVector.x = vector.x = speed * (info.stickX / 473);
        directionVector.z = vector.z = speed * (info.stickY / 473);


        vector = Vector3.Normalize(vector);
        directionVector = Vector3.Normalize(directionVector);
        float direction = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, direction, 0.0f);

       

        transform.Translate((vector * speed) * Time.deltaTime, Space.World);
        
*/


	}
}
