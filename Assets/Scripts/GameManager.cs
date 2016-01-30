using UnityEngine;
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
}
