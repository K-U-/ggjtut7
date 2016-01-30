using UnityEngine;
using System.Collections;

public class ArrowEvent : MonoBehaviour {

    MoveCommand sampleCommand;

    void Awake()
    {
        sampleCommand = new MoveCommand();
    }

    public string PlayerID;
    public int MaxNum;

    void move(string id, float x, float z) {
        sampleCommand.target = id;
        sampleCommand.offsetX = x;
        sampleCommand.offsetZ = z;
        PhotonRPCModel model = new PhotonRPCModel();
        model.senderId = "AAA";
        model.command = PhotonRPCCommand.Move;
        model.message = JsonUtility.ToJson(sampleCommand);
        GetComponent<PhotonRPCHandler>().PostRPC(model);
    }

    #region 矢印キーの取得
    public void MoveUp()
    {
        move(PlayerID, 0f, 1f);
    }

    public void MoveDown()
    {
        move(PlayerID, 0f, -1f);
    }

    public void MoveRight()
    {
        move(PlayerID, 1f, 0f);
    }

    public void MoveLeft()
    {
        move(PlayerID, -1f, 0f);
    }
#endregion
}
