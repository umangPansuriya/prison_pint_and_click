using UnityEngine;

namespace NPC.Guard
{
    public abstract class GunGuardState : State
    {
        protected GunGuard _guard;
        protected float _tempTime;
        public GunGuardState(GunGuard guard)
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