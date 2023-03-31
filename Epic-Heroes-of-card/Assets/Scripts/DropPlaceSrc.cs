using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}

public class DropPlaceSrc : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;

    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
            return;

        CardMovementSrc card = eventData.pointerDrag.GetComponent<CardMovementSrc>();

        if (card && card.GameManager.PlayerFieldCards.Count < 6 &&
            card.GameManager.IsPlayerTurn && card.GameManager.PlayerMana >=
            card.GetComponent<CardInfoSrc>().SelfCard.Manacost)

        {
            card.GameManager.PlayerHandCards.Remove(card.GetComponent<CardInfoSrc>());
            card.GameManager.PlayerFieldCards.Add(card.GetComponent<CardInfoSrc>());
            card.DefaultParent = transform;

            card.GameManager.ReduceMana(true, card.GetComponent<CardInfoSrc>().SelfCard.Manacost);
        }
            
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD || Type == FieldType.ENEMY_HAND ||
            Type == FieldType.SELF_HAND )
            return;

        CardMovementSrc card = eventData.pointerDrag.GetComponent<CardMovementSrc>();

        if (card)
            card.DefaultTempCardParent = transform;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardMovementSrc card = eventData.pointerDrag.GetComponent<CardMovementSrc>();

        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;

    }
}
