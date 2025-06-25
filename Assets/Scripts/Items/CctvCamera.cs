using System.Collections;
using UnityEngine;

public class CctvCamera : MonoBehaviour
{
    [SerializeField] private Vector3 StartRotation;
    [SerializeField] private Vector3 EndRotation;
    [SerializeField] private float _rotationTime;

    [SerializeField] private Transform _rotatingHandle;
    [SerializeField] private VisionCone _visionCone;



    private void OnEnable()
    {
        _visionCone.PlayerDetected += OnPlayerDetect;
    }
    private void OnDisable()
    {
        _visionCone.PlayerDetected -= OnPlayerDetect;
    }

    private void Start()
    {
        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        float temp = 0;
        while (temp < _rotationTime)
        {
            temp += Time.deltaTime;
            _rotatingHandle.localRotation = Quaternion.Euler(Vector3.Lerp(StartRotation, EndRotation, temp / _rotationTime));
            yield return null;
        }
        temp = 0;
        while (temp < _rotationTime)
        {
            temp += Time.deltaTime;
            _rotatingHandle.localRotation = Quaternion.Euler(Vector3.Lerp(EndRotation, StartRotation, temp / _rotationTime));
            yield return null;
        }
        StartCoroutine(Rotate());
    }
    private void OnPlayerDetect(Transform player)
    {
        EnemyEventBroadcaster.RisePlayerDetected(player.position);
    }
}
