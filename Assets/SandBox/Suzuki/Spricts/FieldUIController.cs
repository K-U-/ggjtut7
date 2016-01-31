using UnityEngine;
using System.Collections;

public class FieldUIController : MonoBehaviour {

    public GameObject akumaButton;
    public GameObject killButton;
    public GameObject tickerObject;
    public GameObject[] controller;
    public Transform tickerParent;

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
