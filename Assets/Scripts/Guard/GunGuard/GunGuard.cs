using UnityEditor;
using UnityEngine;

namespace NPC.Guard
{
    public class GunGuard : BaseGuard
    {
        public Transform ShootPostion;
        public GameObject Ammo;

        [Space]
        public float CatchRange = 2;
        public float ShootRange = 4;
        public float ChaseRange = 6;
        public float SearchRange = 8;

        [Space]
        public float WalkSpeed = 2;
        public float ChaseSpeed = 2.5f;
        public float CatchSpeed = 3;

        [Space]
        public float FireCoolDown = 0.1f;
        public float MinimumStayTime = 1;
        protected override void OnEnable()
        {
            base.OnEnable();
            EnemyEventBroadcaster.Alert_Action += OnAlert;
            GameEvent.GameOver += OnGameOver;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EnemyEventBroadcaster.Alert_Action -= OnAlert;
            GameEvent.GameOver += OnGameOver;
        }
        protected override void Start()
        {
            base.Start();
            StateManager.SwitchStateTo(new GunGuardStanding(this));
        }
        private void OnDrawGizmos()
        {
            if (!CanShowGizom) return;
#if UNITY_EDITOR
            Handles.Label(transform.position + (Vector3.up * 2), _currentState);
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, CatchRange);
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, Vector3.up, ShootRange);
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, ChaseRange);
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.up, SearchRange);
#endif
        }

        private void OnAlert(Vector3 position)
        {
            StateManager.SwitchStateTo(new GunGuardWalking(this, position));
        }
        private void OnGameOver()
        {
            Agent.Stop();
            SwitchStateTo(new GunGuardIdle(this));
            gameObject.SetActive(false);
        }
        public void PlayCurrentPointAnimation()
        {
            PlayAnimation(_pathList[_currentPointIndex].Animation.name);
        }
        public void SwitchStateTo(GunGuardState state)
        {
            StateManager.SwitchStateTo(state);
        }

    }
}