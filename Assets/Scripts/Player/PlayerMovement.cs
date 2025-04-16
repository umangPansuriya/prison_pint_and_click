using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AIAgent _agent;

    [Space]
    [SerializeField] private LayerMask _layermask;

    [Space]
    [SerializeField] private GameObject _cursor;

    [Space]
    [SerializeField] private InputActionReference _click;
    [SerializeField] private InputActionReference _point;

    [Space]
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    private Camera _camera;
    private Vector2 _clickPosition;

    private void Start()
    {
        _camera = Camera.main;
        _cursor = Instantiate(_cursor);
        _cursor.gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        _click.action.started += ClickPerformed;
        _point.action.performed += PointPerformed;
        _agent.RechedDestination += OnRechDestination;
    }
    private void OnDisable()
    {
        _click.action.started -= ClickPerformed;
        _point.action.performed -= PointPerformed;
        _agent.RechedDestination -= OnRechDestination;
    }
    private void ClickPerformed(InputAction.CallbackContext obj)
    {
        Move(_clickPosition);
    }
    private void PointPerformed(InputAction.CallbackContext obj)
    {
        _clickPosition = obj.ReadValue<Vector2>();
    }
    private void Move(Vector2 position)
    {
        var pointerEventData = new PointerEventData(_eventSystem) { position = position };
        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
        {
            return;
        }

        Ray ray = _camera.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layermask))
        {
            if (hit.transform.gameObject.layer != 3) return;
            _agent.SetDestination(hit.point);
            _animator.Play("Walking");
            _cursor.gameObject.SetActive(true);
            _cursor.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
        }
    }
    private void OnRechDestination()
    {
        _animator.Play("idle");
        _cursor.gameObject.SetActive(false);
    }
}
