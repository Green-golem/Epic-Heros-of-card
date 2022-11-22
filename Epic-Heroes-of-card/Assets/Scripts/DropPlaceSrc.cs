using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceSrc : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardSrc card = eventData.pointerDrag.GetComponent<CardSrc>();

        if (card)
            card.DefaultParent = transform;
    }
}
