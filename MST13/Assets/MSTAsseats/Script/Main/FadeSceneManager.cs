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

    [SerializeField, TooltipAttribute("フェードさせたいMeshRendererを持ったやつ")] private GameObject[] fadeObjectForMesh;
    [SerializeField, TooltipAttribute("1フレーム毎の変化量")] private float fadeTime;
    [SerializeField, TooltipAttribute("タイトル例外処理")] private bool titleEnable;

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
            fadeMesh[i] = fadeObjectForMesh[i].GetComponent<MeshRenderer>();
            if (titleEnable) fadeMesh[i].material.SetFloat("_alpha", alpha);
            else fadeMesh[i].material.SetFloat("_loadUVpan", alpha);
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
                    if (titleEnable) fadeMesh[i].material.SetFloat("_alpha", alpha);
                    else fadeMesh[i].material.SetFloat("_loadUVpan", alpha);
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
                    if (titleEnable) fadeMesh[i].material.SetFloat("_alpha", alpha);
                    else fadeMesh[i].material.SetFloat("_loadUVpan", alpha);
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

    public void AllDestroy()
    {
        Destroy(gameObject);
    }
}
