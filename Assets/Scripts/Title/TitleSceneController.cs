using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleSceneController : Photon.MonoBehaviour {

    public GameObject loginObjects;
    public GameObject inputObjects;
    public GameObject inputUserName;
    public Text input;
    public Text inputName;
    private bool mState;
    private bool willHost;
    void Awake()
    {
        ShowUserName();
        Application.targetFrameRate = 30;
        mState = PhotonNetwork.ConnectUsingSettings("1");
        PhotonNetwork.sendRate = 30;
    }

    private void ShowInput()
    {
        loginObjects.SetActive(false);
        inputObjects.SetActive(true);
        inputUserName.SetActive(false);
    }

    private void ShowUserName()
    {
        loginObjects.SetActive(false);
        inputObjects.SetActive(false);
        inputUserName.SetActive(true);
    }

    private void ShowLogin()
    {
        loginObjects.SetActive(true);
        inputObjects.SetActive(false);
        inputUserName.SetActive(false);
    }

    public void OnClickStartHost()
    {
        willHost = true;
        GameManager.GetInstance().myInfo.isHost = true;
        ShowInput();
    }

    public void OnClickStartClient()
    {
        willHost = false;
        ShowInput();
    }

    public void ConfirmName()
    {
        GameManager.GetInstance().InitializePlayerInfo(inputName.text);
        ShowLogin();
    }
    public void OnClickOK()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isVisible = true;
        roomOptions.isOpen = true;
        roomOptions.maxPlayers = 8;
        GameManager.GetInstance().roomName = input.text;
        if (willHost)
        {
            PhotonNetwork.JoinOrCreateRoom(input.text, roomOptions, TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.JoinRoom(input.text);
        }

    }

    //接続失敗時にコール
    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("参加失敗");
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinRoom");
        Room room = PhotonNetwork.room;
        GameManager.GetInstance().InitializePlayerId(PhotonNetwork.player.ID);

        PlayerInfo info = new PlayerInfo();
        info.id = PhotonNetwork.player.ID;
        info.isHost = willHost;
        info.name = inputName.text;
        PhotonRPCModel model = new PhotonRPCModel();
        model.senderId = PhotonNetwork.player.ID.ToString();
        model.command = PhotonRPCCommand.Join;
        model.message = JsonUtility.ToJson(info);
        PhotonRPCHandler.GetInstance().PostRPC(model);
        GameManager.GetInstance().LoadScene("Lobby");
    }
}
