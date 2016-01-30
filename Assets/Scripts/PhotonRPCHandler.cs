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
    ActionTickerEvent
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
    public static OnRecieveEvent actionTickerEvent;

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
        {PhotonRPCCommand.Departure,OnDepartureEvent},
        {PhotonRPCCommand.ActionTickerEvent,OnActionTickerEvent}
    };

    private static void OnMoveEvent(PhotonRPCModel model)
    {
        moveEvent(model);
    }

    private static void OnKillEvent(PhotonRPCModel model)
    {
        killEvent(model);
        PhotonRPCModel tickermodel = new PhotonRPCModel();
        tickermodel.senderId = model.senderId;
        tickermodel.command = PhotonRPCCommand.ActionTickerEvent;
        KillCommand com = JsonUtility.FromJson<KillCommand>(model.message);
        tickermodel.message = string.Format("{0}が{1}を KILL!", model.senderId, com.target);
        sharedInstance.PostRPC(tickermodel);
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
    private static void OnActionTickerEvent(PhotonRPCModel model){
        if(actionTickerEvent != null){
            actionTickerEvent(model);
        }
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
