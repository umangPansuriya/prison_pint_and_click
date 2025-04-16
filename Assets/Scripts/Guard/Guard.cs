using Prison.PatrollingGurd;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NPC.Guard
{
    public class Guard : MonoBehaviour
    {
        public bool CanShowGizom = true;

        [SerializeField] private Animator _animator;
        [SerializeField] private PatrollingPath _path;
        public Transform ShootPostion;
        public AIAgent Agent;
        public VisionCone Visioncon;
        public GameObject Ammo;

        [Space]
        public float CatchRange;
        public float ShootRange;
        public float ChaseRange;
        public float SearchRange;

        [Space]
        public float WalkSpeed;
        public float ChaseSpeed;
        public float CatchSpeed;

        [Space]
        public float FireCoolDown;
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
            Visioncon.PlayerDetected += OnPlyerDetect;
            StateManager.StateChanged += OnStateChanged;
        }
        private void OnDisable()
        {
            Visioncon.PlayerDetected -= OnPlyerDetect;
            StateManager.StateChanged -= OnStateChanged;
        }
        private void Start()
        {
            _pathList = _path.GetPath();
            StateManager.SwitchStateTo(new Standing(this));
        }
        private void OnStateChanged(State state)
        {
            _currentState = state.ToString();
        }
        private void OnPlyerDetect(Transform player)
        {
            float distance = Vector3.Distance(transform.position, player.position);
        }
        public void PlayAnimation(string name)
        {
            _animator.Play(name);
        }
        public void PlayCurrentPointAnimation()
        {
            PlayAnimation(_pathList[_currentPointIndex].Animation.name);
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
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, Vector3.up, ShootRange);
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