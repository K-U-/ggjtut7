using UnityEngine;
using System.Collections;

public class SetCos : MonoBehaviour {
    public Sprite[] Head;
    public Sprite[] Eye;
    public Sprite[] Body;
    public Sprite[] Uarm;
    public Sprite[] Barm;
    public Sprite[] Leg;
    public Sprite[] Wepon;
    public SpriteRenderer HeadBase;
    public SpriteRenderer EyeBase;
    public SpriteRenderer BodyBase;
    public SpriteRenderer[] UarmBase;
    public SpriteRenderer[] BarmBase;
    public SpriteRenderer[] LegBase;
    public SpriteRenderer WeponBase;

    // Use this for initialization
    void Start () {
        //SetCharaCos(1, 1, 1, 1, 1, 1, 1);
		// SetCharaCos(Random.Range(0, Head.Length), Random.Range(0, Eye.Length), Random.Range(0, Body.Length), Random.Range(0, Uarm.Length), Random.Range(0, Barm.Length), Random.Range(0, Leg.Length), Random.Range(0, Wepon.Length));
    }

	public void GenerateRandomCostume(CharactorCos model) {
		model.HeadIndex = Random.Range (0, Head.Length);
		model.EyeIndex = Random.Range (0, Eye.Length);
		model.BodyIndex = Random.Range (0, Body.Length);
		model.UarmIndex = Random.Range (0, Uarm.Length);
		model.BarmIndex = Random.Range (0, Barm.Length);
		model.LegIndex = Random.Range (0, Leg.Length);
		model.WeponIndex = Random.Range (0, Wepon.Length);
	}

	public void SetCharaCos(CharactorCos model) {
		SetCharaCos (model.HeadIndex, model.EyeIndex, model.BodyIndex, model.UarmIndex,model.BarmIndex, model.LegIndex, model.WeponIndex);
	}

	// Update is called once per frame
	void SetCharaCos (int _H,int _E,int _B,int _Ua,int _Ba,int _L,int _W) {
        HeadBase.sprite = Head[_H];
        EyeBase.sprite = Eye[_E];
        BodyBase.sprite = Body[_B];
        UarmBase[0].sprite = Uarm[_Ua];
        UarmBase[1].sprite = Uarm[_Ua];
        BarmBase[0].sprite = Barm[_Ba];
        BarmBase[1].sprite = Barm[_Ba];
        LegBase[0].sprite = Leg[_L];
        LegBase[1].sprite = Leg[_L];
        //wepon表示設定
        if (_W == 0) WeponBase.enabled = false;
        else WeponBase.enabled = true;
        WeponBase.sprite = Wepon[_W];
    }
}
