using HT.Framework;
/// <summary>
/// 击中敌人事件
/// </summary>
public class EventHitEnemy : EventHandlerBase
{
    public FSM Enemy;
    public int Hurt;
    public string BuffName;

    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public EventHitEnemy Fill(FSM enemy, int hurt, string buffName)
    {
        Enemy = enemy;
        Hurt = hurt;
        BuffName = buffName;
        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
