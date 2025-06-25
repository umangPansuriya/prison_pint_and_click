using UnityEngine;

namespace NPC.Guard
{
    public class GunGuardWalking : GunGuardState
    {
        private Vector3 _destination;
        public GunGuardWalking(GunGuard guard, Vector3 destination) : base(guard)
        {
            _guard.Agent.Speed = _guard.WalkSpeed;
            _destination = destination;
        }
        public override void Enter()
        {
            base.Enter();
            _guard.Agent.RechedDestination_Action += OnRechDestination;
            _guard.Agent.SetDestination(_destination);

            _guard.PlayAnimation("Walking");
        }
        public override void Exit()
        {
            base.Exit();
            _guard.Agent.RechedDestination_Action -= OnRechDestination;
        }

        protected override void OnPlyerDetect(Transform player)
        {
            float distance = Vector3.Distance(_guard.transform.position, player.position);
            if (distance < _guard.CatchRange)
            {
                _guard.StateManager.SwitchStateTo(new GunGuardCatch(_guard, player));
                return;
            }
            else if (distance < _guard.ShootRange)
            {
                _guard.StateManager.SwitchStateTo(new GunGuardShooting(_guard, player));
                return;
            }
            else if (distance < _guard.ChaseRange)
            {
                _guard.StateManager.SwitchStateTo(new GunGuardChasing(_guard, player));
                return;
            }
        }
        private void OnRechDestination()
        {
            _guard.StateManager.SwitchStateTo(new GunGuardStanding(_guard));
        }
    }
}