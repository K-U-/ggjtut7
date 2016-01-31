using UnityEngine;
using System.Collections;

public class FieldSceneController : MonoBehaviour {

    private float gameRemainTime = 180f;
    private bool startedGame = true;
    public FieldUIController uiController;
    void Update()
    {
        if (startedGame)
        {
            gameRemainTime -= Time.deltaTime;
            gameRemainTime = Mathf.Max(0, gameRemainTime);
            uiController.InitializeTime(gameRemainTime);
            if (gameRemainTime <= 0 && GameManager.GetInstance().myInfo.isHost)
            {
                CallTimeOver();
            }
        }
    }

    private void CallTimeOver()
    {
        PhotonRPCModel model = new PhotonRPCModel();
        model.command = PhotonRPCCommand.TimeOver;
        PhotonRPCHandler.GetInstance().PostRPC(model);
    }
    IEnumerator Start()
    {
        PhotonRPCHandler.startBattleEvent += OnStartBattleEvent;
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
        }
    }

    void OnDestroy()
    {
        PhotonRPCHandler.startBattleEvent -= OnStartBattleEvent;
    }

    private void OnStartBattleEvent(PhotonRPCModel model)
    {
        startedGame = true;
    }

    private void SyncGameRemainTime(PhotonRPCModel model)
    {
        gameRemainTime = float.Parse(model.message);
    }
}
