using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Sounds
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManagerSrc.Instance.EnemyHandCards));
    }
    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);

        int count = cards.Count == 1 ? 1 :
                    Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (GameManagerSrc.Instance.EnemyFieldCards.Count > 5 ||
                GameManagerSrc.Instance.CurrentGame.Enemy.Mana == 0 ||
                GameManagerSrc.Instance.EnemyHandCards.Count == 0)
                break;

            List<CardController> cardsList = cards.FindAll(x => GameManagerSrc.Instance.CurrentGame.Enemy.Mana >= x.Card.Manacost);
            

            if (cardsList.Count == 0)
                break;

            if (cardsList[0].Card.IsSpell)
            {
                CastSpell(cardsList[0]);
                yield return new WaitForSeconds(.51f);
            }
            else
            {
                cardsList[0].GetComponent<CardMovementSrc>().MoveToField(GameManagerSrc.Instance.EnemyField);

                yield return new WaitForSeconds(.51f);

                cardsList[0].transform.SetParent(GameManagerSrc.Instance.EnemyField);

                cardsList[0].OnCast();

                if(cardsList[0].Card.Name == "Murat")
                {
                    PlaySound(sound[0]);
                }
                if (cardsList[0].Card.Name == "Taksist")
                {
                    PlaySound(sound[1]);
                }
            }
            
        }

        yield return new WaitForSeconds(.51f);

        while (GameManagerSrc.Instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = GameManagerSrc.Instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvocation = GameManagerSrc.Instance.PlayerFieldCards.Exists(x => x.Card.IsProvocation);

            if (hasProvocation ||
                Random.Range(0, 2) == 0 &&
                GameManagerSrc.Instance.PlayerFieldCards.Count > 0)
            {
                CardController enemy;

                if (hasProvocation)
                    enemy = GameManagerSrc.Instance.PlayerFieldCards.Find(x => x.Card.IsProvocation);
                else
                    enemy = GameManagerSrc.Instance.PlayerFieldCards[Random.Range(0, GameManagerSrc.Instance.PlayerFieldCards.Count)];

                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Defense + "---> " +
                    enemy.Card.Name + " (" + enemy.Card.Attack + ";" + enemy.Card.Defense + ")");

                activeCard.Movement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                GameManagerSrc.Instance.CardsFight(enemy, activeCard);

            }
            else
            {
                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ") Attacked hero");

                activeCard.GetComponent<CardMovementSrc>().MoveToTarget(GameManagerSrc.Instance.PlayerHero.transform);
                yield return new WaitForSeconds(.75f);

                GameManagerSrc.Instance.DamageHero(activeCard, false);
            }

            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(1);
        GameManagerSrc.Instance.ChangeTurn();
    }

    void CastSpell(CardController card)
    {
        switch (((SpellCard)card.Card).SpellTarget)
        {
            case SpellCard.TargetType.NO_TARGET:
                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS:

                        if (GameManagerSrc.Instance.EnemyFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card));
                        }

                        break;
                    case SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                        if (GameManagerSrc.Instance.PlayerFieldCards.Count > 0)
                        {
                            StartCoroutine(CastCard(card));
                        }

                        break;
                    case SpellCard.SpellType.HEAL_ALLY_HERO:
                        StartCoroutine(CastCard(card));
                        break;
                    case SpellCard.SpellType.DAMAGE_ENEMY_HERO:
                        StartCoroutine(CastCard(card));
                        break;
                }

                break;
            case SpellCard.TargetType.ALLY_CARD_TARGET:

                if (GameManagerSrc.Instance.EnemyFieldCards.Count > 0)
                {
                    StartCoroutine(CastCard(card, GameManagerSrc.Instance.EnemyFieldCards[Random.Range(0, GameManagerSrc.Instance.EnemyFieldCards.Count)]));
                }

                break;
            case SpellCard.TargetType.ENEMY_CARD_TARGET:
                if (GameManagerSrc.Instance.PlayerFieldCards.Count > 0)
                {
                    StartCoroutine(CastCard(card, GameManagerSrc.Instance.PlayerFieldCards[Random.Range(0, GameManagerSrc.Instance.PlayerFieldCards.Count)]));
                }
                break;
        }
    }
    IEnumerator CastCard(CardController spell, CardController target = null)
    {
        if (((SpellCard)spell.Card).SpellTarget == SpellCard.TargetType.NO_TARGET)
        {
            spell.GetComponent<CardMovementSrc>().MoveToField(GameManagerSrc.Instance.EnemyField);
            yield return new WaitForSeconds(.51f);

            spell.OnCast();
        }
        else
        {
            spell.Info.ShowCardInfo();
            spell.GetComponent<CardMovementSrc>().MoveToField(target.transform);
            yield return new WaitForSeconds(.51f);

            GameManagerSrc.Instance.EnemyHandCards.Remove(spell);
            GameManagerSrc.Instance.EnemyHandCards.Add(spell);
            GameManagerSrc.Instance.ReduceMana(false, spell.Card.Manacost);

            spell.Card.IsPlaced = true;

            spell.UseSpell(target);
        }

        string targetStr = target == null ? "no_target" : target.Card.Name;
        Debug.Log("AI spell cast: " + (spell.Card).Name + " target " + targetStr);
    }
}
