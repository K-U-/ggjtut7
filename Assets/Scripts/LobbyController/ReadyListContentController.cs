using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadyListContentController : MonoBehaviour {

    private Text nameLabel;
    [SerializeField]
    private bool isRight = true;
    private Vector3 defaultPosition;
    private Vector3 hiddenPosition;

    void Awake()
    {
        defaultPosition = transform.localPosition;
        nameLabel = GetComponentInChildren<Text>();
        //transform.localPosition += (isRight ? Vector3.right : Vector3.left) * 300f;
        hiddenPosition = transform.localPosition;
    }

    public void Initialize(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (nameLabel.text != name)
            {
                nameLabel.text = name;
                Show();
            }
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        return;
        iTween.MoveTo(gameObject, iTween.Hash(
                "islocal",true,
                "position",defaultPosition,
                "easetype",iTween.EaseType.easeOutBack,
                "time",0.3f
            ));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        return;
        iTween.MoveTo(gameObject, iTween.Hash(
                "islocal", true,
                "position", hiddenPosition,
                "easetype", iTween.EaseType.easeInCubic,
                "time", 0.3f
            ));
    }


}
