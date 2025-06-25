using UnityEngine;
using UnityEngine.UI;

public class KeyCardController : MonoBehaviour
{
    public KeycardName selectedCard;
    KeycardName currntCard;
    GameObject currentCardObj;
    public Image displayCardImg;
    private Sprite selectedCardImgSprt;
    Color seletedCrdCol;
    GameObject oldStoredCard;
    public KeyCard playerKeyCard;
    public void selectCardStore()
    {
        selectedCard = currntCard;
        DisplaySelectedCard();
        SetPlayerKeyCard();
    }
    public void CurrentSelectCard(KeycardName cardNm, GameObject cardObj, Sprite selectedCardSPrt, Color col)
    {
        currntCard = cardNm;
        currentCardObj = cardObj;
        selectedCardImgSprt = selectedCardSPrt;
        seletedCrdCol = col;
    }
    public void ActivatedCardOnBack()
    {
        currentCardObj.SetActive(true);
    }
    public void DisplaySelectedCard()
    {
        if (oldStoredCard != null)
        {
            oldStoredCard.SetActive(true);
        }
        displayCardImg.color = seletedCrdCol;// selectedCardImgSprt;
        oldStoredCard = currentCardObj;
    }
    public void SetPlayerKeyCard()
    {
        playerKeyCard.keyCardNm = selectedCard;
    }
    public KeycardName GetSelectedCard()
    {
        return selectedCard;
    }
}
