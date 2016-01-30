using UnityEngine;
using System.Collections;


[System.Serializable]
public class MoveCommandSample
{
    public string target;
    public float offset;
}

[System.Serializable]
public class KillCommandSample
{
    public string target;
}
public class EventSample : MonoBehaviour {

    void Awake()
    {
        PhotonRPCHandler.moveEvent += MoveEvent;
        PhotonRPCHandler.killEvent += KillEvent;
    }

    void OnDestroy()
    {
        PhotonRPCHandler.moveEvent -= MoveEvent;
        PhotonRPCHandler.killEvent -= KillEvent;
    }

    private void MoveEvent(PhotonRPCModel model)
    {
        MoveCommandSample command = JsonUtility.FromJson<MoveCommandSample>( model.message );
        if (command.target == gameObject.name)
        {
            transform.position += Vector3.right * command.offset;
        }
    }

    private void KillEvent(PhotonRPCModel model)
    {
        KillCommandSample command = JsonUtility.FromJson<KillCommandSample>(model.message);
        if (command.target == gameObject.name)
        {
            Destroy(gameObject);
        }
    }
}
