using UnityEngine;
using System.Collections;

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

	void Start() {
//		switch (playerNum) {
//		case 4:
//			row = 23;
//			col = 23;
//			mahojinsPos [0] = new Vector2 (3, 3);
//			mahojinsPos [1] = new Vector2 (3, 3 + 8);
//			mahojinsPos [2] = new Vector2 (3, 3 + 8 * 2);
//			mahojinsPos [3] = new Vector2 (3 + 8, 3);
//			mahojinsPos [4] = new Vector2 (3 + 8, 3 + 8);
//			mahojinsPos [5] = new Vector2 (3 + 8, 3 + 8 * 2);
//			mahojinsPos [6] = new Vector2 (3 + 8 * 2, 3);
//			mahojinsPos [7] = new Vector2 (3 + 8 * 2, 3 + 8);
//			mahojinsPos [8] = new Vector2 (3 + 8 * 2, 3 + 8 * 2);
//			break;
//		}


		panels = new int[row,col];
		for (int x = 0; x < row; x++) {
			for (int y = 0; y < col; y++) {
				// プレハブから生成.
				GameObject obj = (GameObject)Instantiate (prefab);
				// 配置.(y軸はとりあえず0にする : 地面)
				obj.transform.position = new Vector3 (x, -1.0f, y);
				obj.transform.parent = this.gameObject.transform;
				obj.GetComponent<MeshRenderer> ().material.color = Color.black;
				// ステージの状態を更新.
				panels[x,y] = (int)State.NONE;
			}
		}

		// 魔法陣を置く.
		for (int i = 0; i < mahojinsPos.Length; i++ ) {
			// 魔法陣ように空のオブジェクトを生成.
			GameObject mahojin = (GameObject)Instantiate(mahojinPrefab);
			// とりあえず、地面が0でscaleが1のcube上に表示するので,0.6上にあげている.
			mahojin.transform.position = new Vector3 ((int)mahojinsPos [i].x, 0.6f, (int)mahojinsPos [i].y);
			mahojin.transform.parent = this.gameObject.transform;
			// サイズは後で微調整.
			mahojin.transform.localScale = Vector3.one * 1.5f;
			// 魔法陣がわかるように,最後にナンバーを入れる.
			mahojin.name = "mahojin" + i;
			mahojin.AddComponent<MahojinController> ();
			// 魔法陣の位置をステージの配列に教える.
			SettingMahojin((int)mahojinsPos[i].x, (int)mahojinsPos[i].y);
		}
	}
		
	// キャラクターが乗っている場所の情報を入れる.
	void CharacterEnter(int x, int y) {
		// ステージの状態を更新.
		panels[x,y] = (int)State.ON_CHARACTER;
	}

	// キャラクターが乗っている場所を離れた情報を入れる.
	void CharacterExit(int x, int y) {
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
