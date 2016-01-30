using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private PhotonView photonView;

    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    void Update()
    {
        //if (photonView.isMine)
        //{
        //    float x = Input.GetAxis("Horizontal");
        //    float z = Input.GetAxis("Vertical");
        //    transform.Translate(x * 0.2f, 0, z * 0.2f);

        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        this.GetComponent<Renderer>().material.color = ChangeColor();
        //    }
        //}
    }

    void FixedUpdate() {
        if (photonView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.GetComponent<Renderer>().material.color = ChangeColor();
            }
        }
    }

    Color ChangeColor()
    {
        switch(Random.Range(0, 5))
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            case 4: return Color.white;
            case 5: return Color.black;
        }
        return Color.clear;
    }
}
