using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class IceCreamConeGenerator : MonoBehaviour
{
    public float _coneHeight = 5f;
    public float _coneRadius = 3f;
    public int _radialSegments = 36;
    public float _maxCastDistance = 8f;         // ← new: cap how far we’ll raycast
    public LayerMask _obstacleMask;

    private Mesh _mesh;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    void LateUpdate()
    {
        GenerateConeMesh();
    }

    void GenerateConeMesh()
    {
        _mesh.Clear();

        // 1. prepare vertex array (apex + one ring)
        Vector3[] vertices = new Vector3[_radialSegments + 1];
        vertices[0] = Vector3.zero;  // apex

        Vector3 worldStart = transform.position;
        float fullDistance = _coneHeight + _coneRadius;
        float rayDistance = Mathf.Min(fullDistance, _maxCastDistance);

        // 2. for each segment, raycast up to rayDistance
        for (int i = 0; i < _radialSegments; i++)
        {
            float angle = (2 * Mathf.PI / _radialSegments) * i;
            //Vector3 localTarget = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _coneRadius
            //          + Vector3.up * _coneHeight;
            Vector3 localTarget = new Vector3(Mathf.Cos(angle) * _coneRadius, Mathf.Sin(angle) * _coneRadius, _coneHeight);
            Vector3 worldTarget = transform.TransformPoint(localTarget);
            Vector3 dir = (worldTarget - worldStart).normalized;

            // if we hit within rayDistance, pull the vertex to hit.point
            if (Physics.Raycast(worldStart, dir, out RaycastHit hit, rayDistance, _obstacleMask))
            {
                vertices[i + 1] = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                // either no hit *or* the object is beyond rayDistance:
                // clamp the vertex at worldStart + dir * rayDistance
                Vector3 clampedEnd = worldStart + dir * rayDistance;
                vertices[i + 1] = transform.InverseTransformPoint(clampedEnd);
            }
        }

        // 3. build every triangle (apex→i→i+1)
        List<int> tris = new List<int>();
        for (int i = 0; i < _radialSegments; i++)
        {
            int next = (i + 1) % _radialSegments;
            tris.Add(0);
            tris.Add(i + 1);
            tris.Add(next + 1);
        }

        // 4. push into mesh
        _mesh.vertices = vertices;
        _mesh.triangles = tris.ToArray();
        _mesh.RecalculateNormals();
    }
}
