using System;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Tooltip("if is true so rotation will be perform by unity")]
    [SerializeField] private bool _canCustomRotate = false;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private NavMeshAgent _agent;
    public event Action RechedDestination;

    private float _distanceDelta = 0.5f;
    public float Speed
    {
        get { return _agent.speed; }
        set { _agent.speed = value; }
    }
    public void SetDestination(Vector3 positon)
    {
        this.enabled = true;
        _agent.enabled = true;
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
            this.enabled = false;
            RechedDestination?.Invoke();
        }
    }
    private void OnValidate()
    {
        _agent.updateRotation = !_canCustomRotate;
    }
}
