using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

	// SE
	public AudioClip[] seList;
	// SE記録
	public static Dictionary<string, AudioClip> seDic;
	private static AudioSource seSource;
	// 子オブジェクトのAudioSourceを使う.
	public GameObject seChild;

	// BGM
	public AudioClip[] bgmList;
	// BGM記録
	public static Dictionary<string, AudioClip> bgmDic;
	private static AudioSource bgmSource;
	// 子オブジェクトのAudioSourceを使う.
	public GameObject bgmChild;

	public void Awake() {
		DontDestroyOnLoad (this);
	}

	public void Start() {

		seDic = new Dictionary<string, AudioClip> ();

		// 名前から引いてこれるように入れておく.
		for (int i = 0; i < seList.Length; i++) {
			seDic.Add (seList[i].name, seList[i]);
		}

		bgmDic = new Dictionary<string, AudioClip> ();

		// 名前から引いてこれるように入れておく.
		for (int i = 0; i < bgmList.Length; i++) {
			bgmDic.Add (bgmList[i].name, bgmList[i]);
		}

		seSource = seChild.GetComponent<AudioSource> ();
		bgmSource = bgmChild.GetComponent<AudioSource>();
		bgmSource.loop = true;

		PlayBGM ("ggjTitle");
	}

	public static void PlaySE(string name) {
		seSource.PlayOneShot (seDic[name],0.1f);
	}

	public static void PlayBGM(string name) {
		// もし、BGMがなっていたら止める
		StopBGM();
		bgmSource.clip = bgmDic [name];
		bgmSource.Play ();
	}

	public static void StopSE() {
		if (seSource.isPlaying) {
			seSource.Stop ();
		}
	}

	public static void StopBGM() {
		if (bgmSource.isPlaying == true) {
			bgmSource.Stop ();
		}
	}

}
