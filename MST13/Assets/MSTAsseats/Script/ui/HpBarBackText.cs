using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HpBarBackText : MonoBehaviour {

    int width = Screen.width;//画面横幅
    int height = Screen.height;//画面縦幅
    private Vector3 pos;
    private Vector3 scl;
    public Image _HpBarBackText;


    // Use this for initialization

    void Start () {


        pos = new Vector3(width / 5.0f, height / 1.2f, 0.0f);
        scl = new Vector3(2.0f, 1.2f, 1.5f);
        _HpBarBackText.transform.position = pos;//位置変更
        //_HpBarBackText.transform.lossyScale = scl;
        _HpBarBackText.transform.localScale = scl;　//サイズ変更
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
