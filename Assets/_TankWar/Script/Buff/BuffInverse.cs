/// <summary>
/// 逆反Buff
/// </summary>
public class BuffInverse : Buff
{
    public override string Name => "逆反";
    public override float Duration => 5;
    
    public override void Invalid()
    {
        base.Invalid();

        Target.Controller.IsInverse = false;
    }

    public override void Valid()
    {
        base.Valid();

        Target.Controller.IsInverse = true;
    }
}
