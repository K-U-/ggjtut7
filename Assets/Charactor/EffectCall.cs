using UnityEngine;
using System.Collections;

public class EffectCall : MonoBehaviour {
    public GameObject Effect;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void EFCall () {
        Instantiate(Effect, transform.position, transform.rotation);
	}
}
