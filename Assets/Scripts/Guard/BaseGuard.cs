using Prison.PatrollingGurd;
using System.Collections.Generic;
using UnityEngine;

public class BaseGuard : MonoBehaviour
{
    public bool CanShowGizom = true;

    [Space]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected PatrollingPath _path;

    [Space]
    public AIAgent Agent;
    public VisionCone Visioncon;

    public StateManager StateManager;


    protected int _currentPointIndex;
    [SerializeField] protected string _currentState;
    protected List<PatrollingPoint> _pathList;

    protected virtual void Awake()
    {
        StateManager = new StateManager();
    }
    protected virtual void OnEnable()
    {
        StateManager.StateChanged += OnStateChanged;
    }
    protected virtual void OnDisable()
    {
        StateManager.StateChanged -= OnStateChanged;
    }
    protected virtual void Start()
    {
        _pathList = _path.GetPath();
    }
    protected virtual void OnDestroy()
    {
        StateManager.Destroy();
    }
    protected virtual void OnStateChanged(State state)
    {
        _currentState = state.ToString();
    }
    public void PlayAnimation(string name)
    {
        _animator.Play(name);
    }
    public Vector3 GetNextPointPosition()
    {
        _currentPointIndex = (_currentPointIndex + 1) % _pathList.Count;
        return _pathList[_currentPointIndex].transform.position;
    }
    public PatrollingPoint GetCurrentPoint()
    {
        return _pathList[_currentPointIndex];
    }
}
