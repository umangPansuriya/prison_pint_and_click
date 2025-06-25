using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image leftCharacterImage;     // Player
    public Image rightCharacterImage;    // Old Prisoner / NPC
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("Speaker Sprites")]
    public Sprite playerSprite;
    public Sprite oldPrisonerSprite;

    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private DialogueData _dialogue;

    private Queue<DialogueLine> _dialogueLines = new Queue<DialogueLine>();

    private Action _dialogueFinish_Action;

    private void OnEnable()
    {
        nextButton.onClick.AddListener(OnNextClicked);
    }
    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(OnNextClicked);
    }

    public void TriggerDialogue(DialogueData dialogue, Action onDialogueFinish)
    {
        _dialogueFinish_Action = onDialogueFinish;
        _dialogue = dialogue;
        StartDialogue(_dialogue);
    }
    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }
    public void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
        gameObject.SetActive(false);
    }
    public void DisplayLine(DialogueLine line)
    {
        _speakerNameText.text = line.SpeakerName;
        dialogueText.text = line.Text;

        if (line.SpeakerSprite != null)
        {
            if (line.IsLeftSide)
            {
                leftCharacterImage.sprite = line.SpeakerSprite;
                leftCharacterImage.gameObject.SetActive(true);
                rightCharacterImage.gameObject.SetActive(false);
            }
            else
            {
                rightCharacterImage.sprite = line.SpeakerSprite;
                rightCharacterImage.gameObject.SetActive(true);
                leftCharacterImage.gameObject.SetActive(false);
            }
        }
        else
        {
            leftCharacterImage.gameObject.SetActive(false);
            rightCharacterImage.gameObject.SetActive(false);
        }
    }
    public void UpdateDialogue(string text, DialogueType type)
    {
        //dialogueText.text = text;

        switch (type)
        {
            case DialogueType.Player:
                leftCharacterImage.sprite = playerSprite;
                leftCharacterImage.gameObject.SetActive(true);
                rightCharacterImage.gameObject.SetActive(false);
                break;

            case DialogueType.OldPrisoner:
                rightCharacterImage.sprite = oldPrisonerSprite;
                rightCharacterImage.gameObject.SetActive(true);
                leftCharacterImage.gameObject.SetActive(false);
                break;

            case DialogueType.NPC:// in game instruction
            default:
                leftCharacterImage.gameObject.SetActive(false);
                rightCharacterImage.gameObject.SetActive(false);
                break;
        }
    }
    public void StartDialogue(DialogueData data)
    {
        _dialogueLines.Clear();
        foreach (DialogueLine line in data.Lines)
        {
            _dialogueLines.Enqueue(line);
        }
        ShowDialoguePanel();
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        if (_dialogueLines.Count == 0)
        {
            HideDialoguePanel();
            _dialogueFinish_Action?.Invoke();
            return;
        }
        DialogueLine line = _dialogueLines.Dequeue();
        DisplayLine(line);
    }
    private void OnNextClicked()
    {
        ShowNextLine();
    }
}
/*
 
   public Image speakerImage;

    public float typingSpeed = 0.05f;
    private DialogueData currentDialogue;
    private int currentIndex;
    private Coroutine typingCoroutine;
    public DialogueTyper dialogue_Typer;
    public void StartDialogue(DialogueData dialogueData)
    {
        currentDialogue = dialogueData;
        dialogue_Typer.SetLineData(dialogueData);
        currentIndex = 0;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    public void ShowLine()
    {
        if (currentIndex >= currentDialogue.Lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.Lines[currentIndex];
        speakerNameText.text = line.SpeakerName;
        speakerImage.sprite = line.SpeakerSprite;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.Text));
    }

    IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void OnNextButtonPressed()
    {
        currentIndex++;
        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
    }
 
 
 
 
 */