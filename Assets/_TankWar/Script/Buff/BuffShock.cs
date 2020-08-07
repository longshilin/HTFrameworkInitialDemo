/// <summary>
/// 震慑Buff
/// </summary>
public class BuffShock : Buff
{
    public override string Name => "震慑";
    public override float Duration => 3;
    
    public override void Invalid()
    {
        base.Invalid();

        Target.Controller.IsEnable = true;
    }

    public override void Valid()
    {
        base.Valid();

        Target.Controller.IsEnable = false;
    }
}
