using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void InitializeStage()
    {

    }

    void InitializePlayer()
    {

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
