using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static event Action Tick;
    void Update()
    {
        Tick?.Invoke();
    }
}
