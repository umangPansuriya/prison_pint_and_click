using System;

public class PuzzlePanel : BasePanel
{
    public event Action PuzzleSolved_Action;
    public void Solve()
    {
        PuzzleSolved_Action?.Invoke();
        Close();
    }
}
