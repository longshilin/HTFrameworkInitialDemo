using HT.Framework;
using UnityEngine;
/// <summary>
/// 关卡2
/// </summary>
[EntityResource(Global.ABLevel, "Assets/Prefab/Level/Level2.prefab", "null")]
public class Level2 : EntityLogicBase, ILevel
{
    /// <summary>
    /// 玩家出生点
    /// </summary>
    public Transform Birthplace
    {
        get
        {
            return Entity.FindChildren("玩家出生点").transform;
        }
    }
    /// <summary>
    /// 本关Boss
    /// </summary>
    public FSM Boss { get; private set; }
    /// <summary>
    /// 本关剩余敌人数量
    /// </summary>
    public int EnemyNumber { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        Boss = Entity.GetComponentByChild<FSM>("BOSS");
        EnemyNumber = Entity.GetComponentsInChildren<FSM>(true).Length;

        Main.m_Event.Subscribe<EventEnemyKill>(OnEnemyKill);
        Main.m_Event.Subscribe<EventBossKill>(OnBossKill);
    }

    /// <summary>
    /// 显示
    /// </summary>
    public override void OnShow()
    {
        base.OnShow();

        Global.ChangeBGMusic("第二关背景音乐");
    }

    /// <summary>
    /// 销毁实体
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();

        Main.m_Event.Unsubscribe<EventEnemyKill>(OnEnemyKill);
        Main.m_Event.Unsubscribe<EventBossKill>(OnBossKill);
    }

    private void OnEnemyKill()
    {
        EnemyNumber -= 1;

        //还剩一个敌人（就是BOSS），激活BOSS
        if (EnemyNumber == 1)
        {
            Boss.gameObject.SetActive(true);

            Global.ChangeBGMusic("BOSS战斗音乐");
        }
    }

    private void OnBossKill()
    {
        EnemyNumber -= 1;

        Main.m_Event.Throw(Main.m_ReferencePool.Spawn<EventGameBeat>());
    }
}