using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FieldUIController : MonoBehaviour {

    public GameObject akumaButton;
    public GameObject killButton;
    public GameObject tickerObject;
    public GameObject[] controller;
    public Transform tickerParent;
    public Text remainTime;

    void Awake()
    {
        if (GameManager.GetInstance().myInfo.isHuman)
        {
            akumaButton.SetActive(false);
            killButton.SetActive(true);
        }
        else
        {
            akumaButton.SetActive(true);
            killButton.SetActive(false);
        }
        if (GameManager.GetInstance().myInfo.isSpector)
        {
            foreach (var obj in controller)
            {
                obj.SetActive(false);
            }
            PhotonRPCHandler.actionTickerEvent += OnActionTicker;
        }
        else
        {
            PhotonRPCHandler.actionTickerEvent += DummyActionTicker;
        }
    }

	public void OnClickAkumabutton(){
		var input = GameManager.GetInstance ().GetCharacterControllerById (
			GameManager.GetInstance ().myInfo.id.ToString()).GetComponent<StrikeInputUtility> ();

		input.ChangeMode ();
	}

    public void InitializeTime(float time)
    {
        int minute = ((int) (time / 60f));
        int seconds = (int)time - (minute * 60);
        remainTime.text = minute + ":" + seconds;  
    }

    void OnDestroy()
    {
        if (GameManager.GetInstance().myInfo.isSpector)
        {
            PhotonRPCHandler.actionTickerEvent -= OnActionTicker;
        }
        else
        {
            PhotonRPCHandler.actionTickerEvent -= DummyActionTicker;
        }
    }

    void OnActionTicker(PhotonRPCModel model)
    {
        GameObject obj = Instantiate(tickerObject) as GameObject;
        obj.transform.SetParent(tickerParent);
        var ticker = obj.GetComponent<TickerController>();
        ticker.Initialize(model.message);
    }

    void DummyActionTicker(PhotonRPCModel model)
    {
        return;
    }
		

}
