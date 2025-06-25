
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class YellowCircleController : MonoBehaviour, IBeginDragHandler, IDragHandler
//{
//    public RectTransform childTransform;
//    public float minY = 0f;
//    public float maxY = 300f;

//    private RectTransform rectTransform;
//    private float initialAngle;
//    private float currentZRotation = 0f;
//    private float previousAngle;
//    private float netRotation = 0f;

//    void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        Vector2 startVector = eventData.position - (Vector2)rectTransform.position;
//        previousAngle = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        Vector2 currentVector = eventData.position - (Vector2)rectTransform.position;
//        float currentAngle = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg;

//        float deltaAngle = Mathf.DeltaAngle(previousAngle, currentAngle);
//        netRotation += deltaAngle;

//        // Clamp rotation between 0 and 1080
//        netRotation = Mathf.Clamp(netRotation, 0f, 1080f);

//        // Update rotation
//        rectTransform.rotation = Quaternion.Euler(0, 0, netRotation);

//        // Update green Y position
//        float progress = netRotation / 1080f;
//        float newY = Mathf.Lerp(minY, maxY, progress);
//        childTransform.anchoredPosition = new Vector2(childTransform.anchoredPosition.x, newY);

//        previousAngle = currentAngle;
//    }
//}
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class YellowCircleController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public enum MoveDirection { X, Y }
    public MoveDirection moveDirection = MoveDirection.Y; // Default is Y

    public RectTransform greenSquare;
    public float minValue = 0f;
    public float maxValue = 300f;

    private RectTransform rectTransform;
    private float previousAngle;
    private float netRotation = 0f;


    public float reverseDuration = 3f; // In seconds (Editor ma set karo)
    private bool allowInput = true;
    private bool isReversing = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 startVector = eventData.position - (Vector2)rectTransform.position;
        previousAngle = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentVector = eventData.position - (Vector2)rectTransform.position;
        float currentAngle = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg;

        float deltaAngle = Mathf.DeltaAngle(previousAngle, currentAngle);
        netRotation += deltaAngle;
        netRotation = Mathf.Clamp(netRotation, 0f, 1080f);

        // Apply rotation to yellow circle
        rectTransform.rotation = Quaternion.Euler(0, 0, netRotation);

        // Calculate progress (0 to 1)
        float progress = netRotation / 1080f;
        float newValue = Mathf.Lerp(minValue, maxValue, progress);

        // Apply to greenSquare based on selected axis
        Vector2 pos = greenSquare.anchoredPosition;
        if (moveDirection == MoveDirection.Y)
            pos.y = newValue;
        else
            pos.x = newValue;

        greenSquare.anchoredPosition = pos;

        previousAngle = currentAngle;
        // Check if greenSquare has reached maxValue
        if (newValue >= maxValue && !isReversing)
        {
            allowInput = false;
            StartCoroutine(ReverseRotation());
        }
    }


    private IEnumerator ReverseRotation()
    {
        yield return new WaitForSeconds(1f);
        isReversing = true;

        float startRotation = netRotation;
        float endRotation = 0f;

        float startValue;
        float endValue;
        if (moveDirection == MoveDirection.Y)
        {
            startValue = greenSquare.anchoredPosition.y;
            endValue = minValue;
        }
        else
        {
            startValue = greenSquare.anchoredPosition.x;
            endValue = minValue;
        }

        float elapsed = 0f;
        while (elapsed < reverseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / reverseDuration;

            // Rotate backward
            //netRotation = Mathf.Lerp(startRotation, endRotation, t);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            netRotation = Mathf.Lerp(startRotation, endRotation, smoothT);
            rectTransform.rotation = Quaternion.Euler(0, 0, netRotation);

            // Move greenSquare back
            //float newValue = Mathf.Lerp(startValue, endValue, t);
            float newValue = Mathf.Lerp(startValue, endValue, smoothT);
            Vector2 pos = greenSquare.anchoredPosition;
            if (moveDirection == MoveDirection.Y)
                pos.y = newValue;
            else
                pos.x = newValue;

            greenSquare.anchoredPosition = pos;

            yield return null;
        }

        // Ensure final values are exact
        netRotation = 0f;
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        Vector2 finalPos = greenSquare.anchoredPosition;
        if (moveDirection == MoveDirection.Y)
            finalPos.y = minValue;
        else
            finalPos.x = minValue;

        greenSquare.anchoredPosition = finalPos;
        isReversing = false;
        allowInput = true;
    }
}
