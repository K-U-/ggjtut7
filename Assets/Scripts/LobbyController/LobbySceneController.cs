using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbySceneController : MonoBehaviour {

    public Text roomName;
    public Text[] nameArray;

    IEnumerator Start()
    {
        roomName.text = GameManager.GetInstance().roomName;
        while (true)
        {
            UpdateReadyView();
            yield return new WaitForSeconds(1);
        }
        
    }

    private void UpdateReadyView()
    {
        foreach (Text name in nameArray)
        {
            name.text = "";
        }
        PlayerReadyStatusList list = GameManager.GetInstance().ReadyStatusList;
        for (int i = 0; i < list.readyStatusList.Count; ++i)
        {
            nameArray[i].text = list.readyStatusList[i].info.name;
        }
    }

    public void Departure()
    {
        GameManager.GetInstance().LoadScene("Field");
    }
}
