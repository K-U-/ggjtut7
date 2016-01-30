using UnityEngine;
using UnityEngine.UI;
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
            GameManager.GetInstance().ReadyStatusList.readyStatusList.Count >= 4)
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
        PlayerReadyStatusList list = GameManager.GetInstance().ReadyStatusList;
        for (int i = 0; i < readyList.Length; ++i)
        {
            if (list.readyStatusList.Count > i)
            {
                readyList[i].Initialize(list.readyStatusList[i].info.name);
            }
            else
            {
                readyList[i].Initialize("");
            }
        }
    }

    public void Departure()
    {
        PhotonRPCModel model = new PhotonRPCModel();
        model.senderId = GameManager.GetInstance().myInfo.id.ToString();
        model.command = PhotonRPCCommand.Departure;
        PhotonRPCHandler.GetInstance().PostRPC(model);
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
