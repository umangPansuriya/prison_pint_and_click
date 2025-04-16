using System.Collections;
using UnityEngine;

namespace NPC.Guard
{
    public class Shooting : GuardState
    {
        private Transform _player;
        private Coroutine _fireCoolDownCoroutine;
        public Shooting(Guard guard, Transform player) : base(guard)
        {
            _guard.transform.rotation = Quaternion.LookRotation(VectorHelper.Direction(player.position, _guard.transform.position));
            _player = player;
        }
        public override void Enter()
        {
            base.Enter();
            Timer.Tick += OnTick;
            _guard.Visioncon.PlayerOutOfRange += OnPlayerOutOfRange;
            _guard.Agent.Stop();
            _guard.PlayAnimation("Shooting");
            FireAmmo();
            if (_fireCoolDownCoroutine != null)
            {
                _guard.StopCoroutine(_fireCoolDownCoroutine);
            }
            _fireCoolDownCoroutine = _guard.StartCoroutine(FireCoroutine());
        }
        public void OnTick()
        {
            float distance = Vector3.Distance(_guard.transform.position, _player.position);
            if (_tempTime < _guard.MinimumStayTime)
            {
                _tempTime += Time.deltaTime;
            }
            else
            {
                if (distance < _guard.CatchRange)
                {
                    _guard.StateManager.SwitchStateTo(new Catch(_guard, _player));
                    return;
                }
                else if (distance > _guard.ShootRange && distance < _guard.ChaseRange)
                {
                    _guard.StateManager.SwitchStateTo(new Chasing(_guard, _player));
                    return;
                }
            }
            Vector3 direction = VectorHelper.Direction(_player, _guard.transform);
            Ray ray = new Ray(_guard.ShootPostion.position, _guard.ShootPostion.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.gameObject.layer != 6)
                {
                    _guard.Agent.SetDestination(_player.position);
                }
                else
                {
                    _guard.Agent.Stop();
                }
            }
            _guard.transform.rotation = Quaternion.LookRotation(direction);
        }
        private IEnumerator FireCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_guard.FireCoolDown);
                FireAmmo();
            }
        }
        private void FireAmmo()
        {
            GameObject.Instantiate(_guard.Ammo, _guard.ShootPostion.position, _guard.transform.rotation);
        }
        private void OnPlayerOutOfRange()
        {
            _guard.StateManager.SwitchStateTo(new Searching(_guard, _player));
        }
        public override void Exit()
        {
            base.Exit();
            Timer.Tick -= OnTick;
            if (_fireCoolDownCoroutine != null)
            {
                _guard.StopCoroutine(_fireCoolDownCoroutine);
            }
            _guard.Visioncon.PlayerOutOfRange -= OnPlayerOutOfRange;
        }
    }
}