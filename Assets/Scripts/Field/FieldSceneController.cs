using UnityEngine;
using System.Collections;

public class FieldSceneController : MonoBehaviour {

    public float gameRemainTime = 180f;
    private bool startedGame = true;
    public FieldUIController uiController;
    void Update()
    {
        if (startedGame)
        {
            gameRemainTime -= Time.deltaTime;
            uiController.InitializeTime(gameRemainTime);
        }
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
