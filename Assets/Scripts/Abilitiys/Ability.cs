using System;
using System.Collections;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float _maxTime;
    [SerializeField] protected float _regenTime;

    [SerializeField] protected float _regenRate;//total regenration time = _maxTime/_regenRate


    protected float _currentTime;

    public event Action AbilityChanged_Action;
    protected Coroutine _coroutine;

    private void OnValidate()
    {
        _regenRate = _maxTime / _regenTime;

    }
    private void Awake()
    {
        _currentTime = _maxTime;
    }
    protected IEnumerator UseAbility()
    {
        OnStartUsing();
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Max(_currentTime, 0);
            AbilityChanged_Action?.Invoke();
            yield return null;
        }
        StopCoroutine();
        _coroutine = StartCoroutine(RegenAbility());
    }
    protected IEnumerator RegenAbility()
    {
        OnStopUsing();
        while (_currentTime < _maxTime)
        {
            _currentTime += Time.deltaTime * _regenRate;
            _currentTime = Mathf.Min(_currentTime, _maxTime);
            AbilityChanged_Action?.Invoke();
            yield return null;
        }
    }
    protected void StopCoroutine()
    {
        if (_coroutine != null)
        {
            base.StopCoroutine(_coroutine);
        }
    }
    protected abstract void OnStartUsing();
    protected abstract void OnStopUsing();
}
