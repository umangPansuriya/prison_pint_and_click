using UnityEngine;

namespace NPC.Guard
{
    public class Catch : GuardState
    {
        private Transform _player;
        public Catch(Guard guard, Transform player) : base(guard)
        {
            _guard.Agent.Speed = _guard.ChaseSpeed;
            _player = player;
        }
        public override void Enter()
        {
            base.Enter();
            Timer.Tick += OnTick;

            _guard.PlayAnimation("SlowRun");
        }
        public void OnTick()
        {
            _guard.Agent.SetDestination(_player.position);
            _guard.Agent.Speed = 3;
            float distance = DistanceToPlayer(_player.position);
            _guard.transform.rotation = Quaternion.LookRotation(VectorHelper.Direction(_player.position, _guard.transform.position));
            if (distance < 1f)
            {
                GameEvent.RiseGameOver();
                _guard.Agent.Stop();
                _guard.Visioncon.PlayerDetected -= OnPlyerDetect;
            }
        }
        public override void Exit()
        {
            base.Exit();
            Timer.Tick -= OnTick;
        }
    }
}