using System;

public static class GameEvent
{
    public static event Action GameOver;
    public static void RiseGameOver()
    {
        GameOver?.Invoke();
    }
}
