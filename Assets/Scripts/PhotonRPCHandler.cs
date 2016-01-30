using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PhotonRPCCommand
{
    Move,
    Kill
}

public class PhotonRPCModel : ModelBase
{
    public string message;
    public string senderId;
    public PhotonRPCCommand command;
}

public class PhotonRPCHandler : Photon.MonoBehaviour{

    
    private Dictionary<PhotonRPCCommand, System.Action<PhotonRPCModel>> RPCCommandDictionary = new Dictionary<PhotonRPCCommand, System.Action<PhotonRPCModel>>
    {
        {PhotonRPCCommand.Move,OnRecieveMove},
        {PhotonRPCCommand.Kill,OnRecieveKill}
    };

    [SerializeField]
    private PhotonView photonView;

    public void PostRPC(PhotonRPCModel model)
    {
        photonView.RPC("HandleRPC", PhotonTargets.All, JsonUtility.ToJson(model));
    }

    [PunRPC]
    void HandleRPC(string message)
    {
        PhotonRPCModel model = JsonUtility.FromJson<PhotonRPCModel>(message);
        if (RPCCommandDictionary.ContainsKey(model.command))
        {
            RPCCommandDictionary[model.command](model);
        }
    }

#region RPCCommands

    /// <summary>
    /// Moveのコマンドを受信したとき
    /// </summary>
    /// <param name="model"></param>
    static void OnRecieveMove(PhotonRPCModel model){
        Debug.Log("OnRecieveMove from " + model.senderId);
    }

    /// <summary>
    /// Killのコマンドを受信したとき
    /// </summary>
    /// <param name="model"></param>
    static void OnRecieveKill(PhotonRPCModel model){
        Debug.Log("OnrecieveKill from " + model.senderId);
    }
#endregion //RPCCommands
}
