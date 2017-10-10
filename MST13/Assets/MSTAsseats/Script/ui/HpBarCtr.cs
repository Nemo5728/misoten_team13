using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 


public class HpBarCtr : MonoBehaviour
{
    int width = Screen.width;//画面横幅
    int height = Screen.height;//画面縦幅
    private Vector3 pos;
   // public GameObject HpBar;//オブジェクト
    Slider _slider;

    // Use this for initialization
    void Start()
    {
        // スライダーを取得する
        _slider = GameObject.Find("Slider").GetComponent<Slider>();

        pos = new Vector3(width / 5.0f, height / 1.2f, 0.0f);
        _slider.transform.position = pos;
    }

    float _hp = 0;


    // Update is called once per frame
    void Update()
    {
        _hp += 0.01f;   // HP上昇
        if (_hp > 1)
        {   
            _hp = 0;    // 最大を超えたら0に戻す
        }
        //_hp += 1;   // HP上昇
        //if (_hp > _slider.maxValue)
        //{
        //    //最大を超えたら0に戻す
        //   _hp = _slider.minValue;
        //}
        _slider.value = _hp;    // HPゲージに値を設定
    }
  
}