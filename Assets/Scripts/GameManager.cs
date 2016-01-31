using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ReadyCommand
{
    public PlayerInfo info;
}


public class GameManager : Photon.MonoBehaviour {

    public string roomName;
    private PlayerReadyStatusList readyStatusList;
    public PlayerReadyStatusList ReadyStatusList {
        get{return readyStatusList;}
    }
        
    private static GameManager sharedInstance;
    public static GameManager GetInstance()
    {
        return sharedInstance;
    }

    public PlayerInfo myInfo;

    private Dictionary<string, Character_Controller> characterDictionary = new Dictionary<string, Character_Controller>();

    public void InitializeCharacterController(string key, Character_Controller val)
    {
        characterDictionary.Add(key, val);
        Debug.Log(characterDictionary.Count);
    }

    void Awake()
    {
        readyStatusList = new PlayerReadyStatusList();
        readyStatusList.readyStatusList = new System.Collections.Generic.List<PlayerReadyStatus>();
        GameObject.DontDestroyOnLoad(gameObject);
        sharedInstance = this;
        myInfo = new PlayerInfo();
    }

    void Start()
    {
        PhotonRPCHandler.joinEvent += JoinUser;
        PhotonRPCHandler.updateReadyEvent += UpdateReadyList;
    }

    public void AttachInGameEvent()
    {
        PhotonRPCHandler.syncPositionEvent += OnSyncPositionEvent;
    }

    public void DetachInGameEvent()
    {
        PhotonRPCHandler.syncPositionEvent -= OnSyncPositionEvent;
    }

    public void OnSyncPositionEvent(PhotonRPCModel model)
    {
        SyncCommand command = JsonUtility.FromJson<SyncCommand>(model.message);
        if (characterDictionary.ContainsKey(model.senderId))
        {
            characterDictionary[model.senderId].RepositionPlayer(command);
        }
    }

    public void InitializePlayerInfo(string name)
    {
        myInfo.name = name;
    }

    public void InitializePlayerId(int id)
    {
        myInfo.id = id;
    }

    /// <summary>
    /// ユーザーが入ってきたとき
    /// </summary>
    /// <param name="rpcmodel"></param>
    public void JoinUser(PhotonRPCModel rpcmodel)
    {
        PlayerInfo info = JsonUtility.FromJson<PlayerInfo>(rpcmodel.message);
        if (myInfo.isHost)
        {
            PlayerReadyStatus readyStatus = new PlayerReadyStatus();
            readyStatus.info = info;
            readyStatus.isReady = false;
            readyStatusList.readyStatusList.Add(readyStatus);

            PhotonRPCModel model = new PhotonRPCModel();
            model.message = JsonUtility.ToJson(readyStatusList);
            model.senderId = myInfo.id.ToString();
            model.command = PhotonRPCCommand.UpdateReadyList;
            PhotonRPCHandler.GetInstance().PostRPC(model);
        }
    }

    public void CheckUserList()
    {
        if (this.readyStatusList.readyStatusList.Count != PhotonNetwork.playerList.Length)
        {
            foreach (PlayerReadyStatus player in this.readyStatusList.readyStatusList)
            {
                if (!PhotonNetwork.playerList.Any(_ => _.ID == player.info.id))
                {
                    this.readyStatusList.readyStatusList.Remove(player);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// ReadyListが更新されたとき
    /// </summary>
    /// <param name="rpcModel"></param>
    public void UpdateReadyList(PhotonRPCModel rpcModel)
    {
        PlayerReadyStatusList list = JsonUtility.FromJson<PlayerReadyStatusList>(rpcModel.message);
        this.readyStatusList = list;
        foreach (var prof in list.readyStatusList)
        {
            if (prof.info.id == GameManager.GetInstance().myInfo.id)
            {
                GameManager.GetInstance().myInfo.isHuman = prof.info.isHuman;
            }
        }
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
