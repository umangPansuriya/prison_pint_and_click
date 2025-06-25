using UnityEngine;
public class HideObject : Interactale, IHidable
{
    [SerializeField] private PlayerHud _playerHud;
    [SerializeField] private Transform _interactionPoint;
    public override void OnInteract(PlayerController player)
    {
        _player = player;
        _player.ReachDestination_Action += OnReachDestination;
        _player.Move(_interactionPoint.position);
    }
    public void Hide()
    {
        _player.Hide();
    }
    public void OnReachDestination()
    {
        _playerHud.EnableHideBtn();
    }
    public override void Deselect()
    {
        base.Deselect();
        _playerHud.DisableHideBtn();
        _player.ReachDestination_Action -= OnReachDestination;
    }
}
