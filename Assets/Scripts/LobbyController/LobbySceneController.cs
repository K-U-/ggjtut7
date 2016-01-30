using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class LobbySceneController : MonoBehaviour {

    public Text roomName;
    public ReadyListContentController[] readyList;
    public GameObject readyButton;

    IEnumerator Start()
    {
        PhotonRPCHandler.departureEvent += OnDeparture;
        roomName.text = GameManager.GetInstance().roomName;
        while (true)
        {
            
            GameManager.GetInstance().CheckUserList();
            UpdateReadyView();
            UpdateReadyButtonStatus();
            yield return new WaitForSeconds(1);
        }
        
    }

    void OnDestroy()
    {
        PhotonRPCHandler.departureEvent -= OnDeparture;
    }

    private void UpdateReadyButtonStatus(){
        if (GameManager.GetInstance().myInfo.isHost &&
            GameManager.GetInstance().ReadyStatusList.readyStatusList.Count(_=>!_.info.isSpector) >= 4)
        {
            readyButton.SetActive(true);
        }
        else
        {
            readyButton.SetActive(false);
        }
    }

    private void UpdateReadyView()
    {
        var list = GameManager.GetInstance().ReadyStatusList.readyStatusList.Where(_=>!_.info.isSpector);
        for (int i = 0; i < readyList.Length; ++i)
        {
            if (list.ToList().Count > i)
            {
                readyList[i].Initialize(list.ToList()[i].info.name);
            }
            else
            {
                readyList[i].Initialize("");
            }
        }
    }

    public void Departure()
    {
        if (GameManager.GetInstance().myInfo.isHost)
        {
            PhotonRPCModel model = new PhotonRPCModel();
            model.senderId = GameManager.GetInstance().myInfo.id.ToString();
            model.command = PhotonRPCCommand.Departure;
            model.message = "";
        
            PhotonRPCHandler.GetInstance().PostRPC(model);
        }
        GameManager.GetInstance().LoadScene("Field");
    }

    private void OnDeparture(PhotonRPCModel model)
    {
        if (!GameManager.GetInstance().myInfo.isHost)
        {
            Departure();
        }
    }
}
