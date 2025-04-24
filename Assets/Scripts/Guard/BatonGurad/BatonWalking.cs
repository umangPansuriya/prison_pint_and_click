using UnityEngine;

namespace NPC.BatonGuard
{
    public class BatonWalking : BatonGuardState
    {
        public BatonWalking(BatonGuard guard) : base(guard)
        {
            _guard.Agent.Speed = _guard.WalkSpeed;
        }
        public override void Enter()
        {
            base.Enter();
            _guard.Agent.RechedDestination += OnRechDestination;

            _guard.GotoNextPoint();
            _guard.PlayAnimation("Walking");
        }
        public override void Exit()
        {
            base.Exit();
            _guard.Agent.RechedDestination -= OnRechDestination;
        }

        protected override void OnPlyerDetect(Transform player)
        {
            float distance = Vector3.Distance(_guard.transform.position, player.position);
            if (distance < _guard.CatchRange)
            {
                _guard.StateManager.SwitchStateTo(new BatonCatch(_guard, player));
                return;
            }
            else if (distance < _guard.ChaseRange)
            {
                _guard.StateManager.SwitchStateTo(new BatonChasing(_guard, player));
                return;
            }
        }
        private void OnRechDestination()
        {
            _guard.StateManager.SwitchStateTo(new BatonStanding(_guard));
        }
    }
}