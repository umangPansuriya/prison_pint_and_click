using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Prison.PatrollingGurd
{
    public class PatrollingPath : MonoBehaviour
    {

        [SerializeField] private List<PatrollingPoint> _pathPoints = new List<PatrollingPoint>();

        [SerializeField] private GameObject _startPrefab;
        [SerializeField] private GameObject _circlePrefab;

        [Tooltip("it margin or space between two dots")]
        [SerializeField] private float _spacing = 1f;

        private Transform _pathPointsContainer;
        private Transform _dotsContainer;
        private void EnsureContainersExist()
        {
            _pathPointsContainer = transform.Find("PathPoints") ?? new GameObject("PathPoints").transform;
            _pathPointsContainer.SetParent(transform);

            _dotsContainer = transform.Find("Dots") ?? new GameObject("Dots").transform;
            _dotsContainer.SetParent(transform);
        }
        public void AddPoint()
        {
            EnsureContainersExist();

            if (_pathPoints == null) _pathPoints = new List<PatrollingPoint>();

            GameObject node = new GameObject("Node" + _pathPoints.Count);
            node.transform.SetParent(_pathPointsContainer);
            var patrollingPoint = node.AddComponent<PatrollingPoint>();
            _pathPoints.Add(patrollingPoint);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(node, "Add Patrol Point");
            EditorUtility.SetDirty(this);  // Ensure Unity saves it
#endif
        }

        public void RemovePoint()
        {
            if (_pathPoints.Count > 0)
            {
                DestroyImmediate(_pathPoints[_pathPoints.Count - 1].gameObject);
                _pathPoints.RemoveAt(_pathPoints.Count - 1);
            }
        }

        public void DrawPath()
        {
            if (_pathPoints.Count < 2)
            {
                Debug.LogWarning("Not enough points to draw a path!");
                return;
            }

            if (_startPrefab == null || _circlePrefab == null)
            {
                Debug.LogError("Assign all prefabs!");
                return;
            }

            EnsureContainersExist();
            ClearDots();

            float prefabSize = GetPrefabSize(_circlePrefab);
            float totalSpacing = prefabSize + _spacing;


            Instantiate(_startPrefab, _pathPoints[0].transform.position, Quaternion.Euler(90, 0, 0), _dotsContainer);

            for (int i = 1; i < _pathPoints.Count; i++)
            {
                Vector3 start = _pathPoints[i - 1].transform.position;
                Vector3 end = _pathPoints[i].transform.position;
                float distance = Vector3.Distance(start, end);

                int dotCount = Mathf.Max(1, Mathf.RoundToInt(distance / totalSpacing));
                float adjustedSpacing = distance / dotCount;

                Vector3 direction = (end - start).normalized;

                for (int j = 1; j <= dotCount; j++)
                {
                    Vector3 spawnPos = start + direction * (j * adjustedSpacing);
                    Instantiate(_circlePrefab, spawnPos, Quaternion.Euler(90, 0, 0), _dotsContainer);
                }
            }


            Instantiate(_startPrefab, _pathPoints[_pathPoints.Count - 1].transform.position, Quaternion.Euler(90, 0, 0), _dotsContainer);
        }

        public void ClearDots()
        {
            EnsureContainersExist();
            List<GameObject> children = new List<GameObject>();

            foreach (Transform child in _dotsContainer)
            {
                children.Add(child.gameObject);
            }

            foreach (GameObject child in children)
            {
                DestroyImmediate(child);
            }
        }

        private float GetPrefabSize(GameObject prefab)
        {
            if (prefab == null) return 1f;

            Renderer rend = prefab.GetComponent<Renderer>();

            float size = 1f;
            if (rend != null)
            {
                size = rend.bounds.size.magnitude / 2;
            }

            return size;
        }
        public List<PatrollingPoint> GetPath()
        {
            return _pathPoints;
        }

        public void ToggleGizoms()
        {
            EnsureContainersExist();
            _pathPointsContainer.gameObject.SetActive(!_pathPointsContainer.gameObject.activeInHierarchy);
        }
    }
}

