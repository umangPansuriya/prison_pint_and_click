
using UnityEngine;
public class Grabeable : Interactale
{
    [SerializeField] private Items _name;
    [SerializeField] private Transform _pickupPoint;
    public override void OnInteract(PlayerController player)
    {
        _player = player;
        if (_player.Move(_pickupPoint.position))
        {
            _player.ReachDestination_Action += OnPlayerReachDestination;
        }
    }
    public override void Deselect()
    {
        base.Deselect();
        _player.ReachDestination_Action -= OnPlayerReachDestination;
    }
    private void OnPlayerReachDestination()
    {
        _player.Inventory.AddItem(_name);
        gameObject.SetActive(false);
    }
}
