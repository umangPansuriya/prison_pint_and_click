using Prison.PatrollingGurd;
using UnityEditor;
using UnityEngine;

namespace Prison.PrisonEditor
{
    [CustomEditor(typeof(PatrollingPath))]
    public class PatrollingPathEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            PatrollingPath patrollingPath = (PatrollingPath)target;

            EditorGUILayout.Space();
            if (GUILayout.Button("App Point"))
            {
                patrollingPath.AddPoint();
            }
            if (GUILayout.Button("Remove Point"))
            {
                patrollingPath.RemovePoint();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Draw Path"))
            {
                patrollingPath.DrawPath();
            }
            if (GUILayout.Button("Clear Path"))
            {
                patrollingPath.ClearDots();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Toggle Gizmos"))
            {
                patrollingPath.ToggleGizoms();
            }
        }

    }
}
