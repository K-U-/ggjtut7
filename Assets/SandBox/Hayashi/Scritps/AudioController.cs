using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

	// SE
	public AudioClip[] seList;
	// SE記録
	public static Dictionary<string, AudioClip> seDic;
	private static AudioSource seSource;

	// BGM
	public AudioClip[] bgmList;
	// BGM記録
	public static Dictionary<string, AudioClip> bgmDic;
	private static AudioSource bgmSource;

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

		seSource = GetComponent<AudioSource> ();
		bgmSource = GetComponent<AudioSource>();
		bgmSource.loop = true;
	}

	public static void PlaySE(string name) {
		seSource.PlayOneShot (seDic[name]);
	}

	public static void PlayBGM(string name) {
		// もし、BGMがなっていたら止める
		if (bgmSource.isPlaying == true) {
			bgmSource.Stop ();
		}
		bgmSource.clip = bgmDic [name];
		bgmSource.Play ();
	}

}
