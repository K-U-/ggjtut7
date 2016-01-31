using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RawImageScroll : MonoBehaviour {
    public float scrollX;
    public float scrollY;
    private RawImage _raw;
	// Use this for initialization
	void Start () {
        _raw = gameObject.GetComponent<RawImage>();
        _raw.uvRect = new Rect(_raw.uvRect.x, _raw.uvRect.y, Screen.width / 64, Screen.height / 64);
    }
	
	// Update is called once per frame
	void Update () {
        _raw.uvRect = new Rect(_raw.uvRect.x + scrollX * Time.deltaTime, _raw.uvRect.y + scrollY * Time.deltaTime, _raw.uvRect.width, _raw.uvRect.height);
	}
}
