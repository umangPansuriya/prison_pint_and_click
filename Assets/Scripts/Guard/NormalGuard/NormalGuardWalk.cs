using UnityEngine;

public class NormalGuardWalk : NormalGuardState
{
    private Vector3 _destination;
    public NormalGuardWalk(NormalGuard guard, Vector3 destination) : base(guard)
    {
        _destination = destination;
    }
    public override void Enter()
    {
        _guard.Agent.SetDestination(_destination);
        _guard.Agent.RechedDestination_Action += OnRechedDestination;
    }

    private void OnRechedDestination()
    {

    }

    public override void Exit()
    {

    }
}
