using UnityEngine;
using System.Collections;

public class RoomList : MonoBehaviour {

    public GameObject roomListContent;
    public Transform parent;

    public void Initialize()
    {
        foreach (var obj in parent.GetComponentsInChildren<RoomListContent>())
        {
            Destroy(obj.gameObject);
        }
        Debug.Log(PhotonNetwork.GetRoomList().Length);
        foreach (var room in PhotonNetwork.GetRoomList())
        {
            GameObject obj = Instantiate(roomListContent) as GameObject;
            obj.transform.SetParent(parent);
            var content = obj.GetComponent<RoomListContent>();
            content.roomId = room.name;
            content.GetComponentInChildren<UnityEngine.UI.Text>().text = room.name;
            content.onClick = delegate(string id)
            {
                PhotonNetwork.JoinRoom(id);
            };
        }
    }
}
