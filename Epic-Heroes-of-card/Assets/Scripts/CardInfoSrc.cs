using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoSrc : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI Name, Attack, Defense, Manacost;
    public GameObject HideObj, HighlitedObj;
    public bool IsPlayer;
    public Color NormalCol, TargetCol;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        HideObj.SetActive(true);
        IsPlayer = false;
        Manacost.text = "";
    }

    public void ShowCardInfo(Card card, bool isPlayer)
    {
        IsPlayer = isPlayer;
        HideObj.SetActive(false);
        SelfCard = card;

        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;

        RefreshData();
    }

    public void RefreshData()
    {
        Attack.text = SelfCard.Attack.ToString();
        Defense.text = SelfCard.Defense.ToString();
        Manacost.text = SelfCard.Manacost.ToString();
    }

    public void HighlightCard()
    {
        HighlitedObj.SetActive(true);
    }

    public void DeHighlightCard()
    {
        HighlitedObj.SetActive(false);
    }

    public void CheckForAvailability(int currentMana)
    {
        GetComponent<CanvasGroup>().alpha = currentMana >= SelfCard.Manacost ?
                                            1 :
                                            .5f;
    }

    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ?
                                      TargetCol :
                                      NormalCol;
    }

}
