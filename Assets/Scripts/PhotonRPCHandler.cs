using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PhotonRPCCommand
{
    Join,
    UpdateReadyList,
    Move,
    Kill,
    StartSync,
    SyncPosition,
    Departure,
	Costume,
}

public class PhotonRPCModel : ModelBase
{
    public string message;
    public string senderId;
    public PhotonRPCCommand command;

}

public class PhotonRPCHandler : Photon.MonoBehaviour{

    private static PhotonRPCHandler sharedInstance;
    public delegate void OnRecieveEvent(PhotonRPCModel model);
    public static OnRecieveEvent moveEvent;
    public static OnRecieveEvent killEvent;
    public static OnRecieveEvent joinEvent;
    public static OnRecieveEvent updateReadyEvent;
    public static OnRecieveEvent startSyncEvent;
    public static OnRecieveEvent syncPositionEvent;
    public static OnRecieveEvent departureEvent;
	public static OnRecieveEvent costumeEvent;

    public static PhotonRPCHandler GetInstance()
    {
        return sharedInstance;
    }
    void Awake()
    {
        sharedInstance = this;
    }

    private Dictionary<PhotonRPCCommand, System.Action<PhotonRPCModel>> RPCCommandDictionary = new Dictionary<PhotonRPCCommand, System.Action<PhotonRPCModel>>
    {
        {PhotonRPCCommand.Move,OnMoveEvent},
        {PhotonRPCCommand.Kill,OnKillEvent},
        {PhotonRPCCommand.Join,OnJoinEvent},
        {PhotonRPCCommand.UpdateReadyList,OnUpdateReady},
        {PhotonRPCCommand.StartSync,OnStartSync},
        {PhotonRPCCommand.SyncPosition,OnSyncPosition},
        {PhotonRPCCommand.Departure,OnDepartureEvent},
		{PhotonRPCCommand.Costume, OnCostumeEvent}
    };

    private static void OnMoveEvent(PhotonRPCModel model)
    {
        moveEvent(model);
    }

    private static void OnKillEvent(PhotonRPCModel model)
    {
        killEvent(model);
    }

    private static void OnJoinEvent(PhotonRPCModel model)
    {
        joinEvent(model);
    }

    private static void OnUpdateReady(PhotonRPCModel model)
    {
        updateReadyEvent(model);
    }

    private static void OnStartSync(PhotonRPCModel model)
    {
        if (startSyncEvent != null)
        {
            startSyncEvent(model);
        }
    }

    private static void OnSyncPosition(PhotonRPCModel model)
    {
        if (syncPositionEvent != null)
        {
            syncPositionEvent(model);
        }
    }

    private static void OnDepartureEvent(PhotonRPCModel model)
    {
        departureEvent(model);
    }

	private static void OnCostumeEvent(PhotonRPCModel model)
	{
		costumeEvent (model);
	}


    [SerializeField]
    private PhotonView photonView;

    public void PostRPC(PhotonRPCModel model)
    {
        photonView.RPC("HandleRPC", PhotonTargets.All, JsonUtility.ToJson(model));
    }

    [PunRPC]
    void HandleRPC(string message)
    {
        Debug.Log(message);
        PhotonRPCModel model = JsonUtility.FromJson<PhotonRPCModel>(message);
        if (RPCCommandDictionary.ContainsKey(model.command))
        {
            if (RPCCommandDictionary[model.command] != null)
            {
                RPCCommandDictionary[model.command](model);
            }
        }
    }

}
