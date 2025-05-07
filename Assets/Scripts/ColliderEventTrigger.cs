using System;
using UnityEngine;

public class ColliderEventTrigger : MonoBehaviour
{
    public event Action<Transform> TriggerEnter_Action;
    public event Action<Transform> TriggerExit_Action;

    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {

    }
}
