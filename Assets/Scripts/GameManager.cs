using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ReadyCommand
{
    public PlayerInfo info;
}


public class GameManager : Photon.MonoBehaviour {

    public int roundCount = 3;
    public string roomName;
    public Image mask;
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
        PhotonRPCHandler.timeOverEvent += OnTimeOver;
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

    public void ClearCharacterDictionary()
    {
        characterDictionary.Clear();
    }
    public void AddCharacterController(string id, Character_Controller controller)
    {
        characterDictionary.Add(id, controller);
    }
    public Character_Controller GetCharacterControllerById(string id)
    {
        return characterDictionary[id];
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

    public void PostUpdateReadyRPC()
    {
        PhotonRPCModel updateModel = new PhotonRPCModel();
        updateModel.command = PhotonRPCCommand.UpdateReadyList;
        updateModel.senderId = myInfo.id.ToString();
        updateModel.message = JsonUtility.ToJson(ReadyStatusList);
        PhotonRPCHandler.GetInstance().PostRPC(updateModel);
    }

    /// <summary>
    /// 時間切れで次に遷移するかもしれない
    /// </summary>
    /// <param name="model"></param>
    public void OnTimeOver(PhotonRPCModel model)
    {
        LoadScene("Field");
        roundCount--;

        if (roundCount == 0)
        {
            //最終リザルト
        }
        else
        {
            //ラウンドリザルト
        }
    }

    public void MaskTo(float alpha , float time,System.Action completeAction = null)
    {
        mask.gameObject.SetActive(true);
        this.completeAction = completeAction;
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", mask.color.a,
            "to", alpha,
            "time", time,
            "easetime", iTween.EaseType.easeInCirc,
            "onupdate","UpdateValue",
            "oncomplete","OnCompleteAction"
            ));
    }

    public void MaskOff( float time,System.Action completeAction = null)
    {
        mask.gameObject.SetActive(true);
        this.completeAction = completeAction;
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", mask.color.a,
            "to", 0,
            "time", time,
            "easetime", iTween.EaseType.easeInCirc,
            "onupdate", "UpdateValue",
            "oncomplete", "OnCompleteAction"
            ));
    }

    void UpdateValue(float val)
    {
        mask.color = new Color(0, 0, 0, val);
        mask.SetAllDirty();
    }

    private System.Action completeAction;
    void OnCompleteAction()
    {
        mask.gameObject.SetActive(mask.color.a > 0);
        if(completeAction != null){
            completeAction();
        }

        completeAction = null;
    }

    public void LoadScene(string sceneName)
    {
        this.MaskTo(1, 0.5f, delegate
        {
            SceneManager.LoadScene(sceneName);
            this.MaskOff(0.5f);
        });
        
    }

    public void PostPlayerStatus()
    {
        Room room = PhotonNetwork.room;
    }
    
}
