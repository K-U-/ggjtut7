using UnityEngine;
using System.Linq;
using System.Collections;

public class CameraController : MonoBehaviour {
	// スマートフォン側.
	// キャラクターを追いかけるカメラコントローラー.

	// ターゲット.(プレイヤーキャラクター)
	public Transform target;
	// キャラクターとのオフセット.
	private Vector3 offset;

	void Start() {
        if (GameManager.GetInstance().myInfo.isSpector)
        {
            var list = GameManager.GetInstance().ReadyStatusList.readyStatusList.Where(_=>!_.info.isSpector).ToList();
            target = GameObject.Find(list[Random.Range(0,list.Count)].info.id.ToString()).transform;
            StartCoroutine(RandomRoutine());
        }
        else
        {
            target = GameObject.Find(GameManager.GetInstance().myInfo.id.ToString()).gameObject.transform;
        }

		// 決め打ちすみません.
		offset = new Vector3 (0.0f, -8.0f, 3.5f);
		transform.rotation = Quaternion.Euler (70.0f, 0.0f, 0.0f);

		AudioController.PlayBGM ("ggjPlay");
	}

    IEnumerator RandomRoutine()
    {
        while (true)
        {
            var list = GameManager.GetInstance().ReadyStatusList.readyStatusList.Where(_ => !_.info.isSpector).ToList();
            target = GameObject.Find(list[Random.Range(0, list.Count)].info.id.ToString()).transform;
            yield return new WaitForSeconds(5);
        }
    }
	void Update() {
		// これがUpdateにあるのもどうかと考えてます.
		// 十字キーのアクションがあった時に一緒に動くようにしてもらうとか？
		transform.position = Vector3.Lerp(transform.position,target.position - offset,Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.A)) {
			AudioController.PlaySE ("Mahojin1");
		}
	}
}
