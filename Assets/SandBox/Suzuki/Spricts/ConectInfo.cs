using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ConectInfo : Photon.MonoBehaviour {

    private Text mText;

	// Use this for initialization
	void Start () {
        //mText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        
        //mText.text = PhotonNetwork.room.
	}

    [ContextMenu("Print")]
    void OnReceivedRoomListUpdate()
    {
        // 既存のRoomを取得.
        RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
        Debug.Log(roomInfo.Length);
        if (roomInfo == null || roomInfo.Length == 0) return;

        // 個々のRoomの名前を表示.
        for (int i = 0; i < roomInfo.Length; i++)
        {
            Debug.Log((i).ToString() + " : " + roomInfo[i].name);
        }
    }
}
