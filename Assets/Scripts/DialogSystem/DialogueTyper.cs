using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTyper : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public Button nextButton;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;
    public List<DialogueLine> lines = new List<DialogueLine>();

    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        nextButton.onClick.AddListener(HandleNext);
        PlayDialogueFromStart();
    }

    public void PlayDialogueFromStart()
    {
        currentLineIndex = 0;
        PlayCurrentLine();
    }

    void HandleNext()
    {
        if (isTyping)
        {
            // Skip to full line if still typing
            StopCoroutine(typingCoroutine);
            dialogueText.text = lines[currentLineIndex].Text;
            isTyping = false;
        }
        else
        {
            // Go to next line
            currentLineIndex++;
            if (currentLineIndex < lines.Count)
                PlayCurrentLine();
            else
                EndDialogue();
        }
    }

    void PlayCurrentLine()
    {
        Debug.Log("currentindex " + currentLineIndex);
        DialogueLine line = lines[currentLineIndex];
        speakerNameText.text = line.Text;
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(TypeText(line.Text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueText.text = "";
        speakerNameText.text = "";
        // Optionally: disable UI or trigger next event
    }
    public void SetLineData(DialogueData dialogueData)
    {
        lines = dialogueData.Lines;
    }
}
