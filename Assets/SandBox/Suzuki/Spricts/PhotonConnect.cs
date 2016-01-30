using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PhotonConnect : Photon.MonoBehaviour
{

    [SerializeField]
    private string mRoomName;

    [SerializeField]
    private Text mText;

    void Start()
    {
        Application.targetFrameRate = 30;

        // Photonへの接続を行う
        PhotonNetwork.ConnectUsingSettings("1");

        // PhotonNetworkの更新回数のセット
        PhotonNetwork.sendRate = 30;

# if UNITY_EDITOR
        Invoke("OnPhotonRandomJoinFailed",3.0f);
# else
        Invoke("OnJoinedLobby", 3.0f);
        Invoke("OnReceivedRoomListUpdate", 5.0f);
# endif
    }

    /// <summary>
    /// ロビーに接続すると呼ばれるメソッド
    /// </summary>
    
    [ContextMenu("Joine")]
    void OnJoinedLobby()
    {
        // ランダムでルームに入室する
        //PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinRoom(mRoomName);
        Debug.Log("rondom");
    }

    /// <summary>
    /// ランダムで部屋に入室できなかった場合呼ばれるメソッド
    /// </summary>

    [ContextMenu("Create")]
    void OnPhotonRandomJoinFailed()
    {
        // ルームを作成、部屋名は今回はnullに設定
        //PhotonNetwork.CreateRoom(null);
        //PhotonNetwork.CreateRoom(mRoomName, new RoomOptions() { maxPlayers = 4 }, null);
        Debug.Log("Create");
        RoomOptions roomOptions = new RoomOptions ();
	    roomOptions.isVisible = true;
	    roomOptions.isOpen = true;
	    roomOptions.maxPlayers = 4;
        //roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "CustomProperties", 0000 } };
        //roomOptions.customRoomPropertiesForLobby = new string[] { "CustomProperties" };
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        PhotonNetwork.CreateRoom(mRoomName, roomOptions, TypedLobby.Default);
        
    }





    private bool ConnectInUpdate = true;
    public bool AutoConnect = true;
    private void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(1 + "." + SceneManagerHelper.ActiveSceneBuildIndex);

        }
        //OnReceivedRoomListUpdate();


    }

    // カスタムプロパティを一時保存する
    private string text = "";


    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }
    //部屋作成に成功したときにコール
    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        Room room = PhotonNetwork.room;
        Debug.Log(room.name);
    }
    //接続が切断されたときにコール
    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
    }
    //接続失敗時にコール
    public void OnFailedToConnectToPhoton(object parameters)
    {

        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log(player.name);
    }



    [ContextMenu("Print")]
    public void OnReceivedRoomListUpdate()
    {
        // 既存のRoomを取得.
        RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
        Debug.Log(roomInfo.Length);
        mText.text = roomInfo.Length.ToString(); ;
        if (roomInfo == null || roomInfo.Length == 0) return;
        Debug.Log(roomInfo.GetValue(0));



        //customProperties

        // 個々のRoomの名前を表示.
        for (int i = 0; i < roomInfo.Length; i++)
        {
            Debug.Log((i).ToString() + " : " + roomInfo[i].name);
        }
    }
}
