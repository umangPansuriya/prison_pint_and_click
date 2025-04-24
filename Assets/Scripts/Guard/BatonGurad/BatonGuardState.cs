using UnityEngine;

namespace NPC.BatonGuard
{
    public abstract class BatonGuardState : State
    {
        protected BatonGuard _guard;
        protected float _tempTime;
        public BatonGuardState(BatonGuard guard)
        {
            _guard = guard;
        }
        public override void Enter()
        {
            _guard.Visioncon.PlayerDetected += OnPlyerDetect;
        }
        public override void Exit()
        {
            _guard.Visioncon.PlayerDetected -= OnPlyerDetect;
        }
        protected virtual void OnPlyerDetect(Transform player) { }
        protected float DistanceToPlayer(Vector3 player)
        {
            return Vector3.Distance(_guard.transform.position, player);
        }
    }
}