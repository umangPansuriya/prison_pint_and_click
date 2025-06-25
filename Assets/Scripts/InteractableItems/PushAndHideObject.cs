using System.Collections;
using UnityEngine;

public class PushAndHideObject : Interactale, IHidable, IPushable
{
    [SerializeField] private float _timeToMove;
    [SerializeField] private Transform _playerPlace;
    [SerializeField] private Transform _target;
    [SerializeField] private ObjectInteractPanel _interactPanel;

    public bool CanPush
    {
        get
        {
            return Vector3.Distance(transform.position, _target.position) > 0.5f;
        }
    }

    private Coroutine _coroutine;

    public override void OnInteract(PlayerController player)
    {
        _player = player;
        _player.Move(_playerPlace.position);
        _player.ReachDestination_Action += OnReachDestination;
    }
    public void OnReachDestination()
    {
        _interactPanel.Show(this);
    }
    public override void Deselect()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        base.Deselect();
        _player.ReachDestination_Action -= OnReachDestination;
    }
    public void Hide()
    {
        _player.Hide();
    }
    public void Push()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _player.transform.rotation = Quaternion.LookRotation(_playerPlace.forward);
        _player.PlayAnimation("Push");
        _coroutine = StartCoroutine(PushCoroutine());
    }
    private IEnumerator PushCoroutine()
    {
        float tempTime = 0;
        Vector3 startPosition = transform.position;
        while (tempTime <= _timeToMove)
        {
            tempTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, _target.position, tempTime / _timeToMove);
            _player.transform.position = new Vector3(_playerPlace.position.x, _player.transform.position.y, _playerPlace.position.z);
            yield return null;
        }
        Deselect();
        _player.PlayAnimation("idle");
    }


}
