using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


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
    public List<PlayerReadyStatus> readyStatusList;
}
