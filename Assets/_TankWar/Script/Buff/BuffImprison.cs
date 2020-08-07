/// <summary>
/// 禁锢Buff
/// </summary>
public class BuffImprison : Buff
{
    public override string Name => "禁锢";
    public override float Duration => 5;

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
        Target.Speed = 0;
    }
}
