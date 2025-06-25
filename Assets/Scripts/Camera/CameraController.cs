using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _freeCamera;
    [SerializeField] private CinemachineCamera _playerCamera;

    public void ChangeFocusTo(Transform target, float time = 2, Action OnFocusDone = null)
    {
        _freeCamera.transform.SetPositionAndRotation(_playerCamera.transform.position, _playerCamera.transform.rotation);
        _playerCamera.enabled = false;
        _freeCamera.enabled = true;
        StartCoroutine(MoveCoroutine(_freeCamera.transform.position, target.position, time, OnFocusDone));
    }
    public void ChangeFocusFromCurrentPosition(Transform target, float time = 2, Action OnFocusDone = null)
    {
        _playerCamera.enabled = false;
        _freeCamera.enabled = true;
        StartCoroutine(MoveCoroutine(_freeCamera.transform.position, target.position, time, OnFocusDone));
    }
    private IEnumerator MoveCoroutine(Vector3 startPosition, Vector3 targetPosition, float time, Action OnFocusDone = null)
    {
        float temp = 0;
        targetPosition.y = startPosition.y;
        while (temp < time)
        {
            temp += Time.deltaTime;
            _freeCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, temp / time);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        OnFocusDone?.Invoke();
    }
    public void FocusBackToPlayer()
    {
        StartCoroutine(MoveCoroutine(_freeCamera.transform.position, _playerCamera.transform.position, 2, () =>
        {
            _playerCamera.enabled = true;
            _freeCamera.enabled = false;
        }));
    }
}
