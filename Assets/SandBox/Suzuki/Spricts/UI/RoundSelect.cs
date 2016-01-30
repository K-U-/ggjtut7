using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RoundSelect : MonoBehaviour {

    private InputField inputField;  //InputFieldオブジェクトをアサイン
	// Use this for initialization
	void Start () {
        inputField = GetComponent<InputField>();
        inputField.onEndEdit.AddListener(SendRound);
	}
	
    private void SendRound(string round)
    {
        Debug.Log(round);
    }

}
