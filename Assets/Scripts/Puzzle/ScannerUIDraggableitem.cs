using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ScannerUIDraggableitem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    [SerializeField] private RectTransform targetArea; // Assign in inspector
    [SerializeField] private InputActionReference pointPosition;
    private Vector2 _mousePosition;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.anchoredPosition;
    }
    private void OnEnable()
    {
        pointPosition.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        _mousePosition = obj.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        pointPosition.action.performed -= Action_performed;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // So it doesn't block raycasts on drop
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.root.localScale.x;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, _mousePosition))
        {
            rectTransform.position = targetArea.position; // Snap to target
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition; // Reset to original
        }
    }
}