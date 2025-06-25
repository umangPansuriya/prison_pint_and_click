using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] protected DialogueData _dialogue;
    [SerializeField] protected DialogueUIController _dialogueUiController;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _dialogueUiController.TriggerDialogue(_dialogue, OnDialogueFinish);
        }
    }
    protected virtual void OnDialogueFinish()
    {
        gameObject.SetActive(false);
    }
}
