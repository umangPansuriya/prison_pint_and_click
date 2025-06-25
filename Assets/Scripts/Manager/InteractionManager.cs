using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [Space]
    [Header("Input Actions")]
    [SerializeField] private InputActionReference _point;
    [SerializeField] private InputActionReference _doubleClick;

    [Space]
    [SerializeField] private PlayerController _player;

    [Space]
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    [Space]
    [SerializeField] private LayerMask _layermask;

    private Vector2 _clickPosition;
    private Camera _camera;
    private Interactale _currentInteractable;

    private void Awake()
    {
        _camera = Camera.main;
    }
    private void OnEnable()
    {
        _doubleClick.action.started += DoubleClickStarted;
        _point.action.performed += PointPerformed;
        GameEvent.GameOver += OnGameOver;
    }
    private void OnDisable()
    {
        _doubleClick.action.started -= DoubleClickStarted;
        _point.action.performed -= PointPerformed;
        GameEvent.GameOver -= OnGameOver;
    }
    private void DoubleClickStarted(InputAction.CallbackContext obj)
    {
        if (_player.IsHidden) return;
        Vector3 position = _clickPosition;

        if (IsHittingUI(position)) return;
        Ray ray = _camera.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layermask))
        {
            _currentInteractable?.Deselect();
            _currentInteractable = null;
            if (hit.transform.gameObject.layer == 3)
            {
                _player.Move(hit.point);
            }
            else if (hit.transform.gameObject.layer == 8)
            {
                _currentInteractable = hit.transform.GetComponent<Interactale>();
                _currentInteractable.OnSelect();
                _currentInteractable.OnInteract(_player);
            }
        }
    }
    private bool IsHittingUI(Vector3 position)
    {
        var pointerEventData = new PointerEventData(_eventSystem) { position = position };
        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            return true;
        }
        return false;
    }
    private void PointPerformed(InputAction.CallbackContext obj)
    {
        _clickPosition = obj.ReadValue<Vector2>();
    }
    private void OnGameOver()
    {
        this.enabled = false;
    }
}
