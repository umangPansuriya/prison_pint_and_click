using Unity.AI.Navigation;
using UnityEngine;

public class DoorContorller : Interactale
{
    [SerializeField] private Door _door1;
    [SerializeField] private Door _door2;

    [Space]
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

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
    public override void Deselect()
    {
        base.Deselect();
        _player.ReachDestination_Action -= OnRechDestination;
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
        GetComponent<NavMeshLink>().enabled = true;
    }
    public void OpenPanel()
    {
        _panel.Open();
    }
    public override void OnInteract(PlayerController player)
    {
        _player = player;
        if (player.Move(_startTransform.position) || player.Move(_endTransform.position))
        {
            player.ReachDestination_Action += OnRechDestination;
        }
        else
        {
            Deselect();
        }

    }
    private void OnRechDestination()
    {
        if (IsAccessible(_player.transform))
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
        }
    }
}
