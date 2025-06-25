using UnityEngine;

public class KeyDoorController : TwoDoorController
{
    [SerializeField] private Items _requriedItem;
    private bool _isUnlocked;

    protected override void OpenDoor(Transform other)
    {
        base.OpenDoor(other);
    }
    protected override void OnRechDestination()
    {
        if (PlayerHasKey())
        {
            TraverseThroughDoor();
        }
        else
        {
            Debug.Log("Player Don't have key");
        }
    }
    private bool PlayerHasKey()
    {
        return _player.Inventory.HasItem(_requriedItem);
    }
}
