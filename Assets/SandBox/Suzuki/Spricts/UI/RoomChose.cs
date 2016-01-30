using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RoomChose : Photon.MonoBehaviour
{
    [SerializeField]
    private InputField inputField;  //InputFieldオブジェクトをアサイン
    [SerializeField]
    private GameObject mTarget;

    private bool mState;

	// Use this for initialization
	void Start () {
        //inputField = GetComponent<InputField>();
        //inputField.onEndEdit.AddListener( OnJoinedLobby );
        Application.targetFrameRate = 30;

        // Photonへの接続を行う
        mState = PhotonNetwork.ConnectUsingSettings("1");

        // PhotonNetworkの更新回数のセット
        PhotonNetwork.sendRate = 30;
	}

    private bool ConnectInUpdate = true;
    public bool AutoConnect = true;
    private void UpdateNetSetting()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(1 + "." + SceneManagerHelper.ActiveSceneBuildIndex);

        }

    }

    private void Update()
    {
        //if (mState)
        //{ GetComponent<InputField>().interactable = false; }
        //else GetComponent<InputField>().interactable = true;
        UpdateNetSetting();
    }

    public void OnJoinedLobby()
    {
        string name = inputField.text;
        UnityEngine.Debug.Log(name);
        PhotonNetwork.JoinRoom(name);
        mTarget.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPhotonRandomJoinFailed()
    {
        string name = inputField.text;
        UnityEngine.Debug.Log(name);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isVisible = true;
        roomOptions.isOpen = true;
        roomOptions.maxPlayers = 8;
        PhotonNetwork.CreateRoom(name, roomOptions, TypedLobby.Default);
    }

    //接続失敗時にコール
    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("参加失敗");
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
#if UNITY_EDITOR
        Application.LoadLevel("SuzukiDebug");
# else
        Application.LoadLevel("PCWindow");
# endif
    }

	void OnJoinedRoom()
    {
        Debug.Log("OnCreatedRoom");
        Room room = PhotonNetwork.room;
        Debug.Log(room.name);

        TargetActive();
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log(player.name);
        
    }

    public void TargetActive()
    {
        mTarget.SetActive(true);
        transform.gameObject.SetActive(false);
    }
	
	// Update is called once per fram
}
