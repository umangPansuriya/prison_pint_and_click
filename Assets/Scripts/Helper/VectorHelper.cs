using UnityEngine;

public class VectorHelper : MonoBehaviour
{
    public static Vector3 Direction(Vector3 a, Vector3 b)
    {
        return a - b;
    }
    public static Vector3 Direction(Transform a, Transform b)
    {
        return a.position - b.position;
    }
}
