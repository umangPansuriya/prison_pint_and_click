public abstract class NormalGuardState : State
{
    protected NormalGuard _guard;
    public NormalGuardState(NormalGuard guard)
    {
        _guard = guard;
    }
}
