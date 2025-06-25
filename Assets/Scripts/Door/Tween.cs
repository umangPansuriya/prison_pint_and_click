using System.Collections;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public AnimationType AnimationType;
    public AnimationAxis Direction = AnimationAxis.Y;
    public float Initial;
    public float Target;
    public float Duration;
    private Coroutine _animation;
    private bool _isOpen;

    public void Open()
    {
        if (AnimationType == AnimationType.Rotate)
        {
            DoRotate();

        }
        else if (AnimationType == AnimationType.Move)
        {
            DoMove();
        }
    }
    public void Open(int direction)
    {
        if (AnimationType == AnimationType.Rotate)
        {
            DoRotate(direction);

        }
        else if (AnimationType == AnimationType.Rotate)
        {
            DoMoveCoroutine(direction);
        }
    }
    public void Close()
    {
        if (_isOpen)
        {
            if (AnimationType == AnimationType.Rotate)
            {
                DoRotate();

            }
            else if (AnimationType == AnimationType.Move)
            {
                DoMove();
            }
        }

    }
    public void DoMove(int direction = 1)
    {
        StopCoroutine();
        _animation = StartCoroutine(DoMoveCoroutine(direction));
    }
    public void DoRotate(int direction = 1)
    {
        StopCoroutine();
        _animation = StartCoroutine(DoRoateCoroutine(direction));
    }
    public void StopCoroutine()
    {
        if (_animation != null)
        {
            StopCoroutine(_animation);
        }
    }
    private IEnumerator DoRoateCoroutine(int direction)
    {
        float tt;
        if (_isOpen)
        {
            _isOpen = false;
            tt = Initial;
        }
        else
        {
            _isOpen = true;
            tt = Target * direction;
        }
        Quaternion target = new Quaternion();
        switch (Direction)
        {
            case AnimationAxis.X:
                target = Quaternion.Euler(new Vector3(tt, transform.localEulerAngles.y, transform.localEulerAngles.z));
                break;
            case AnimationAxis.Y:
                target = Quaternion.Euler(new Vector3(transform.localEulerAngles.x, tt, transform.localEulerAngles.z));
                break;
            case AnimationAxis.Z:
                target = Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, tt));
                break;
        }
        float elapsedTime = 0f;
        float t;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, target, t);
            yield return null;
        }
    }
    private IEnumerator DoMoveCoroutine(int direction)
    {
        float tt;
        if (_isOpen)
        {
            _isOpen = false;
            tt = Initial;
        }
        else
        {
            _isOpen = true;
            tt = Target * direction;
        }
        Vector3 target = new Vector3();
        switch (Direction)
        {
            case AnimationAxis.X:
                target = new Vector3(tt, transform.localPosition.y, transform.localPosition.z);
                break;
            case AnimationAxis.Y:
                target = new Vector3(transform.localPosition.x, tt, transform.localPosition.z);
                break;
            case AnimationAxis.Z:
                target = new Vector3(transform.localPosition.x, transform.localPosition.y, tt);
                break;
        }
        float elapsedTime = 0f;
        float t;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            t = elapsedTime / Duration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, t);
            yield return null;
        }
    }
}
public enum AnimationType
{
    Rotate,
    Move,
    Sacle
}
public enum AnimationAxis
{
    X, Y, Z
}
