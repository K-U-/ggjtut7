using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	// スマートフォン側.
	// キャラクターを追いかけるカメラコントローラー.

	// ターゲット.(プレイヤーキャラクター)
	public Transform target;
	// キャラクターとのオフセット.
	private Vector3 offset;

	void Start() {
		// GameManagerさん,私は誰のカメラなのでしょうか!

		// 決め打ちすみません.
		offset = new Vector3 (0.0f, -7.0f, 3.5f);
		transform.rotation = Quaternion.Euler (70.0f, 0.0f, 0.0f);
	}

	void Update() {
		// これがUpdateにあるのもどうかと考えてます.
		// 十字キーのアクションがあった時に一緒に動くようにしてもらうとか？
		transform.position = target.position - offset;
	}
}
