using UnityEngine;

public class DoorContorller : MonoBehaviour
{
    [SerializeField] private Door _door1;
    [SerializeField] private Door _door2;

    [Space]
    [SerializeField] private PuzzlePanel _panel;

    [Space]
    [SerializeField] protected bool _canShowAlways;
    [SerializeField] protected bool _isSolved;
    private void OnEnable()
    {
        _panel.PuzzleSolved_Action += OnPuzzleSolve;
    }
    private void OnDisable()
    {
        _panel.PuzzleSolved_Action -= OnPuzzleSolve;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 toPlayer = VectorHelper.Direction(transform, other.transform);
            int direction = 0 < Vector3.Dot(transform.forward, toPlayer) ? 1 : -1;
            if (_panel != null && !_isSolved || _canShowAlways && direction == -1)
            {
                _panel.Open();
                return;
            }
        }
        OpenDoor(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        _door1.Close();
        _door2.Close();
    }
    private void OpenDoor(Transform other)
    {
        Vector3 toPlayer = transform.position - other.position;
        int direction = 0 < Vector3.Dot(transform.forward, toPlayer) ? 1 : -1;
        _door1.Open(direction);
        _door2.Open(direction);
    }
    public bool IsAccessible(Transform other)
    {
        Vector3 toPlayer = transform.position - other.position;
        int direction = 0 < Vector3.Dot(transform.forward, toPlayer) ? 1 : -1;
        if (direction == 1) return true;
        return _isSolved && !_canShowAlways;
    }
    private void OnPuzzleSolve()
    {
        _door1.Open(-1);
        _door2.Open(-1);
        _isSolved = true;
    }
    public void OpenPanel()
    {
        _panel.Open();
    }
}
