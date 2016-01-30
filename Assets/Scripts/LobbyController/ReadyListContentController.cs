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
        nameLabel.color = Color.white;
        transform.localScale = Vector3.zero;
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
        iTween.ScaleTo(gameObject, iTween.Hash(
                "islocal",true,
                "scale",Vector3.one,
                "easetype",iTween.EaseType.easeOutBack,
                "time",0.3f
            ));
    }

    public void Hide()
    {
        iTween.ScaleTo(gameObject, iTween.Hash(
                "islocal", true,
                "scale", Vector3.zero,
                "easetype", iTween.EaseType.easeInCubic,
                "time", 0.3f
            ));
    }


}
