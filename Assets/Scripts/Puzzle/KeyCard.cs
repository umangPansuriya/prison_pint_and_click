using UnityEngine;
using UnityEngine.UI;

public class KeyCard : MonoBehaviour
{
    public KeyCardController key_card_controller;
    public KeycardName keyCardNm;

    public void setSelectCard()
    {
        Debug.Log("gameobject " + gameObject.name);
        key_card_controller.CurrentSelectCard(keyCardNm, this.gameObject,
            GetComponent<Image>().sprite, GetComponent<Image>().color);
    }

}