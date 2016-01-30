using UnityEngine;
using System.Collections;

public class JsonSample : MonoBehaviour {

    private PlayerInfo info;
    private string playerInfoJson;
    void Awake()
    {
        info = new PlayerInfo();
        info.name = "ほげ";
        info.id = 1;

        ToJson();
        ToObject();
    }

    /// <summary>
    /// クラスのインスタンスを文字列にする
    /// </summary>
    void ToJson()
    {
        playerInfoJson = JsonUtility.ToJson(info);
        Debug.Log(playerInfoJson);
    }

    /// <summary>
    /// 文字列からクラスのインスタンスを生成する
    /// 正しい形式でやらないとダメ
    /// </summary>
    void ToObject()
    {
        PlayerInfo serializedInfo = JsonUtility.FromJson<PlayerInfo>(playerInfoJson);
        Debug.Log(serializedInfo.name + ":" + serializedInfo.id);
    }
}
