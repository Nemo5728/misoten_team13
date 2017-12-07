using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ParticleManager : MonoBehaviour
{
    [System.Serializable]   
    public class EffectInf
    {

        public int waitTime;
        public GameObject obj;


    }
    public List<EffectInf> ParticleList;
    Dictionary<string, EffectInf> ParticleDic = new Dictionary<string, EffectInf>();
    Vector3 pos;

   
    private void Start()
    {
        pos = new Vector3(0.0f, 0.0f, 0.0f);
        //リスト作成
        foreach (var particle in ParticleList)
        {
            ParticleDic.Add(particle.obj.name, particle);
        }
        
    }

    // 指定の座標で再生
    public void Play(string particleName, Vector3 position ,bool roop = false)
    {
        StartCoroutine(WaitFunction(particleName, position, roop));

       

    }

    IEnumerator WaitFunction(string particleName, Vector3 position, bool roop = false)
    {
        yield return new WaitForFrame(ParticleDic[particleName].waitTime);
        GameObject particle = Instantiate(ParticleDic[particleName].obj);

        particle.transform.position = position;
        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        if (roop)
        {
            StartCoroutine(WaitDestroy(ps.time, particle));

        }
    }

    IEnumerator WaitDestroy(float frame, GameObject obj)
    {
        yield return new WaitForSeconds(frame);
        Destroy(obj);
    }


    void Update()
    {
        if (Input.GetKey("s"))
        {
            Play("aaaaaa", pos);
        }
    }


}