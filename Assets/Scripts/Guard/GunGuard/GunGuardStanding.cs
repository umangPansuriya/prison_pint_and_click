using Prison.PatrollingGurd;
using System.Collections;
using UnityEngine;

namespace NPC.Guard
{
    public class GunGuardStanding : GunGuardState
    {
        private Coroutine _waitOnPointCoroutine;
        public GunGuardStanding(GunGuard guard) : base(guard)
        {
        }
        public override void Enter()
        {
            base.Enter();

            if (_waitOnPointCoroutine != null)
            {
                _guard.StopCoroutine(_waitOnPointCoroutine);
            }
            _waitOnPointCoroutine = _guard.StartCoroutine(WaitOnPoint());
        }
        private IEnumerator WaitOnPoint()
        {
            PatrollingPoint point = _guard.GetCurrentPoint();
            if (point.Animation != null)
            {
                _guard.PlayAnimation(point.Animation.name);
            }
            else
            {
                _guard.PlayAnimation("idle");
            }
            yield return new WaitForSeconds(point.StayDuration);
            _guard.StateManager.SwitchStateTo(new GunGuardWalking(_guard, _guard.GetNextPointPosition()));
        }
        public override void Exit()
        {
            base.Exit();

            if (_waitOnPointCoroutine != null)
            {
                _guard.StopCoroutine(_waitOnPointCoroutine);
            }
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
    }
}