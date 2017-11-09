using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testParticle : MonoBehaviour {

    private ParticleSystem particle;

    // Use this for initialization
    void Start () {
        particle = this.GetComponent<ParticleSystem>();

        // ここで Particle System を停止する.
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("s"))
        {
            // ここで Particle System を開始します.
            particle.Play();
        }
    }
}
