using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class monster : TrueSyncBehaviour {

	private const byte INPUT_KEY_FORWARD = 0;	// 前移動
	private const byte INPUT_KEY_BACK = 1;		// 後移動
	private const byte INPUT_KEY_RIGHT = 2;     // 右移動
	private const byte INPUT_KEY_LEFT = 3;      // 左移動
	private const byte INPUT_KEY_ATTACK_A = 4;	// 弱攻撃
	private const byte INPUT_KEY_ATTACK_B = 5;	// 強攻撃
	private const byte INPUT_KEY_BUTTON = 6;	// ボタン

	private TSRigidBody rb = null;		// リジット
	private Animator anime;				//アニメーター
	private TSVector directionVector = TSVector.zero;

	public float 	speed;					// 速度
	public int   	attackPower;			// 攻撃力


	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	// 初期化
	public override void OnSyncedStart()
	{
		anime = GetComponent<Animator> ();	//アニメーションの取得
		rb = GetComponent<TSRigidBody>();	// RigidBodyの取得
	}

	// インプット
	public override void OnSyncedInput()
	{
		bool forward = Input.GetKey(KeyCode.W);
		bool back = Input.GetKey(KeyCode.S);
		bool right = Input.GetKey(KeyCode.D);
		bool left = Input.GetKey(KeyCode.A);
		bool attackA = Input.GetKeyDown (KeyCode.G);
		bool attackB = Input.GetKeyDown (KeyCode.H);

		TrueSyncInput.SetBool(INPUT_KEY_FORWARD, forward);
		TrueSyncInput.SetBool(INPUT_KEY_BACK, back);
		TrueSyncInput.SetBool(INPUT_KEY_RIGHT, right);
		TrueSyncInput.SetBool(INPUT_KEY_LEFT, left);
		TrueSyncInput.SetBool (INPUT_KEY_ATTACK_A, attackA);
		TrueSyncInput.SetBool (INPUT_KEY_ATTACK_B, attackB);
	}

	// 更新
	public override void OnSyncedUpdate()
	{
		bool forward = TrueSyncInput.GetBool(INPUT_KEY_FORWARD);
		bool back = TrueSyncInput.GetBool(INPUT_KEY_BACK);
		bool right = TrueSyncInput.GetBool(INPUT_KEY_RIGHT);
		bool left = TrueSyncInput.GetBool(INPUT_KEY_LEFT);
		bool attackA = TrueSyncInput.GetBool (INPUT_KEY_ATTACK_A);
		bool attackB = TrueSyncInput.GetBool (INPUT_KEY_ATTACK_B);

		TSVector vector = TSVector.zero;
		if (forward)
		{
			directionVector = vector += TSVector.forward;
			// 移動モーション
			anime.SetBool ("monsterMove", true);
		}
		if (back)
		{
			directionVector = vector += TSVector.back;
		}
		if (left)
		{
			directionVector = vector += TSVector.left;
		}
		if (right)
		{
			directionVector = vector += TSVector.right;
		}

		// 攻撃中は移動不可
		if( attackA)
		{
			// 弱攻撃モーションへ
			anime.SetTrigger("monsterAttack_A");
		}
		TSVector.Normalize(vector);

		// addForceで移動量を加算
	//	rb.AddForce(speed * vector, ForceMode.Force);
		rb.position += speed * vector;
		Debug.Log (speed);

		// 角度を設定する
		//FP direction = TSMath.Atan2(directionVector.x, directionVector.z) * TSMath.Rad2Deg;
		//transform.rotation = Quaternion.Euler(0.0f, (float)direction, 0.0f);
	}
}
