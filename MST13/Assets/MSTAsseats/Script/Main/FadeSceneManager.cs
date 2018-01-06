using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSceneManager : MonoBehaviour {

    public enum Fade
    {
        In,
        Out,
        End,
        None
    };

    Fade fade;

    //canvas系対応難しいなー
    [SerializeField, TooltipAttribute("フェードさせたいMeshRendererを持ったやつ")] private GameObject[] fadeObjectForMesh;
    [SerializeField, TooltipAttribute("Canvas出すだけ")] private GameObject[] fadeObjectForCanvas;
    [SerializeField, TooltipAttribute("1フレーム毎の変化量")] private float fadeTime;

    private GameObject[] fadeObject;
    private MeshRenderer[] fadeMesh;
    //private Canvas[] fadeCanvas;
    private float alpha;

    // Use this for initialization
    void Start()
    {
        alpha = 0;
        fadeMesh = new MeshRenderer[fadeObjectForMesh.Length];
        fade = Fade.In;

        for (int i = 0; i < fadeObjectForMesh.Length; i++)
        {
            //fadeObject[i] = Instantiate(fadeObjectForMesh[i], transform.position, Quaternion.identity);
            //fadeObject[i].transform.parent = transform;

            fadeMesh[i] = fadeObjectForMesh[i].GetComponent<MeshRenderer>();
            fadeMesh[i].material.SetFloat("_alpha", alpha);
        }

        for (int i = 0; i < fadeObjectForCanvas.Length; i ++)
        {
            //Instantiate(fadeObjectForCanvas[i], transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (fade)
        {
            case Fade.In:
                alpha += fadeTime;

                for (int i = 0; i < fadeMesh.Length; i++)
                {
                    fadeMesh[i].material.SetFloat("_alpha", alpha);
                }

                if (alpha >= 1.0f)
                {
                    fade = Fade.None;
                }

                break;
            case Fade.Out:
                alpha -= fadeTime;

                for (int i = 0; i < fadeMesh.Length; i++)
                {
                    fadeMesh[i].material.SetFloat("_alpha", alpha);
                }

                if (alpha <= 0.0f)
                {
                    fade = Fade.End;
                }

                break;
            default:
                break;
        }
    }

    public Fade GetFadeState()
    {
        return fade;
    }

    public void SetFadeStart()
    {
        fade = Fade.Out;
    }

    public void AllDelete(){
        Destroy(gameObject);
    }
}
