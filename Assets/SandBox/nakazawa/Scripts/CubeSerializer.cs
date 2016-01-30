using UnityEngine;
using System.Collections;

public class CubeSerializer : Photon.MonoBehaviour {

    float ti;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            //データの送信
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(this.GetComponent<Renderer>().material.color.r);
            stream.SendNext(this.GetComponent<Renderer>().material.color.g);
            stream.SendNext(this.GetComponent<Renderer>().material.color.b);
            stream.SendNext(this.GetComponent<Renderer>().material.color.a);
            ti = Time.timeSinceLevelLoad;
            Debug.LogWarning("r = " + this.GetComponent<Renderer>().material.color.r + " g = " + this.GetComponent<Renderer>().material.color.g + " b = " + this.GetComponent<Renderer>().material.color.b + " a = " + this.GetComponent<Renderer>().material.color.a);
        } else {
            //データの受信
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            float r = (float)stream.ReceiveNext();
            float g = (float)stream.ReceiveNext();
            float b = (float)stream.ReceiveNext();
            float a = (float)stream.ReceiveNext();
            ti = Time.timeSinceLevelLoad - ti;
            Debug.Log("r = " + r + " g = " + g + " b = " + b + " a = " + a);
            this.GetComponent<Renderer>().material.color = new Vector4(r, g, b, a);
        }
    }    
}