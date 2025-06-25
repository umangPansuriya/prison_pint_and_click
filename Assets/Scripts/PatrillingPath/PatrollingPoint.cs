using UnityEditor;
using UnityEngine;
namespace Prison.PatrollingGurd
{
    public class PatrollingPoint : MonoBehaviour
    {
        public float StayDuration;
        public AnimationClip Animation;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
#if UNITY_EDITOR
            Handles.Label(transform.position + Vector3.up * 1, transform.name);
#endif
        }
    }
}