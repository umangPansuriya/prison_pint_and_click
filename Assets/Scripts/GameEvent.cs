using System;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static event Action GameOver;
    public static void RiseGameOver()
    {
        GameOver?.Invoke();
    }
}
