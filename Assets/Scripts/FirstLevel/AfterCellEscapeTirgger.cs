using NPC.Guard;
using UnityEngine;
public class AfterCellEscapeTirgger : DialogueTrigger
{
    [SerializeField] private GunGuard _guard;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform _cameraRoom;
    [SerializeField] private DialogueData _guardInfoCellOpen;
    [SerializeField] private DialogueUIController _dialogueUIController;
    [SerializeField] private GameObject _slidingDoor;
    [SerializeField] private GameObject _staticDoor;
    protected override void OnDialogueFinish()
    {
        base.OnDialogueFinish();
        _cameraController.ChangeFocusTo(_cameraRoom, 2, OnCameraRoomFocusFinish);
    }
    private void OnCameraRoomFocusFinish()
    {
        _dialogueUiController.TriggerDialogue(_guardInfoCellOpen, GuardDialogfinish);
    }
    private void GuardDialogfinish()
    {
        _guard.gameObject.SetActive(true);
        _guard.StateManager.StateChanged += OnGuadrdStateChange;
        _cameraController.FocusBackToPlayer();
        //_cameraController.ChangeFocusFromCurrentPosition(_guard.transform, 2, FocusBackToPlayer);
    }
    private void FocusBackToPlayer()
    {
        _cameraController.FocusBackToPlayer();
    }
    private void OnGuadrdStateChange(State state)
    {
        _guard.StateManager.StateChanged -= OnGuadrdStateChange;
        _guard.SwitchStateTo(new GunGuardWalking(_guard, transform.position));
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        _slidingDoor.SetActive(false);
        _staticDoor.SetActive(true);
    }
}
