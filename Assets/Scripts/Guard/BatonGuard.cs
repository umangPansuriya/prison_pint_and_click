using Prison.PatrollingGurd;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace NPC.BatonGuard
{
    public class BatonGuard : MonoBehaviour
    {
        public bool CanShowGizom = true;

        [SerializeField] private Animator _animator;
        [SerializeField] private PatrollingPath _path;

        public AIAgent Agent;
        public VisionCone Visioncon;

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
        private List<PatrollingPoint> _pathList;


        private int _currentPointIndex;

        public StateManager StateManager;
        private string _currentState;

        private void Awake()
        {
            StateManager = new StateManager();
        }
        private void OnEnable()
        {
            StateManager.StateChanged += OnStateChanged;
        }
        private void OnDisable()
        {
            StateManager.StateChanged -= OnStateChanged;
        }
        private void Start()
        {
            _pathList = _path.GetPath();
            StateManager.SwitchStateTo(new BatonStanding(this));
        }
        private void OnStateChanged(State state)
        {
            _currentState = state.ToString();
        }
        public void PlayAnimation(string name)
        {
            _animator.Play(name);
        }
        public void GotoNextPoint()
        {
            _currentPointIndex = (_currentPointIndex + 1) % _pathList.Count;
            Agent.SetDestination(_pathList[_currentPointIndex].transform.position);
        }
        public PatrollingPoint GetCurrentPoint()
        {
            return _pathList[_currentPointIndex];
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
        private void OnDestroy()
        {
            StateManager.Destroy();
        }
    }
}

