using UnityEngine;
using System.Collections;

public class RoundResultController : MonoBehaviour {

    void Awake()
    {
        PhotonRPCHandler.departureEvent = OnDeparture;
    }

    public void Departure(string target)
    {
        if (GameManager.GetInstance().myInfo.isHost)
        {
            var list = GameManager.GetInstance().ReadyStatusList.readyStatusList;
            foreach (var val in list)
            {
                val.info.isHuman = true;
            }
            list[Random.Range(0, list.Count)].info.isHuman = false;
            GameManager.GetInstance().PostUpdateReadyRPC();
            PhotonRPCModel model = new PhotonRPCModel();
            model.senderId = GameManager.GetInstance().myInfo.id.ToString();
            model.command = PhotonRPCCommand.Departure;
            model.message = target;

            PhotonRPCHandler.GetInstance().PostRPC(model);
        }
        GameManager.GetInstance().LoadScene(target);
    }

    void OnDeparture(PhotonRPCModel model)
    {
        if (!GameManager.GetInstance().myInfo.isHost)
        {
            Departure(model.message);
        }
    }
}
