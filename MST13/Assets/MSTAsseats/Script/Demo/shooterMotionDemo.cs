using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterMotionDemo : MonoBehaviour {

	private Animator anime;  		// アニメーション(ミニオン(シューター))

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
			anime.SetBool ("shooterMove", true);
		} 
		// 上キーを話したら
		else if (Input.GetKeyUp ("up"))
		{
			// 待機状態へ
			anime.SetBool ("shooterMove",false );
		}

		if (Input.GetKeyDown ("e")) 
		{
			// ダウンモーションへ
			anime.SetTrigger("shooterDown");
		}

		if (Input.GetKeyDown ("r")) 
		{
			// 召喚モーションへ
			anime.SetTrigger("shooterSpawn");
		}

		if (Input.GetKeyDown ("a"))
		{
			// 弱攻撃モーションへ
			anime.SetTrigger("shooterAttack_A");
		}
		else if (Input.GetKeyDown ("s"))
		{
			// 強攻撃モーションへ
			anime.SetTrigger("shooterAttack_B");
		}

		if (Input.GetKeyDown ("b")) 
		{
			// ダメージモーションへ
			anime.SetTrigger("shooterDamage");
		}
	}
}
