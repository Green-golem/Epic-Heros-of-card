using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerSrc.Instance.IsPlayerTurn)
            return;

        CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                       target = GetComponent<CardController>();

        if (spell &&
            spell.Card.IsSpell &&
            spell.IsPlayerCard &&
            target.Card.IsPlaced &&
            GameManagerSrc.Instance.CurrentGame.Player.Mana >= spell.Card.Manacost)
        {
            var spellCard = (SpellCard)spell.Card;

            if ((spellCard.SpellTarget == SpellCard.TargetType.ALLY_CARD_TARGET &&
                 target.IsPlayerCard) ||
                (spellCard.SpellTarget == SpellCard.TargetType.ENEMY_CARD_TARGET &&
                 !target.IsPlayerCard))
            {
                GameManagerSrc.Instance.ReduceMana(true, spell.Card.Manacost);
                spell.UseSpell(target);
                GameManagerSrc.Instance.CheckCardsForManaAvaliability();
            }
        }
    }

}
