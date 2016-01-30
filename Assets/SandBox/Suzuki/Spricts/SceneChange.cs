using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {
    [SerializeField]
    private string name;
    public void Change()
    {
        Debug.Log("SceneChange");
        CameraFade.StartAlphaFade(Color.white, false, 0.5f, 0f, () => { Application.LoadLevel(name); });
    }
}
