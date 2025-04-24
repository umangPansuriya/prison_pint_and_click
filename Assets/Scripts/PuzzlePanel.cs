using System;
using UnityEngine;

public class PuzzlePanel : MonoBehaviour
{
    public event Action PuzzleSolved_Action;
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Solve()
    {
        PuzzleSolved_Action?.Invoke();
        Close();
    }
}
