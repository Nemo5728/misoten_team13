using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bannerMotionDemo : MonoBehaviour 
{

	private Animator anime;  		// アニメーション(プレイヤー）

	// Use this for initialization
	void Start ()
	{
		anime = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// キーを推してモーションテスト！

		// 上キー押したら
		if (Input.GetKey ("up")) 
		{
			// 移動モーション
			anime.SetBool ("bannerMove", true);
		} 
		// 上キーを話したら
		else if (Input.GetKeyUp ("up"))
		{
			// 待機状態へ
			anime.SetBool ("bannerMove",false );
		}

		if (Input.GetKeyDown ("e")) 
		{
			// ダウンモーションへ
			anime.SetBool("bannerDown",true);
		}
		else if (Input.GetKeyDown ("d"))
		{
			// 待機状態へ
			anime.SetBool ("bannerDown",false );
		}

		if (Input.GetKeyDown ("t")) 
		{
			// 変身モーションへ
			anime.SetTrigger("bannerTransform");
		}
	}
}
