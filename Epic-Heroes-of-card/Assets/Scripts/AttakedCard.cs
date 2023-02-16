using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttakedCard : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        CardInfoSrc card = eventData.pointerDrag.GetComponent<CardInfoSrc>();

        if (card &&
            card.SelfCard.CanAttack &&
            transform.parent == GetComponent<CardMovementSrc>().GameManager.EnemyField)
        {
            card.SelfCard.ChangeAttackState(false);
            GetComponent<CardMovementSrc>().GameManager.CardsFight(card, GetComponent<CardInfoSrc>());
        }

    }

}
