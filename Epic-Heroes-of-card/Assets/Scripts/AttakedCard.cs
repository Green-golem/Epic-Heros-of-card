using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttakedCard : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (!GetComponent<CardMovementSrc>().GameManager.IsPlayerTurn)
            return;

        CardInfoSrc card = eventData.pointerDrag.GetComponent<CardInfoSrc>();

        if (card &&
            card.SelfCard.CanAttack &&
            transform.parent == GetComponent<CardMovementSrc>().GameManager.EnemyField)
        {
            card.SelfCard.ChangeAttackState(false);

            if (card.IsPlayer)
                card.DeHighlightCard();

            GetComponent<CardMovementSrc>().GameManager.CardsFight(card, GetComponent<CardInfoSrc>());
        }

    }

}
 