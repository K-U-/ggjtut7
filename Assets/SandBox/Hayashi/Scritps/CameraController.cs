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
        target = GameObject.Find(GameManager.GetInstance().myInfo.id.ToString()).gameObject.transform;

		// 決め打ちすみません.
		offset = new Vector3 (0.0f, -8.0f, 3.5f);
		transform.rotation = Quaternion.Euler (70.0f, 0.0f, 0.0f);

		AudioController.PlayBGM ("ggjPlay");
	}
		
	void Update() {
		// これがUpdateにあるのもどうかと考えてます.
		// 十字キーのアクションがあった時に一緒に動くようにしてもらうとか？
		transform.position = target.position - offset;

		if (Input.GetKeyDown(KeyCode.A)) {
			AudioController.PlaySE ("Mahojin1");
		}
	}
}
