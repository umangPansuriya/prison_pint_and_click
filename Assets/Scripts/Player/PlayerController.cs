using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] private Animator _animator;
    [SerializeField] private AIAgent _agent;
    [SerializeField] private Collider _collider;

    [Space]
    [SerializeField] private GameObject _cursor;
    [SerializeField] private GameObject _maincharacter;
    [SerializeField] private Ragdool _ragDool;
    public bool IsHidden;
    public float Speed { get { return _agent.Speed; } set { _agent.Speed = value; } }
    public bool IsAgentActive { get { return _agent.isActiveAndEnabled; } }

    public event Action ReachDestination_Action;
    public event Action StartMoveing_Action;

    public event Action Hide_Action;
    public event Action Unhide_Action;

    private Coroutine _doorTravelCoroutine;
    private void Awake()
    {
        _cursor = Instantiate(_cursor);
    }
    private void Start()
    {
        _cursor.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _agent.StartMoveing_Action += OnStartMoveing;
        _agent.RechedDestination_Action += OnRechDestination;
    }
    private void OnDisable()
    {
        _agent.StartMoveing_Action -= OnStartMoveing;
        _agent.RechedDestination_Action -= OnRechDestination;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DoorContorller door))
        {
            if (door.IsAccessible(transform))
            {
                //_agent.AddLayer(3);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DoorContorller door))
        {
            //_agent.RemoveLayer(3);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ammo"))
        {
            _collider.enabled = false;
            _maincharacter.SetActive(false);
            _ragDool.ActiveRagdool();
            _ragDool.AddForceToCloset(collision.contacts[0].point);
        }
    }
    private void OnRechDestination()
    {
        _cursor.gameObject.SetActive(false);
        ReachDestination_Action?.Invoke();
    }
    private void OnStartMoveing()
    {
        StartMoveing_Action?.Invoke();
    }
    public bool Move(Vector3 position, bool canCheck = true)
    {
        if (canCheck)
        {
            if (!_agent.IsRechable(position))
            {
                return false;
            }
        }
        _agent.SetDestination(position);
        _cursor.gameObject.SetActive(true);
        _cursor.transform.position = new Vector3(position.x, 0.1f, position.z);
        return true;
    }
    public void PassOnDoor(Vector3 startPos, Vector3 endPos)
    {
        if (_doorTravelCoroutine != null)
        {
            StopCoroutine(_doorTravelCoroutine);
        }
        _doorTravelCoroutine = StartCoroutine(DoorTravelCoroutine(startPos, endPos));
    }
    private IEnumerator DoorTravelCoroutine(Vector3 startPos, Vector3 endPos)
    {
        transform.forward = endPos - transform.position;
        PlayAnimation("Walking");
        float time = 1f;
        float temp = 0;
        while (temp < time)
        {
            temp += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, temp / time);
            yield return null;
        }
        PlayAnimation("idle");
    }
    public void PlayAnimation(string name, int layer = 0, float normalizedTime = 0)
    {
        _animator.Play(name, layer, normalizedTime);
    }
    public void Hide()
    {
        IsHidden = true;
        _collider.enabled = false;
        Hide_Action?.Invoke();
        _maincharacter.SetActive(false);
    }
    public void UnHide()
    {
        IsHidden = false;
        _collider.enabled = true;
        Unhide_Action?.Invoke();
        _maincharacter.SetActive(true);
    }

}
