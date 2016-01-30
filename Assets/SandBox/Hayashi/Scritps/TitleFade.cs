using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleFade : MonoBehaviour {
	// タイトルロゴ
	public Image titleLogo;

	float time = 3.0f;
	float startTime;

	void Start() {
		startTime = Time.timeSinceLevelLoad;
	}

	void Update() {
		float diff = Time.timeSinceLevelLoad - startTime;
		
		if (diff > time) {
			titleLogo.fillAmount = 1.0f;

			enabled = false;
		}

		float rate = diff / time;

		titleLogo.fillAmount = Mathf.Lerp (0, 1, rate);

	}

}
