using System.Collections;
using UnityEngine;

namespace NPC.Guard
{
    public class Searching : GuardState
    {
        private Transform _player;
        private int _totalAttemptToFind = 3;
        private int _attempt;
        private Coroutine _rotateGuard;
        public Searching(Guard guard, Transform player) : base(guard)
        {
            _player = player;
        }
        public override void Enter()
        {
            base.Enter();
            _guard.Agent.RechedDestination += OnReachDestination;

            _guard.PlayAnimation("Walking");
            _guard.Agent.SetDestination(_player.position);
        }
        public override void Exit()
        {
            base.Exit();
            _guard.Agent.RechedDestination -= OnReachDestination;

            if (_rotateGuard != null)
            {
                _guard.StopCoroutine(_rotateGuard);
            }
        }
        private void OnReachDestination()
        {
            float distance = Vector3.Distance(_guard.transform.position, _player.position);
            if (_rotateGuard != null)
            {
                _guard.StopCoroutine(_rotateGuard);
            }
            _rotateGuard = _guard.StartCoroutine(RotateGuard());
            if (_attempt < _totalAttemptToFind && distance < _guard.SearchRange)
            {
                _attempt++;
                _guard.Agent.SetDestination(_player.position);
            }
            else
            {
                _guard.StateManager.SwitchStateTo(new Walking(_guard));
                return;
            }
        }
        private IEnumerator RotateGuard()
        {
            Quaternion targetRotation = Quaternion.LookRotation(VectorHelper.Direction(_player, _guard.transform));
            _tempTime = 0;
            float rotationTime = 1;
            while (_tempTime < rotationTime)
            {
                _tempTime += Time.deltaTime;
                _guard.transform.rotation = Quaternion.Slerp(targetRotation, targetRotation, _tempTime / rotationTime);
                yield return null;
            }
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
    }
}
