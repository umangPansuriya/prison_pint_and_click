using Prison.PatrollingGurd;
using System.Collections;
using UnityEngine;

namespace NPC.BatonGuard
{
    public class BatonStanding : BatonGuardState
    {
        private Coroutine _waitOnPointCoroutine;

        public BatonStanding(BatonGuard guard) : base(guard)
        {
        }
        public override void Enter()
        {
            base.Enter();

            if (_waitOnPointCoroutine != null)
            {
                _guard.StopCoroutine(_waitOnPointCoroutine);
            }
            _waitOnPointCoroutine = _guard.StartCoroutine(WaitOnPoitn());
        }
        private IEnumerator WaitOnPoitn()
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
            _guard.StateManager.SwitchStateTo(new BatonWalking(_guard));
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
                _guard.StateManager.SwitchStateTo(new BatonCatch(_guard, player));
                return;
            }
            else if (distance < _guard.ChaseRange)
            {
                _guard.StateManager.SwitchStateTo(new BatonChasing(_guard, player));
                return;
            }
        }
    }
}