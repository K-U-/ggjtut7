using UnityEngine;
using System.Collections;

// 魔法陣コントローラー.
public class MahojinController : MonoBehaviour {
	// 魔法陣は9マス分で一人しか入れない.

	// 魔法陣の番号
	private int id{ get{return id;} }

	// 魔法陣自身の居場所.
	private int x{ get{ return x;} }
	private int y{ get{ return y;} }

	// キャラクターが乗っているかどうか.
	private bool onCharacter;
	// 0~1の間でゲージが溜まる.
	[SerializeField,Range(0,1)]
	protected float controlGauge;
	// 魔法陣が光る為のスクリプト.
	private MagicCircleLight mCL;

	void Start() {
		// ゲーム開始時には,誰も乗っていない.
		onCharacter = false;

		// 自身のMCLを拾う.
		mCL = GetComponent<MagicCircleLight>();

		// 確認用.
		// GetComponent<MeshRenderer>().enabled = false;
	}

	void Update() {
		// 光はゲージの値で入れる.
		 mCL.LightState = controlGauge;
	}

	/// <summary>
	/// ゲージを貯める為の関数.
	/// </summary>
	/// <param name="value">Value : 増やす量.</param>
	public void AddGauge(float value) {
		controlGauge += value;
	}

	/// <summary>
	/// ゲージを減らす為の関数.
	/// </summary>
	/// <param name="value">Value : 減らす量.</param>

	public void subGauge(float value) {
		controlGauge -= value;
	}
}
