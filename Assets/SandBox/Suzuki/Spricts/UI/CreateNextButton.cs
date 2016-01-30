using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CreateNextButton : UIAction {

    [SerializeField]
    private GameObject mNext;

    public override void Action()
    {
        base.Action();
        AddSeq(Alpha);
        AlphaMove(() => { 
            //UIContller.GetInstance.CreateUI(mNext, Vector2.zero, Layers.Layer_Def); 
            mNext.SetActive(true);
            Reset();
            //Destroy(gameObject); 
        });

        //ここら辺で一人目の通知

        //
        
    }

    public void Seq()
    {
        base.Action();
        AddSeq(Alpha);
        AlphaMove(() =>
        {
            //UIContller.GetInstance.CreateUI(mNext, Vector2.zero, Layers.Layer_Def); 
            mNext.SetActive(true);
            Reset();

        },transform.parent.gameObject);
    }

}
