using System.Collections;
using UnityEngine;

public class CctvCamera : MonoBehaviour
{
    [SerializeField] private Vector3 StartRotation;
    [SerializeField] private Vector3 EndRotation;
    [SerializeField] private float _rotationTime;

    [SerializeField] private Transform _rotatingHandle;


    private void Start()
    {
        StartCoroutine(Rotate());
    }
    private void Update()
    {
    }
    private IEnumerator Rotate()
    {
        float temp = 0;
        while (temp < _rotationTime)
        {
            temp += Time.deltaTime;
            _rotatingHandle.localRotation = Quaternion.Euler(Vector3.Lerp(StartRotation, EndRotation, temp / _rotationTime));
            yield return new WaitForEndOfFrame();
        }
        temp = 0;
        while (temp < _rotationTime)
        {
            temp += Time.deltaTime;
            _rotatingHandle.localRotation = Quaternion.Euler(Vector3.Lerp(EndRotation, StartRotation, temp / _rotationTime));
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Rotate());
    }
}
