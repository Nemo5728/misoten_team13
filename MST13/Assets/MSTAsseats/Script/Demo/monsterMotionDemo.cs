using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterMotionDemo : MonoBehaviour {

	private Animator anime;  		// アニメーション(ラブモンスター)

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
			anime.SetBool ("monsterMove", true);
		} 
		// 上キーを話したら
		else if (Input.GetKeyUp ("up"))
		{
			// 待機状態へ
			anime.SetBool ("monsterMove",false );
		}

		if (Input.GetKeyDown ("e")) 
		{
			// ダウンモーションへ
			anime.SetTrigger("monsterDown");
		}

		if (Input.GetKeyDown ("r")) 
		{
			// 召喚モーションへ
			anime.SetTrigger("monsterTransform");
		}

		if (Input.GetKeyDown ("g"))
		{
			// 弱攻撃モーションへ
            anime.SetTrigger("monsterWeakAttack");
		}
		else if (Input.GetKeyDown ("h"))
		{
			// 強攻撃モーションへ
            anime.SetTrigger("monsterStrAttack");
		}

		if (Input.GetKeyDown ("b")) 
		{
			// ダメージモーションへ
			anime.SetTrigger("monsterDamage");
		}

		if (Input.GetKeyDown ("v")) 
		{
			// 消滅モーションへ
			anime.SetTrigger("monsterSplit");
		}

	}
}
