using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PopButton : UIAction {

	// Use this for initialization
	void Start () {
        transform.GetComponent<Button>().onClick.AddListener(
            () => {
                AddSeq(Print);
                AddSeq(Scale);
                AddSeq(Alpha);
                Move(CallSeq);
            }
            );
	}

    public override void Action()
    {
        base.Action();
    }

    private void Print()
    {
        Debug.Log("Call");
    }

    private void CallBack()
    {
        Debug.Log("CallBack");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //public override void Action()
    //{

    //}
}
