using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _viewDistance = 5f;
    [SerializeField] private int _rayCount = 50;

    [Space]
    [SerializeField] private float _detectionFrequency;

    [Space]
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private MeshRenderer _meshRenderer;

    private Mesh _mesh;

    private bool _canSeePlayer;
    private bool _canFireOutOfRange;

    public event Action<Transform> PlayerDetected;
    public event Action PlayerOutOfRange;

    private float _angle;
    private float _angleIncrement;

    private float _tick;

    private int[] _triangles;
    private Vector3[] _vertices;

    private void OnValidate()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }
    void LateUpdate()
    {
        DrawVision();
        if (_tick > _detectionFrequency)
        {
            _tick = 0;
            CheckPlayerInView();
        }
        else
        {
            _tick += Time.deltaTime;
        }
    }
    void DrawVision()
    {
        _angle = -_viewAngle / 2f;
        _angleIncrement = _viewAngle / _rayCount;
        _vertices = new Vector3[_rayCount + 2];
        _triangles = new int[_rayCount * 3];
        _vertices[0] = Vector3.zero;

        for (int i = 0; i <= _rayCount; i++)
        {
            Vector3 dir = DirFromAngle(_angle, false);
            Vector3 rayOrigin = transform.position + transform.forward * 0.1f;

            Vector3 vertex;

            if (Physics.Raycast(rayOrigin, dir, out RaycastHit hit, _viewDistance, _obstacleMask))
            {
                vertex = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                Vector3 endPoint = rayOrigin + dir * _viewDistance;
                vertex = transform.InverseTransformPoint(endPoint);
            }

            _vertices[i + 1] = vertex;

            if (i < _rayCount)
            {
                int start = i * 3;
                _triangles[start] = 0;
                _triangles[start + 1] = i + 1;
                _triangles[start + 2] = i + 2;
            }

            _angle += _angleIncrement;
        }
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool global)
    {
        if (!global)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    void CheckPlayerInView()
    {

        _canSeePlayer = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, _viewDistance, _playerLayer);

        foreach (var hit in hits)
        {
            Vector3 playerPosition = hit.transform.position;
            playerPosition.y = 0.1f;
            Vector3 dirToPlayer = (playerPosition - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleToPlayer < _viewAngle / 2f)
            {
                if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hitInfo, _viewDistance))
                {
                    if (hitInfo.transform.gameObject.layer == 6)
                    {
                        _canSeePlayer = true;
                        _canFireOutOfRange = true;
                        PlayerDetected?.Invoke(hit.transform);
                        break;
                    }
                }
            }
        }
        if (_canFireOutOfRange && !_canSeePlayer)
        {
            _canFireOutOfRange = false;
            PlayerOutOfRange?.Invoke();
        }
        _meshRenderer.material.color = _canSeePlayer ? Color.red : Color.green;
    }

    //private void OnDrawGizmos()
    //{
    //    Handles.DrawWireDisc(transform.position, Vector3.up, _viewDistance);
    //}
}
