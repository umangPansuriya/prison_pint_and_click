using UnityEditor;
using UnityEngine;
namespace NPC.BatonGuard
{
    public class BatonGuard : BaseGuard
    {
        [Space]
        public float CatchRange;
        public float ChaseRange;
        public float SearchRange;

        [Space]
        public float WalkSpeed;
        public float ChaseSpeed;
        public float CatchSpeed;

        [Space]
        public float MinimumStayTime = 1;

        protected override void Start()
        {
            base.Start();
            StateManager.SwitchStateTo(new BatonStanding(this));
        }
        private void OnDrawGizmos()
        {
            if (!CanShowGizom) return;
#if UNITY_EDITOR
            Handles.Label(transform.position + (Vector3.up * 2), _currentState);
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, CatchRange);
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, ChaseRange);
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.up, SearchRange);
#endif
        }
    }
}

