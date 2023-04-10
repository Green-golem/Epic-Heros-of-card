using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public CardController CC;

    public GameObject Shield,
                      Provacation,
                      DoubleAttack,
                      Regeniration,
                      CounterAttack;
                      

    public void OnCast()
    {
        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.INSTANT_ACTIVE:

                    CC.Card.CanAttack = true;

                    if (CC.IsPlayerCard)
                        CC.Info.HighlightCard(true);

                    break;

                case Card.AbilityType.SHIELD:
                    Shield.SetActive(true);
                    break;

                case Card.AbilityType.DOUBLE_ATTACK:
                    DoubleAttack.SetActive(true);
                    break;

                case Card.AbilityType.REGENERATION_EACH_TURN:
                    Regeniration.SetActive(true);
                    break;

                case Card.AbilityType.COUNTER_ATTACK:
                    CounterAttack.SetActive(true);
                    break;

                //case Card.AbilityType.PROVOCATION:
                //    Provacation.SetActive(true);/* выключил отображение провокации но обьект оставил невидимым*/
                //         break;
            }
        }
    }

    public void OnDamageDeal()
    {
        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.DOUBLE_ATTACK:

                    if (CC.Card.TimesDealedDamage == 1)
                    {
                        CC.Card.CanAttack = true;

                        if (CC.IsPlayerCard)
                            CC.Info.HighlightCard(true);
                    }

                    break;
            }
        }
    }

    public void OnDamageTake(CardController attacker = null)
    {
        Shield.SetActive(false);

        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.SHIELD:
                    Shield.SetActive(true);
                    break;

                case Card.AbilityType.COUNTER_ATTACK:

                    if (attacker != null)
                        attacker.Card.GetDamage(CC.Card.Attack);

                    break;
            }
        }
    }

    public void OnNewTurn()
    {
        CC.Card.TimesDealedDamage = 0;


        foreach (var ability in CC.Card.Abilities)
        {
            switch (ability)
            {
                case Card.AbilityType.REGENERATION_EACH_TURN:
                    CC.Card.Defense += 2;
                    CC.Info.RefreshData();
                    break;
            }
        }
    }

}
