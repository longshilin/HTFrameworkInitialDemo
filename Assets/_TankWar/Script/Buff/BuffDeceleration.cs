/// <summary>
/// 减速Buff
/// </summary>
public class BuffDeceleration : Buff
{
    public override string Name => "减速";
    public override float Duration => 10;

    private float _originalSpeed;

    public override void Invalid()
    {
        base.Invalid();

        Target.Speed = _originalSpeed;
    }

    public override void Valid()
    {
        base.Valid();

        _originalSpeed = Target.Speed;
        Target.Speed *= 0.5f;
    }
}
