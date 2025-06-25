using UnityEngine;

public class TwoDoorController : PuzzleDoorController
{
    [SerializeField] private Tween _door1;
    [SerializeField] private Tween _door2;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            OpenDoor(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CloseDoor();
    }
    protected override void CloseDoor()
    {
        _door1.Close();
        _door2.Close();
    }
    protected override void OpenDoor(Transform other)
    {
        Vector3 toPlayer = transform.position - other.position;
        int direction = 0 < Vector3.Dot(transform.forward, toPlayer) ? 1 : -1;
        _door1.Open(direction);
        _door2.Open(direction);
    }
}
