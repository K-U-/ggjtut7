using UnityEngine;
using System;
using System.Collections;


/// <summary>
/// ロビーでの準備状態
/// </summary>
[Serializable]
public class PlayerReadyStatus : ModelBase
{
    public PlayerInfo info;
    public bool isReady;
}

/// <summary>
/// 準備状態の一覧
/// </summary>
[Serializable]
public class PlayerReadyStatusList : ModelBase {
    PlayerReadyStatus[] readyStatusList;
}
