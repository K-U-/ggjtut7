using UnityEngine;
using System.Collections;


[System.Serializable]
public class MoveCommand
{
    public string target;
    public float offsetX;
    public float offsetZ;
}

[System.Serializable]
public class KillCommand
{
    public string target;
}

public class Character_Controller : MonoBehaviour{

    StageController stage;

    void Awake()
    {
        PhotonRPCHandler.moveEvent += MoveEvent;
        PhotonRPCHandler.killEvent += KillEvent;

        stage = GameObject.Find("Stage").GetComponent<StageController>(); ;
    }

    void OnDestroy()
    {
        PhotonRPCHandler.moveEvent -= MoveEvent;
        PhotonRPCHandler.killEvent -= KillEvent;
    }

    private void MoveEvent(PhotonRPCModel model)
    {
        MoveCommand command = JsonUtility.FromJson<MoveCommand>(model.message);
        if (command.target == gameObject.name)
        {
            Vector3 pos;
            pos = transform.position;
            pos += Vector3.right * command.offsetX;
            pos += Vector3.forward * command.offsetZ;
            if (command.offsetX < 0) {
                if (this.transform.position.x > 0) {
                    search(pos, command);
                }
            }

            if (command.offsetX > 0)
            {
                if (this.transform.position.x < stage.row - 1)
                {
                    search(pos, command);
                }
            }

            if (command.offsetZ < 0)
            {
                if (this.transform.position.z > 0)
                {
                    search(pos, command);
                }
            }

            if (command.offsetZ > 0)
            {
                if (this.transform.position.z < stage.col - 1)
                {
                    search(pos, command);
                }
            }
        }
    }

    private void search(Vector3 pos, MoveCommand command) {
        Debug.Log(stage.panels[(int)pos.x, (int)pos.z]);
        if (stage.panels[(int)pos.x, (int)pos.z] == 0 && stage.panels[(int)pos.x, (int)pos.z] == 2)
        {
            stage.CharacterExit((int)this.transform.position.x, (int)this.transform.position.z);
            //Debug.Log("x = " + (int)this.transform.position.x + "\r\nz = " + (int)this.transform.position.z);
            transform.position += Vector3.right * command.offsetX;
            transform.position += Vector3.forward * command.offsetZ;
            stage.CharacterEnter((int)this.transform.position.x, (int)this.transform.position.z);
            //Debug.Log("x = " + (int)this.transform.position.x + "\r\nz = " + (int)this.transform.position.z);
        }
    }

    private void KillEvent(PhotonRPCModel model)
    {
        KillCommand command = JsonUtility.FromJson<KillCommand>(model.message);
        if (command.target == gameObject.name)
        {
            Destroy(gameObject);
        }
    }
}