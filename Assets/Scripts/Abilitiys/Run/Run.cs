using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Run : Ability, IPointerDownHandler, IPointerUpHandler
{
    [Header("Contoller Variables")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    [SerializeField] private Image _image;

    [SerializeField] private AIAgent _playerAgent;
    public void Awake()
    {
        _tempTime = _abilityTime;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_tempTime > 0)
        {
            Timer.Tick += Use;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_tempTime < _abilityRegenrationTime)
        {
            Timer.Tick += Regenrate;
        }
    }
    private void Use()
    {
        if (_tempTime > 0)
        {
            _tempTime -= Time.deltaTime;
            _image.fillAmount = _tempTime / _abilityTime;
        }
        else
        {
            Timer.Tick -= Use;
        }
    }
    private void Regenrate()
    {
        if (_tempTime < _abilityRegenrationTime)
        {
            _tempTime += Time.deltaTime;
            _image.fillAmount = _tempTime / _abilityRegenrationTime;
        }
        else
        {
            Timer.Tick -= Regenrate;
        }
    }
}
