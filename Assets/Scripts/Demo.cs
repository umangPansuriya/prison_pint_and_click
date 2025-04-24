using UnityEngine;
using UnityEngine.EventSystems;

public class Demo : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }


}


