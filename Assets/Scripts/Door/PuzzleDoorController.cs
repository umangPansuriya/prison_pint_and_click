using UnityEngine;

public abstract class PuzzleDoorController : DoorContorller
{
    [Space]
    [SerializeField] protected PuzzlePanel _puzzlePanel;

    [Space]
    //[SerializeField] protected bool _canShowAlways;
    [SerializeField] protected bool _isSolved;
    private void OnEnable()
    {
        if (_puzzlePanel != null)
            _puzzlePanel.PuzzleSolved_Action += OnPuzzleSolve;
    }
    private void OnDisable()
    {
        if (_puzzlePanel != null)
            _puzzlePanel.PuzzleSolved_Action -= OnPuzzleSolve;
    }
    protected void OnPuzzleSolve()
    {
        _isSolved = true;
        TraverseThroughDoor();
        Deselect();
    }
    public void OpenPanel()
    {
        _puzzlePanel.Open();
    }
    public bool IsAccessible(Transform other)
    {
        Vector3 toPlayer = transform.position - other.position;
        int direction = 0 < Vector3.Dot(transform.forward, toPlayer) ? 1 : -1;
        if (direction == 1) return true;
        //return _isSolved && !_canShowAlways;
        return _isSolved;
    }

    protected abstract override void OpenDoor(Transform other);
    protected override void OnRechDestination()
    {
        if (IsAccessible(_player.transform))
        {
            TraverseThroughDoor();
        }
        else
        {
            _puzzlePanel.Open();
        }
    }
    protected void TraverseThroughDoor()
    {
        float a = Vector3.Distance(_player.transform.position, _startTransform.position);
        float b = Vector3.Distance(_player.transform.position, _endTransform.position);
        if (a < b)
        {
            _player.PassOnDoor(_startTransform.position, _endTransform.position);
        }
        else
        {
            _player.PassOnDoor(_endTransform.position, _startTransform.position);
        }
        OpenDoor(_player.transform);
    }
}
