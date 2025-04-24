using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Space]
    [SerializeField] private LayerMask _layermask;

    [Header("Componets")]
    [SerializeField] private Animator _animator;
    [SerializeField] private AIAgent _agent;

    [Space]
    [SerializeField] private GameObject _cursor;

    [Space]
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    [Space]
    [Header("Input Actions")]
    [SerializeField] private InputActionReference _click;
    [SerializeField] private InputActionReference _point;
    [SerializeField] private InputActionReference _doubleClick;


    private Camera _camera;
    private Vector2 _clickPosition;
    private void Awake()
    {
        _camera = Camera.main;
        _cursor = Instantiate(_cursor);
    }
    private void Start()
    {
        _cursor.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //_click.action.started += ClickPerformed;
        _doubleClick.action.started += DoubleClickStarted;
        //_doubleClick.action.performed += DoubleClickPerformed;
        _point.action.performed += PointPerformed;
        _agent.RechedDestination += OnRechDestination;
    }



    private void OnDisable()
    {
        //_click.action.started -= ClickPerformed;
        _doubleClick.action.started -= DoubleClickStarted;
        //_doubleClick.action.performed -= DoubleClickPerformed;
        _point.action.performed -= PointPerformed;
        _agent.RechedDestination -= OnRechDestination;
    }

    private void ClickPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Click");
        _animator.Play("Walking");
        Move(_clickPosition);
    }

    private void DoubleClickStarted(InputAction.CallbackContext obj)
    {
        _animator.Play("Walking");
        Move(_clickPosition);
    }
    private void DoubleClickPerformed(InputAction.CallbackContext obj)
    {
        _animator.Play("SlowRun");
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
            if (hit.transform.gameObject.layer == 3)
            {
                if (!_agent.IsRechable(hit.point)) return;
                _agent.SetDestination(hit.point);

                _cursor.gameObject.SetActive(true);
                _cursor.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
            }
            else if (hit.transform.gameObject.layer == 8)
            {

                _agent.SetDestination(hit.point);
                _cursor.gameObject.SetActive(true);
                _cursor.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
            }
        }
    }
    private void OnRechDestination()
    {
        _animator.Play("idle");
        _cursor.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DoorContorller door))
        {
            _agent.Stop();
            if (door.IsAccessible(transform))
            {
                _agent.AddLayer(3);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DoorContorller door))
        {
            _agent.RemoveLayer(3);
        }
    }

}
