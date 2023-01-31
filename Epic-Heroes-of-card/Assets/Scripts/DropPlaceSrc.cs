using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceSrc : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardMovementSrc card = eventData.pointerDrag.GetComponent<CardMovementSrc>();

        if (card)
            card.DefaultParent = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
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
