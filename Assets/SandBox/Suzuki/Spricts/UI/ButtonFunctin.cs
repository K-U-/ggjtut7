using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonFunctin : MonoBehaviour {

    [SerializeField]
    protected Layers mLayer;
    [SerializeField]
    protected GameObject mNextUI;
    [SerializeField]
    protected Vector2 mPosition;

    protected UIContller mUICtr;

    private void Start()
    {
        mUICtr = UIContller.GetInstance;
        transform.GetComponent<Button>().onClick.AddListener(UIButtonFunction);
        
        //UIButtonFunction();
    }

    public virtual void UIButtonFunction()
    {

    }

}
