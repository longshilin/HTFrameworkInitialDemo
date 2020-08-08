using HT.Framework;
using UnityEngine;

/// <summary>
/// 关卡
/// </summary>
public interface ILevel
{
    /// <summary>
    /// 玩家出生点
    /// </summary>
    Transform Birthplace { get; }
    /// <summary>
    /// 本关Boss
    /// </summary>
    FSM Boss { get; }
}
