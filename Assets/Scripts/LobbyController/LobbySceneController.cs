using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbySceneController : MonoBehaviour {

    public Text roomName;
    public ReadyListContentController[] readyList;

    IEnumerator Start()
    {
        roomName.text = GameManager.GetInstance().roomName;
        while (true)
        {
            
            GameManager.GetInstance().CheckUserList();
            UpdateReadyView();
            yield return new WaitForSeconds(1);
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
        GameManager.GetInstance().LoadScene("Field");
    }
}
