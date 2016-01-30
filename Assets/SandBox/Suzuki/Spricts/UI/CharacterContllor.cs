using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterContllor : MonoBehaviour {

    private PhotonView mView;
    private enum KeyState
    {
        STATE_NULL,
        STATE_RIGHT,
        STATE_DOWN,
        STATE_LEFT,
        STATE_UP
    }
    private KeyState mKeyState;
    private Text mText;

	// Use this for initialization
	void Start () {
        AttachKeyCommand();
	}

    public void Search()
    {
        mText = transform.FindChild("Log").GetComponent<Text>();

    }

    private void AttachKeyCommand()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(InputUP);
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(InputDown);
    }

    private void InputUP()
    {
        if (!mView.isMine) return;
        UnityEngine.Debug.Log("Push");
        mKeyState = KeyState.STATE_UP;
        mText.text = mKeyState.ToString();
    }

    private void InputDown()
    {
        if (!mView.isMine) return;
        mKeyState = KeyState.STATE_DOWN;
        mText.text = mKeyState.ToString();
    }

}
