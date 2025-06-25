using UnityEngine;
using UnityEngine.UI;

public class SortPuzzleController : PuzzlePanel
{
    public Transform codeDisplayParent;
    public Button enterBtnObj;

    private void OnEnable()
    {
        enterBtnObj.onClick.AddListener(OpenDoor);
    }
    private void OnDisable()
    {
        enterBtnObj.onClick.RemoveListener(OpenDoor);
    }
    public void CheckSequence()
    {
        for (int i = 0; i < codeDisplayParent.childCount; i++)
        {
            int index = codeDisplayParent.GetChild(i).GetComponent<DraggableItem>().idIndex;
            if (i != index)
                return;
        }
        enterBtnObj.interactable = true;
    }
    public void OpenDoor()
    {
        Solve();
    }
}
