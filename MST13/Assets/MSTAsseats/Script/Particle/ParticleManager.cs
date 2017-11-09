using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ParticleManager : MonoBehaviour
{
    // プール用オブジェクトのリスト
    private static List<ParticlePooler> particlePoolerList = new List<ParticlePooler>();


    // 指定した名前のパーティクル再生
    // 	初めて再生するパーティクルはプール用オブジェクトを生成
    /// <param name="particleName">Particle name.</param>
    /// <param name="position">Position.</param>
    public static void PlayParticle(string particleName, Vector3 position)
    {
        //リストから指定した名前のプール用オブジェクトを取得
        ParticlePooler pooler = particlePoolerList.Where(tempPooler => tempPooler.ParticleName == particleName).FirstOrDefault();
        if (pooler == null)
        {
            //取得できなければ新たに生成
            pooler = new ParticlePooler(particleName);
            particlePoolerList.Add(pooler);
        }
        pooler.Play(position);
    }
}



///// <summary>
///// 使い方
///// ①Resourcesディレクトリ配下に生成したいパーティクルを配置
///// ②Instantiate(ParticleManager.Instance.Create("パーティクル名"));
///// </summary>
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ParticleManager : SingletonMonoBehaviour<ParticleManager>
//{

//    public GameObject Create(string name)
//    {
//        return (GameObject)Resources.Load(string.Format("Particles/{0}", name));
//    }
//}

//Instantiate(ParticleManager.Instance.Create("パーティクル名"));//使いたいところで
