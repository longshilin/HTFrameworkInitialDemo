using HT.Framework;
/// <summary>
/// 超级武器冷却中事件
/// </summary>
public class EventSuperWeaponCooling : EventHandlerBase
{
    public float Percen;

    /// <summary>
    /// 填充数据，所有属性、字段的初始化工作可以在这里完成
    /// </summary>
    public EventSuperWeaponCooling Fill(float percen)
    {
        Percen = percen;
        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {
        
    }
}
