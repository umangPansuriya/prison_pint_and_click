using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _viewDistance = 5f;
    [SerializeField] private int _rayCount = 50;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _playerLayer;
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;
    private bool _canSeePlayer;
    private bool _canFireOutOfRange;
    public event Action<Transform> PlayerDetected;
    public event Action PlayerOutOfRange;
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _meshRenderer = GetComponent<MeshRenderer>();
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }

    void LateUpdate()
    {
        DrawVision();
        CheckPlayerInView();
    }

    void DrawVision()
    {
        // Setup angles and array sizes
        float angle = -_viewAngle / 2f;
        float angleIncrement = _viewAngle / _rayCount;

        Vector3[] vertices = new Vector3[_rayCount + 2];
        int[] triangles = new int[_rayCount * 3];

        vertices[0] = Vector3.zero; // Origin of the cone (local center)

        for (int i = 0; i <= _rayCount; i++)
        {
            // Convert angle into a world-space direction from the object's forward
            Vector3 dir = DirFromAngle(angle, false); // Use 'false' to make angle relative to enemy's current rotation
            Vector3 rayOrigin = transform.position + transform.forward * 0.1f; // Slightly offset to avoid self-hit

            Vector3 vertex;

            // Raycast to detect obstacles
            //Debug.DrawRay(rayOrigin, dir * _viewDistance, Color.white);
            if (Physics.Raycast(rayOrigin, dir, out RaycastHit hit, _viewDistance, _obstacleMask))
            {
                vertex = transform.InverseTransformPoint(hit.point);
                //Debug.DrawRay(rayOrigin, dir * hit.distance, Color.red);
            }
            else
            {
                Vector3 endPoint = rayOrigin + dir * _viewDistance;
                vertex = transform.InverseTransformPoint(endPoint);
                //Debug.DrawRay(rayOrigin, dir * _viewDistance, Color.green);
            }

            vertices[i + 1] = vertex;

            // Create triangle fan
            if (i < _rayCount)
            {
                int start = i * 3;
                triangles[start] = 0;
                triangles[start + 1] = i + 1;
                triangles[start + 2] = i + 2;
            }

            angle += angleIncrement;
        }

        // Assign and update mesh
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
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
