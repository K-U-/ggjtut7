using UnityEngine;
using System.Collections;

public class ObjectActive : MonoBehaviour {

    [SerializeField]
    private GameObject[] mTargetIsEnable;

    public void Enable()
    {
        foreach (var i in mTargetIsEnable) i.SetActive(true);
    }
}
