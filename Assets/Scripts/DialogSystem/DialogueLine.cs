using UnityEngine;

[System.Serializable]
public class DialogueLine
{

    public string SpeakerName;
    public DialogueType dialogueType;
    //[TextArea(2, 5)]
    //public string sentence;
    [TextArea]
    public string Text; 
    public Sprite SpeakerSprite;
    public bool IsLeftSide = true;
} 