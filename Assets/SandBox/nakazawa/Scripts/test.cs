using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class test : MonoBehaviour {

    public Text tes;
    Vector3 pos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        pos = this.transform.position;
        tes.text = ("x = " + pos.x + "\r\nz = " + pos.z);
	}
}
