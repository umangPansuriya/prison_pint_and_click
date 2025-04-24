using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _runTime;
    [SerializeField] private Image _image;
    [SerializeField] private AIAgent _agent;
    [SerializeField] private Animator _animator;

    private float _tempTime;
    /*
     * player stamina 
     * stamina regenration time
     * stamina manager 
     * onstamina change
     * usestamina 
     * regenrate stamina
     * onstamina finish
     * draintime per second
     * regenrationTimer per second
     */
    private void Awake()
    {
        _tempTime = _runTime;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.Play("SlowRun");

        Timer.Tick -= Refill;
        Timer.Tick += Run;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _animator.Play("Walking");
        Timer.Tick -= Run;
        Timer.Tick += Refill;
        _agent.Speed = 2;
    }


    private void Run()
    {
        if (_tempTime > 0)
        {
            _tempTime -= Time.deltaTime;
            _image.fillAmount = _tempTime / _runTime;
            _agent.Speed = 3;
        }
        else
        {
            _animator.Play("Walking");
            _agent.Speed = 2;
            Timer.Tick -= Run;
            Timer.Tick += Refill;
        }
    }
    private void Refill()
    {
        if (_tempTime < _runTime)
        {
            _tempTime += Time.deltaTime;
            _image.fillAmount = _tempTime / _runTime;
        }
        else
        {
            Timer.Tick -= Refill;
        }
    }

}