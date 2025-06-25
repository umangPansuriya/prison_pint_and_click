using System;
using UnityEngine;

public static class EnemyEventBroadcaster
{
    public static event Action<Vector3> Alert_Action;
    public static void RisePlayerDetected(Vector3 position)
    {
        Alert_Action?.Invoke(position);
    }
}
