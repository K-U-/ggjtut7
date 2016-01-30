using UnityEngine;
using System.Collections;

public class PhotonManager : Photon.MonoBehaviour {

    public string objectName;
    public string roomName;

    void Start()
    {
        Application.targetFrameRate = 30;

        // Photonへの接続を行う
        PhotonNetwork.ConnectUsingSettings("0.1");

        // PhotonNetworkの更新回数のセット
        PhotonNetwork.sendRate = 30;
    }


    public virtual void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { maxPlayers = 8 }, null);
    }

    /// <summary>
    /// ロビーに接続すると呼ばれるメソッド
    /// </summary>
    void OnJoinedLobby()
    {
        // ランダムでルームに入室する
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("rondom");
    }

    /// <summary>
    /// ランダムで部屋に入室できなかった場合呼ばれるメソッド
    /// </summary>
    void OnPhotonRandomJoinFailed()
    {
        // ルームを作成、部屋名は今回はnullに設定
        PhotonNetwork.CreateRoom(roomName);
        Debug.Log("Create");
    }

    /// <summary>
    /// ルームに入室成功した場合呼ばれるメソッド
    /// </summary>
    public void OnJoinedRoom()
    {
        GameObject cube = PhotonNetwork.Instantiate(objectName, Vector3.zero, Quaternion.identity, 0);
        Debug.Log("Can");
    }

    /// <summary>
    /// UnityのGameウィンドウに表示させる
    /// </summary>
    void OnGUI()
    {
        // Photonのステータスをラベルで表示させています
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        Rect rect1 = new Rect(10, 10, 400, 30);
        roomName = GUI.TextField(rect1, roomName, 16);
    }
}
