using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoSrc : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI Name;

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;

        Logo.sprite = card.Logo;
       
        Name.text = card.Name;
    }

    private void Start()
    {
        ShowCardInfo(CardManager.AllCards[transform.GetSiblingIndex()]);
    }

}
