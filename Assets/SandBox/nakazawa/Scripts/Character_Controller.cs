using UnityEngine;
using System.Linq;
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

[System.Serializable]
public class SyncCommand
{
    public int x;
    public int z;
}

public class Character_Controller : MonoBehaviour{

    StageController stage;
    public GameObject Effect;
    public Animator anim;
    public float watetime;

    void Awake()
    {
        PhotonRPCHandler.moveEvent += MoveEvent;
        PhotonRPCHandler.killEvent += KillEvent;
        if (GameManager.GetInstance().myInfo.id.ToString() == gameObject.name)
        {
            PhotonRPCHandler.startSyncEvent += StartSyncEvent;
        }
        else
        {
            PhotonRPCHandler.startSyncEvent += DummySyncEvent;
        }
        stage = GameObject.Find("Stage").GetComponent<StageController>(); ;
    }

    void OnDestroy()
    {
        PhotonRPCHandler.moveEvent -= MoveEvent;
        PhotonRPCHandler.killEvent -= KillEvent;
        if (GameManager.GetInstance().myInfo.id.ToString() == gameObject.name)
        {
            PhotonRPCHandler.startSyncEvent -= StartSyncEvent;
        }
        else
        {
            PhotonRPCHandler.startSyncEvent -= DummySyncEvent;
        }
    }
    private void DummySyncEvent(PhotonRPCModel model)
    {
        return;
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
        //Debug.Log(stage.panels[(int)pos.x, (int)pos.z] + ":" + pos + ":");
        if (stage.panels[(int)pos.x, (int)pos.z] == 0 || stage.panels[(int)pos.x, (int)pos.z] == 2)
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
            Instantiate(Effect, transform.position, transform.rotation);
            StartCoroutine(efect());
            PhotonRPCModel tickermodel = new PhotonRPCModel();
            tickermodel.senderId = model.senderId;
            tickermodel.command = PhotonRPCCommand.ActionTickerEvent;
            KillCommand com = JsonUtility.FromJson<KillCommand>(model.message);
            tickermodel.message = string.Format("{0}が{1}を KILL!", model.senderId, com.target);

            PlayerInfo killer = null;
            PlayerInfo victim = null;
            foreach (var obj in GameManager.GetInstance().ReadyStatusList.readyStatusList)
            {
                if (obj.info.id.ToString() == model.senderId)
                {
                    killer = obj.info;
                }

                if (obj.info.id.ToString() == com.target)
                {
                    victim = obj.info;
                }
            }

            if (GameManager.GetInstance().myInfo.isHost)
            {
                CalcPointByKill(killer, victim);
            }

            PhotonRPCHandler.GetInstance().PostRPC(tickermodel);
            //Destroy(gameObject);
        }
    }

    private void CalcPointByKill(PlayerInfo killer, PlayerInfo victim)
    {
        if (killer == null || victim == null)
        {
            return;
        }

        if (victim.isHuman)
        {
            killer.point += GameManager.KillHumanPoint;
            victim.point += GameManager.HumanKilledPoint;
        }
        else
        {
            killer.point += GameManager.KillDevilPoint;
            victim.point += GameManager.DevilKilledPoint;
        }
        killer.point = Mathf.Max(killer.point, 0);
        victim.point = Mathf.Max(victim.point, 0);

        GameManager.GetInstance().PostUpdateReadyRPC();
    }

    IEnumerator efect() {
        anim.SetBool("Damage", true);
        yield return new WaitForSeconds(watetime);
        anim.SetBool("Damage", false);
    }

    private void StartSyncEvent(PhotonRPCModel model)
    {
        SyncCommand command = new SyncCommand();
        command.x = (int)transform.position.x;
        command.z = (int)transform.position.z;
        PhotonRPCModel rpcmodel = new PhotonRPCModel();
        rpcmodel.command = PhotonRPCCommand.SyncPosition;
        rpcmodel.senderId = gameObject.name;
        rpcmodel.message = JsonUtility.ToJson(command);
        PhotonRPCHandler.GetInstance().PostRPC(rpcmodel);
    }

    public void RepositionPlayer(SyncCommand command)
    {
        stage.CharacterExit((int)this.transform.position.x, (int)this.transform.position.z);
        this.transform.position = new Vector3((float)command.x, 1f, (float)command.z);
    }
}