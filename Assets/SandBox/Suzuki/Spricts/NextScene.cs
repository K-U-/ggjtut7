using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NextScene : MonoBehaviour {

    [SerializeField]
    private string mSceneName;

    public void SceneChange()
    {
        GameManager.GetInstance().LoadScene(mSceneName);
        GetComponent<Button>().interactable = false;
    }
}
