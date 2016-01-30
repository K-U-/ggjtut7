using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;

// ステージコントローラー
public class StageController : MonoBehaviour {
	// ステージの状態.
	enum State {NONE, ON_CHARACTER, ON_MAHOJIN, } ;

	// ステージ画像を貼り付けたゲームオブジェクトのプレハブ.
	public GameObject prefab;
	// とりあえず,ステージの大きさを決める.
	public int row, col;
	// ステージの配列.
	public int [,] panels;
	// 魔法陣の位置.
	public Vector2[] mahojinsPos;
	// 魔法陣のプレバブ
	public GameObject mahojinPrefab;
	// プレイヤーの人数
	public int playerNum;
	// キャラクターのプレハブ.
	public GameObject characterPrefab;
	// テキストデータを持っている.
	public GameObject[] textAssets;
	// コスチュームを保持.
    private Dictionary<int, CharactorCos> playerCos = new Dictionary<int, CharactorCos>();
	// 外壁のプレハブ.
	public GameObject wallPrefab;

    IEnumerator SyncRoutine()
    {
        while (true)
        {
            PhotonRPCModel model = new PhotonRPCModel();
            model.command = PhotonRPCCommand.StartSync;
            model.senderId = gameObject.name;
            PhotonRPCHandler.GetInstance().PostRPC(model);
            yield return new WaitForSeconds(3.0f);
        }
    }

	void Start() {
        if (GameManager.GetInstance().myInfo.isHost)
        {
            StartCoroutine(SyncRoutine());
        }
		// GameManagerから準備のできているプレイヤーの数を取得する.
        playerNum = GameManager.GetInstance().ReadyStatusList.readyStatusList.Where(_ => !_.info.isSpector).ToList().Count;

		SetCos setCos = characterPrefab.GetComponent<SetCos> ();
		CharactorCos model = new CharactorCos ();
		// コスチュームを先に選択しておく.
		for (int i = 0; i < playerNum; i++) {
			setCos.GenerateRandomCostume (model);
			playerCos.Add (i, model);
		}

		// ステージの情報を引っ張ってくる.
		StringReader stringReader = new StringReader (textAssets [playerNum].GetComponent<StageData> ().mapSize.text);
		string[] str;
		while (stringReader.Peek () > -1) {
			// 列,行,魔法陣の数
			str = stringReader.ReadLine ().Split (',');
			row = int.Parse (str [0]);
			col = int.Parse (str [1]);
			mahojinsPos = new Vector2[int.Parse (str [2])];
		}
			
		panels = new int[row, col];
		for (int x = 0; x < row; x++) {
			for (int y = 0; y < col; y++) {
				// プレハブから生成.
				GameObject obj = (GameObject)Instantiate (prefab);
				// 配置.(y軸はとりあえず0にする : 地面)
				obj.transform.position = new Vector3 (x, 0.0f, y);
				obj.transform.parent = this.gameObject.transform;
				// obj.GetComponent<MeshRenderer> ().material.color = Color.black;
				// ステージの状態を更新.
				panels [x, y] = (int)State.NONE;
			}
		}

		// 外側に壁を敷き詰める.
		for (int x = -1; x < row + 1; x++) {
			GameObject obj1 = (GameObject)Instantiate(wallPrefab);
			obj1.transform.position = new Vector3 (x, 0.0f, -1.0f);
			GameObject obj2 = (GameObject)Instantiate(wallPrefab);
			obj2.transform.position = new Vector3 (x, 0.0f, col);
		}

		for (int y = -1; y < col + 1; y++) {
			GameObject obj1 = (GameObject)Instantiate(wallPrefab);
			obj1.transform.position = new Vector3 (-1.0f, 0.0f, y);
			GameObject obj2 = (GameObject)Instantiate(wallPrefab);
			obj2.transform.position = new Vector3 (row, 0.0f, y);
		}

		// 魔法陣の位置を引っ張ってくる.
		stringReader = new StringReader (textAssets [playerNum].GetComponent<StageData> ().mahojinPos.text);
		int mahojinIndex = 0;
		while (stringReader.Peek () > -1) {					
			str = stringReader.ReadLine ().Split (',');
			mahojinsPos [mahojinIndex].x = int.Parse (str [0]);
			mahojinsPos [mahojinIndex].y = int.Parse (str [1]);
			mahojinIndex++;
		}

		// 魔法陣を置く.
		for (int i = 0; i < mahojinsPos.Length; i++) {
			// 魔法陣ように空のオブジェクトを生成.
			GameObject mahojin = (GameObject)Instantiate (mahojinPrefab);
			// とりあえず、地面が0でscaleが1のcube上に表示するので,0.6上にあげている.
			mahojin.transform.position = new Vector3 ((int)mahojinsPos [i].x, 0.1f, (int)mahojinsPos [i].y);
			mahojin.transform.parent = this.gameObject.transform;
			// サイズは後で微調整.
			mahojin.transform.localScale = Vector3.one * 1.5f;
			// 魔法陣がわかるように,最後にナンバーを入れる.
			mahojin.name = "mahojin" + i;
			mahojin.AddComponent<MahojinController> ();
			// 魔法陣の位置をステージの配列に教える.
			SettingMahojin ((int)mahojinsPos [i].x, (int)mahojinsPos [i].y);
		}

		// キャラクターの生成位置を引っ張ってくる.
		stringReader = new StringReader (textAssets [playerNum].GetComponent<StageData> ().playerStartPos.text);
		int characterIndex = 0;
		while (stringReader.Peek () > -1) {
			str = stringReader.ReadLine ().Split (',');
			GameObject obj = (GameObject)Instantiate (characterPrefab);
			obj.transform.position = new Vector3 (int.Parse (str [0]), 1.0f, int.Parse (str [1]));
            if (GameManager.GetInstance().ReadyStatusList.readyStatusList.Count > characterIndex)
            {
				// あらかじめ作っておいたコスチュームを持ってくる.
				obj.GetComponent<SetCos>().SetCharaCos(this.playerCos[characterIndex]);
                obj.name = GameManager.GetInstance().ReadyStatusList.readyStatusList[characterIndex].info.id.ToString();
            }
            else
            {
                obj.name = "hoge";
            }
			characterIndex++;
			CharacterEnter (int.Parse (str [0]), int.Parse (str [1]));
		}
	}
		
	// キャラクターが乗っている場所の情報を入れる.
	public void CharacterEnter(int x, int y) {
		// ステージの状態を更新.
		panels[x,y] = (int)State.ON_CHARACTER;
	}

	// キャラクターが乗っている場所を離れた情報を入れる.
	public void CharacterExit(int x, int y) {
		// ステージの状態を更新.
		panels[x,y] = (int)State.NONE;
	}

	// 魔法陣の位置情報をステージの配列に入れ込む.
	void SettingMahojin(int x, int y) {
		for (int i = -1; i <=1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (panels[x + i,y + j] != null) {
					panels [x + i, y + j] = (int)State.ON_MAHOJIN;
				}
			}
		}
	}

    
}
