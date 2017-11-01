using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogMotionDemo : MonoBehaviour {

	private Animator anime;  		// アニメーション(ミニオン（犬））

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
			anime.SetBool ("dogMove", true);
		} 
		// 上キーを話したら
		else if (Input.GetKeyUp ("up"))
		{
			// 待機状態へ
			anime.SetBool ("dogMove",false );
		}

		if (Input.GetKeyDown ("e")) 
		{
			// ダウンモーションへ
			anime.SetTrigger("dogDown");
		}


		if (Input.GetKeyDown ("r")) 
		{
			// 召喚モーションへ
			anime.SetTrigger("dogSpawn");
		}

		if (Input.GetKeyDown ("a"))
		{
			// 弱攻撃モーションへ
			anime.SetTrigger("dogWeakAttack");
		}
		else if (Input.GetKeyDown ("s"))
		{
			// 強攻撃モーションへ
			anime.SetTrigger("dogStrAttack");
		}


		if (Input.GetKeyDown ("b")) 
		{
			// ダメージモーションへ
			anime.SetTrigger("dogDamage");
		}

        if (Input.GetKeyDown("g"))
        {
            // 弱攻撃モーションへ
            anime.SetTrigger("dogMoveWeakAttack");
        }
        else if (Input.GetKeyDown("h"))
        {
            // 強攻撃モーションへ
            anime.SetTrigger("dogMoveStrAttack");
        }

	}
}
