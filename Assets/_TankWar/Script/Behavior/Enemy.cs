using HT.Framework;
using System;
using UnityEngine;

/// <summary>
/// 敌人
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 敌人坦克数据集
    /// </summary>
    public DataSetTank DataSet;

    private void Awake()
    {
        FSM fsm = GetComponent<FSM>();
        fsm.CurrentData.Cast<TankData>().SetData(DataSet, "Enamy" + Guid.NewGuid().ToString());
        Main.m_FSM.RegisterFSM(fsm);
    }
}
