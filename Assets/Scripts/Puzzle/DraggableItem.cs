using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SortPuzzleController sort_pzl_ctrl;
    private Transform parent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private int originalIndex;
    public Transform originalParent;
    public int idIndex;

    public Transform canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parent = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalIndex = transform.GetSiblingIndex();
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(canvas.transform); // So it drags over everything

    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(.5f, .5f);
        rectTransform.anchorMax = new Vector2(.5f, .5f);
        //rectTransform.anchoredPosition = new Vector2(0, rectTransform.localPosition.y);
        Canvas.ForceUpdateCanvases();
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            originalParent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            // Only update Y position
            rectTransform.anchoredPosition = new Vector2(0, localPoint.y);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;

        int newIndex = originalParent.childCount;

        for (int i = 0; i < originalParent.childCount; i++)
        {
            if (transform.position.y > originalParent.GetChild(i).position.y)
            {
                newIndex = i;
                break;
            }
        }

        // Clamp to avoid going out of range
        newIndex = Mathf.Clamp(newIndex, 0, originalParent.childCount);
        transform.SetSiblingIndex(newIndex);
        sort_pzl_ctrl.CheckSequence();
    }


}
