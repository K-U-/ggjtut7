using UnityEngine;
using System.Collections;

public class RoomListContent : MonoBehaviour {

    public string roomId;
    public System.Action<string> onClick;

    public void OnClickButton()
    {
        if (onClick != null)
        {
            GameManager.GetInstance().roomName = roomId;
            onClick(roomId);
        }
    }
}
