using UnityEngine;

public abstract class Interactale : MonoBehaviour, IInteractble
{
    protected PlayerController _player;
    public abstract void OnInteract(PlayerController playerMovement);
    public virtual void OnSelect()
    {
        GetComponent<Outline>().OutlineColor = Color.yellow;
    }
    public virtual void Deselect()
    {
        GetComponent<Outline>().OutlineColor = Color.white;
    }
}
