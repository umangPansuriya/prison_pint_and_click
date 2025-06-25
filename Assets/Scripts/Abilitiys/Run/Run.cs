using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Run : Ability, IPointerDownHandler, IPointerUpHandler
{
    [Header("Contoller Variables")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    [SerializeField] private Image _image;

    [SerializeField] private PlayerController _player;
    [SerializeField] private InputActionReference _leftshiftAction;
    [SerializeField] private InputActionReference _doubleClick;

    private bool _isPressed;

    private void OnEnable()
    {
        AbilityChanged_Action += OnAbilityChanged;
        _player.StartMoveing_Action += OnStartMoving;
        _player.ReachDestination_Action += OnReachDestination;
        _leftshiftAction.action.started += RunClick_Started;
        _leftshiftAction.action.canceled += RunClick_Canceled;
        _doubleClick.action.performed += DoubleClick_Performed;
        _doubleClick.action.canceled += DoubleClick_Canceled;
    }
    private void OnDisable()
    {
        AbilityChanged_Action -= OnAbilityChanged;
        _player.StartMoveing_Action -= OnStartMoving;
        _player.ReachDestination_Action -= OnReachDestination;
        _leftshiftAction.action.started -= RunClick_Started;
        _leftshiftAction.action.canceled -= RunClick_Canceled;
        _doubleClick.action.performed -= DoubleClick_Performed;
        _doubleClick.action.canceled -= DoubleClick_Canceled;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        TryToUseAbility();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        StopUsingAbility();
    }
    private void RunClick_Started(InputAction.CallbackContext obj)
    {
        _isPressed = true;
        TryToUseAbility();
    }
    private void RunClick_Canceled(InputAction.CallbackContext obj)
    {
        _isPressed = false;
        StopUsingAbility();
    }
    private void DoubleClick_Performed(InputAction.CallbackContext obj)
    {
        TryToUseAbility();
    }
    private void DoubleClick_Canceled(InputAction.CallbackContext obj)
    {
        if (!_isPressed)
        {
            StopUsingAbility();
        }
    }

    private void TryToUseAbility()
    {
        if (_currentTime > 0 && _player.IsPlayerMoving)
        {
            _player.Speed = _runSpeed;
            StopCoroutine();
            _coroutine = StartCoroutine(UseAbility());
        }
    }
    private void StopUsingAbility()
    {
        _player.Speed = _walkSpeed;
        if (_currentTime < _maxTime)
        {
            StopCoroutine();
            _coroutine = StartCoroutine(RegenAbility());
        }
    }
    private void OnAbilityChanged()
    {
        _image.fillAmount = _currentTime / _maxTime;
    }

    protected override void OnStartUsing()
    {
        _player.PlayAnimation("SlowRun");
    }
    protected override void OnStopUsing()
    {
        _player.Speed = _walkSpeed;
        if (_player.IsPlayerMoving)
        {
            _player.PlayAnimation("Walking");
        }
    }

    private void OnStartMoving()
    {
        if (_isPressed)
        {
            TryToUseAbility();
        }
        if (_player.Speed != _runSpeed)
        {
            _player.PlayAnimation("Walking");
        }
        else
        {
            _player.PlayAnimation("SlowRun");
        }
    }
    private void OnReachDestination()
    {
        StopUsingAbility();
        _player.PlayAnimation("idle", 0, 0);
    }
}
