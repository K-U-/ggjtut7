using UnityEngine;
using System.Collections;

public class MagicCircleLight : MonoBehaviour {
    public float LightState;
    public MeshRenderer Body;
    public MeshRenderer Side1;
    public MeshRenderer Side2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SetLight();
    }

    void SetLight()
    {
        Color _setColor = Color.white;
        _setColor.a = LightState;
        Body.materials[1].SetColor("_TintColor", _setColor);
        Side1.material.SetColor("_TintColor", _setColor);
        Side2.material.SetColor("_TintColor", _setColor);
    }
}
