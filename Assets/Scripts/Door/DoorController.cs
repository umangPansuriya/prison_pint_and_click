using UnityEngine;

public abstract class DoorContorller : Interactale
{
    [Space]
    [SerializeField] protected Transform _startTransform;
    [SerializeField] protected Transform _endTransform;

    public override void Deselect()
    {
        base.Deselect();
        _player.ReachDestination_Action -= OnRechDestination;
    }
    protected abstract void OpenDoor(Transform other);
    protected abstract void CloseDoor();
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
    protected virtual void OnRechDestination()
    {

    }
}
