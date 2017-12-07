



using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//  パーティクルのプーリング用クラス
public class ParticlePooler : MonoBehaviour
{
    public ParticlePooler(string particleName)
    {
        this.particleName = particleName;
    }

 
    // パーティクル名
    private string particleName;
    public string ParticleName
    {
        get { return particleName; }
    }

  
    // パーティクルを保持しておくリスト
    private List<ParticleSystem> particleList = new List<ParticleSystem>();
    public List<ParticleSystem> ParticleList
    {
        get { return particleList; }
    }


   
	// 生成元のパーティクル
	private GameObject particleOrigin = null;


    // 指定の座標で再生
    /// <param name="position">Position.</param>
    public void Play(Vector3 position)
    {
        ParticleSystem particle = GetPlayableParticle();
        if (particle == null)
        {
            particle = InstantiateParticle();
        }
        particle.transform.position = position;
        particle.Play();
    }

    // 指定の座標で再生
    /// <param name="position">Position.</param>
    public void Play(Vector3 position, int waitTime)
    {
        ParticleSystem particle = GetPlayableParticle();
        if (particle == null)
        {
            particle = InstantiateParticle();
        }

        StartCoroutine(WaitFunction(waitTime));

        particle.transform.position = position;
        particle.Play();
    }

    IEnumerator WaitFunction(float waitTime) {
        yield return new WaitForSeconds(waitTime);
    }


    // 再生可能なパーティクルを取得
    /// <returns>再生可能なパーティクル.</returns>
    private ParticleSystem GetPlayableParticle()
    {
        return particleList.Where(particle => !particle.isPlaying).FirstOrDefault();
    }

    // パーティクル生成
    /// <returns>The particle.</returns>
    private ParticleSystem InstantiateParticle()
    {
        LoadOrigin();
        GameObject particleGO = GameObject.Instantiate(particleOrigin) as GameObject;
        ParticleSystem particle = particleGO.GetComponent<ParticleSystem>();
        particleList.Add(particle);
        return particle;
    }

    
    // 生成元のオブジェクトをロード
    private void LoadOrigin()
    {
        if (particleOrigin == null)
        {
            particleOrigin = Resources.Load(particleName) as GameObject;
            particleOrigin.name = particleName;
        }
    }

    
    // 破棄時処理
    private void Clean()
    {
        particleList.Clear();
        particleList = null;
        particleOrigin = null;
    }
}



//using UnityEngine;
//using System;

//public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
//{

//    private static T instance;
//    public static T Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                Type t = typeof(T);

//                instance = (T)FindObjectOfType(t);
//                if (instance == null)
//                {
//                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
//                }
//            }

//            return instance;
//        }
//    }

//    virtual protected void Awake()
//    {
//        // 他のゲームオブジェクトにアタッチされているか調べる
//        // アタッチされている場合は破棄する。
//        CheckInstance();
//    }

//    protected bool CheckInstance()
//    {
//        if (instance == null)
//        {
//            instance = this as T;
//            return true;
//        }
//        else if (Instance == this)
//        {
//            return true;
//        }
//        Destroy(this);
//        return false;
//    }
//}
