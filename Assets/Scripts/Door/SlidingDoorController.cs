using UnityEngine;

public class SlidingDoorController : PuzzleDoorController
{
    [SerializeField] private Tween _door1;
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
    protected override void OpenDoor(Transform other)
    {
        Vector3 toPlayer = transform.position - other.position;
        int direction = 0 < Vector3.Dot(transform.forward, toPlayer) ? 1 : -1;
        _door1.Open();
    }
    protected override void CloseDoor()
    {
        _door1.Close();
    }

}
