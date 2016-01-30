using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TickerController : MonoBehaviour {

    private Text text;

    public void Initialize(string str)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-30f, 30f), Random.Range(-100f, 100f));
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10.0f, 10f)));
        text = GetComponent<UnityEngine.UI.Text>();
        text.text = str;
        iTween.ScaleFrom(gameObject, iTween.Hash(
            "islocal",true,
            "scale",Vector3.zero,
            "time",0.3f,
            "easetype",iTween.EaseType.easeOutCirc));
        StartCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(2.0f);
        iTween.ValueTo(gameObject, iTween.Hash(
            "onupdate","UpdateValue",
            "from",1.0f,"to",0f,
            "time",0.3f,
            "easetype",iTween.EaseType.linear,
            "oncomplete","CompleteAlpha",
            "oncompletetarget",gameObject));
    }

    void UpdateValue(float val)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, val);
        text.SetAllDirty();
    }

    void CompleteAlpha()
    {
        Destroy(gameObject);
    }
}
