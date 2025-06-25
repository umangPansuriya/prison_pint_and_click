using UnityEngine;
public class PhaysicalBook : Interactale
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private DialogueUIController _dialogueUiController;
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _interactableDoor;
    public override void OnInteract(PlayerController player)
    {
        _player = player;
        if (_player.Move(_interactionPoint.position))
        {
            _player.ReachDestination_Action += PlayerReachDestination_Action;
        }
    }
    public override void Deselect()
    {
        base.Deselect();
        _player.ReachDestination_Action -= PlayerReachDestination_Action;
    }
    private void PlayerReachDestination_Action()
    {
        _dialogueUiController.TriggerDialogue(_dialogueData, OnDialogueFinish);
        gameObject.SetActive(false);
    }
    private void OnDialogueFinish()
    {
        _door.SetActive(false);
        _interactableDoor.SetActive(true);
    }
}
