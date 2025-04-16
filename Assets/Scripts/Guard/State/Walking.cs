using UnityEngine;

namespace NPC.Guard
{
    public class Walking : GuardState
    {
        public Walking(Guard guard) : base(guard)
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
                _guard.StateManager.SwitchStateTo(new Catch(_guard, player));
                return;
            }
            else if (distance < _guard.ShootRange)
            {
                _guard.StateManager.SwitchStateTo(new Shooting(_guard, player));
                return;
            }
            else if (distance < _guard.ChaseRange)
            {
                _guard.StateManager.SwitchStateTo(new Chasing(_guard, player));
                return;
            }
        }
        private void OnRechDestination()
        {
            _guard.StateManager.SwitchStateTo(new Standing(_guard));
        }
    }
}