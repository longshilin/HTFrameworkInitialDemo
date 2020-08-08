using HT.Framework;

/// <summary>
/// Buff基类
/// </summary>
public abstract class Buff : IReference
{
    /// <summary>
    /// Buff目标
    /// </summary>
    public TankData Target { get; set; }
    /// <summary>
    /// Buff状态
    /// </summary>
    public BuffState State { get; private set; }
    /// <summary>
    /// Buff描述名称
    /// </summary>
    public abstract string Name { get; }
    /// <summary>
    /// Buff持续时间
    /// </summary>
    public abstract float Duration { get; }
    /// <summary>
    /// Buff生效
    /// </summary>
    public virtual void Valid()
    {
        State = BuffState.Valid;
    }
    /// <summary>
    /// Buff失效
    /// </summary>
    public virtual void Invalid()
    {
        State = BuffState.Invalid;
    }
    /// <summary>
    /// 重置
    /// </summary>
    public virtual void Reset()
    {
        Target = null;
    }

    /// <summary>
    /// Buff状态
    /// </summary>
    public enum BuffState
    {
        Valid,
        Invalid
    }
}