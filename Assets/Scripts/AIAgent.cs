using System;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Tooltip("if is true so rotation will be perform by unity")]
    [SerializeField] private bool _canCustomRotate = false;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private NavMeshAgent _agent;
    public event Action RechedDestination_Action;

    private float _distanceDelta = 0.1f;
    public event Action StartMoveing_Action;
    public float Speed
    {
        get { return _agent.speed; }
        set { _agent.speed = value; }
    }
    public bool IsOnOffMeshLink { get { return _agent.isOnOffMeshLink; } }
    public OffMeshLinkData CurrentOffMeshLinkData { get { return _agent.currentOffMeshLinkData; } }

    public bool IsRechable(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        bool value = _agent.enabled;
        _agent.enabled = true;
        _agent.CalculatePath(target, path);
        _agent.enabled = value;
        return path.status == NavMeshPathStatus.PathComplete;
    }
    public void SetDestination(Vector3 positon)
    {
        this.enabled = true;
        _agent.enabled = true;
        StartMoveing_Action?.Invoke();
        _agent.SetDestination(positon);
    }
    public void Stop()
    {
        SetDestination(transform.position);
    }
    private void Update()
    {
        if (_canCustomRotate && _agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        if (!_agent.pathPending && _agent.remainingDistance < _distanceDelta)
        {
            _agent.enabled = false;
            this.enabled = false;
            RechedDestination_Action?.Invoke();
        }
    }
    private void OnValidate()
    {
        _agent.updateRotation = !_canCustomRotate;
    }
    public void AddLayer(int layer)
    {
        _agent.areaMask |= (1 << layer);
    }
    public void RemoveLayer(int layer)
    {
        _agent.areaMask &= ~(1 << layer);
    }
}
