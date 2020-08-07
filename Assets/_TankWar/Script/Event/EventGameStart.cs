using HT.Framework;
/// <summary>
/// 游戏开始事件
/// </summary>
public class EventGameStart : EventHandlerBase
{
    public FSM Player;
    public int Level;

    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public EventGameStart Fill(FSM player, int level)
    {
        Player = player;
        Level = level;
        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
