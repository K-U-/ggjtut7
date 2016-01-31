using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NextScene : MonoBehaviour {

    [SerializeField]
    private string mSceneName;

    void Awake()
    {
        PhotonRPCHandler.departureEvent += OnDeparture;
    }

    [ContextMenu("Change")]
    public void SceneChange()
    {
        GetComponent<Button>().interactable = false;
        GameManager.GetInstance().LoadScene(mSceneName);



        if (GameManager.GetInstance().myInfo.isHost)
        {
            PhotonRPCModel model = new PhotonRPCModel();
            model.senderId = GameManager.GetInstance().myInfo.id.ToString();
            model.command = PhotonRPCCommand.Departure;
            model.message = mSceneName;
            PhotonRPCHandler.GetInstance().PostRPC(model);
        }

    }

    private void Departure()
    {


        //ここらへんでクライアントに通知を出す。
        if (!GameManager.GetInstance().myInfo.isHost)
        {
            SceneChange();
        }
    }

    void OnDestroy()
    {
        PhotonRPCHandler.departureEvent -= OnDeparture;
    }

    private void OnDeparture(PhotonRPCModel model)
    {
        if (!GameManager.GetInstance().myInfo.isHost)
        {
            SceneChange();
        }
    }

}
