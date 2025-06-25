using UnityEngine;

public class PanelOpener : Interactale
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private BasePanel _panel;
    public override void OnInteract(PlayerController player)
    {
        _player = player;
        _player.ReachDestination_Action += OnPlayerReachDestination;
        _player.Move(_interactionPoint.position);
    }

    public override void Deselect()
    {
        base.Deselect();
        _player.ReachDestination_Action -= OnPlayerReachDestination;
    }
    private void OnPlayerReachDestination()
    {
        _panel.Open();
    }
}
