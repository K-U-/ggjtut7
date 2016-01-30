using UnityEngine;
using System.Collections;

[System.Serializable] //これがないと変換がうまくいかない
public class PlayerInfo {
    public int id;
    public string name;
    public string hash;
    public bool isHost;
}