using UnityEngine;
using UnityEngine.UI;

public class KeyCardScanner : PuzzlePanel
{
    public KeyCard playerKeyCard;
    public KeyCardController key_card_controller;
    public Image SignalLable_Img;
    public Text SignalLable_Txt;
    public void CheckCardIdetity()
    {
        if (playerKeyCard.keyCardNm == KeycardName.cameraRoomCardKey)
        {
            SignalLable_Img.color = Color.green;
            SignalLable_Txt.text = "DOOR OPEN!";
            Solve();
        }
        else
        {
            Close();
        }
    }
    public void SetPlayerKeyCard()
    {
        playerKeyCard.setSelectCard();
    }
    public void TurnOffScanner()
    {
        gameObject.SetActive(false);
    }
}
