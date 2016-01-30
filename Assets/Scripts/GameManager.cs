using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Photon.MonoBehaviour {

    void Awake()
    {
        Application.targetFrameRate = 30;

        // Photonへの接続を行う
        PhotonNetwork.ConnectUsingSettings("0.1");

        // PhotonNetworkの更新回数のセット
        PhotonNetwork.sendRate = 30;

        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public virtual void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinOrCreateRoom("HogeHoge", new RoomOptions() { maxPlayers = 8 }, null);
    }

    void InitializeStage()
    {

    }

    void InitializePlayer()
    {

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PostPlayerStatus()
    {
        Room room = PhotonNetwork.room;
    }
    
}
