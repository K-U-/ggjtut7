using UnityEngine;
using System.Collections;

// 魔法陣コントローラー.
public class MahojinController : MonoBehaviour {
	// 魔法陣は9マス分で一人しか入れない.

	// 魔法陣の番号
	public int id;

	// 魔法陣自身の居場所.
	public int x;
	public int y;

	// キャラクターが乗っているかどうか.
	public bool onCharacter;
	// 0~1の間でゲージが溜まる.
	private int  controlGauge;
	// ゲージのマックス.
	[SerializeField]
	int maxGauge = 5;
	// 魔法陣が光る為のスクリプト.
	private MagicCircleLight mCL;
	// 音を鳴らすかどうかを決めるフラグ.
	private bool soundPlayed;
	// 加算するポイント.
	public int addValue;

	void Start() {
		// ゲーム開始時には,誰も乗っていない.
		onCharacter = false;

		// 自身のMCLを拾う.
		mCL = GetComponent<MagicCircleLight>();

		// ゲーム開始時には,音は鳴らせる状態.
		soundPlayed = false;

		this.x = (int)transform.position.x;
		this.y = (int)transform.position.z;

		this.id = int.Parse(name.Substring (name.Length-1));

		// 確認用.
		// GetComponent<MeshRenderer>().enabled = false;
	}

	void Update() {
		// 光はゲージの値で入れる.
		mCL.LightState = controlGauge / (float)maxGauge;
	}

	/// <summary>
	/// ゲージを貯める為の関数.
	/// </summary>
	/// <param name="value">Value : 増やす量.</param>
	public void AddGauge(int value) {
		controlGauge += value;
		if (controlGauge >= maxGauge) {
			controlGauge = maxGauge;
			if (soundPlayed == false) {
				// 音でお知らせ.
				AudioController.PlaySE ("Mahojin1");
				// ポイントを加算.
				soundPlayed = true;
			}
		}
	}

	/// <summary>
	/// ゲージを減らす為の関数.
	/// </summary>
	/// <param name="value">Value : 減らす量.</param>

	public void subGauge(int value) {
		controlGauge -= value;
		if (controlGauge <= 0) {
			controlGauge = 0;
			soundPlayed = false;
		}
	}
}
