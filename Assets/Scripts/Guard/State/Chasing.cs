using UnityEngine;

namespace NPC.Guard
{
    public class Chasing : GuardState
    {
        private Transform _player;
        public Chasing(Guard guard, Transform player) : base(guard)
        {
            _guard.Agent.Speed = _guard.ChaseSpeed;
            _player = player;
        }
        public override void Enter()
        {
            base.Enter();
            Timer.Tick += OnTick;
            _guard.Visioncon.PlayerOutOfRange += OnPlayerOutOfRange;
            _guard.PlayAnimation("SlowRun");
        }
        public void OnTick()
        {
            if (_tempTime < _guard.MinimumStayTime)
            {
                _tempTime += Time.deltaTime;
            }
            else
            {
                float distance = Vector3.Distance(_guard.transform.position, _player.position);
                if (distance < _guard.CatchRange)
                {
                    _guard.StateManager.SwitchStateTo(new Catch(_guard, _player));
                    return;
                }
                else if (distance < _guard.ShootRange)
                {
                    _guard.StateManager.SwitchStateTo(new Shooting(_guard, _player));
                    return;
                }
            }
            _guard.Agent.SetDestination(_player.position);
        }
        public override void Exit()
        {
            base.Exit();
            Timer.Tick -= OnTick;
            _guard.Visioncon.PlayerOutOfRange -= OnPlayerOutOfRange;
        }
        private void OnPlayerOutOfRange()
        {
            _guard.StateManager.SwitchStateTo(new Searching(_guard, _player));
        }
    }
}
