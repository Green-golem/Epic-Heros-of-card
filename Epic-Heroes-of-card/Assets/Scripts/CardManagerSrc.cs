using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public enum AbilityType
    {
        NO_ABILITY,
        INSTANT_ACTIVE,
        DOUBLE_ATTACK,
        SHIELD,
        PROVOCATION,
        REGENERATION_EACH_TURN,
        COUNTER_ATTACK
    }

    public enum SpellType 
    {
        NO_SPELL,
        HEAL_ALLY_FIELD_CARDS,
        DAMAGE_ENEMY_FIELD_CARDS,
        HEAL_ALLY_HERO,
        DAMAGE_ENEMY_HERO,
        HEAL_ALLY_CARD,
        DAMAGE_ENEMY_CARD,
        SHIELD_ON_ALLY_CARD,
        PROVOCATION_ON_ALLY_CARD,     //не реализовывать у нас провокация и так у всех карт сразу есть
        BUFF_CARD_DAMAGE,
        DEBUFF_CARD_DAMAGE
    }

    public enum TargetType
    {
        NO_TARGET,
        ALLY_CARD_TARGET,
        ENEMY_CARD_TARGET
    }

    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost;
    public bool CanAttack;
    public bool IsPlaced;

    public List<AbilityType> Abilities;
    public SpellType Spell;
    public TargetType SpellTarget;
    public int SpellValue;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public bool IsSpell
    {
        get
        {
            return Spell != SpellType.NO_SPELL;
        }
    }
    public bool HasAbility
    {
        get
        {
            return Abilities.Count > 0;
        }
    }
    public bool IsProvocation
    {
        get
        {
            return Abilities.Exists(x => x == AbilityType.PROVOCATION);
        }
    }

    public int TimesDealedDamage;

    public Card(string name, string logoPath, int attack, int defense, int manacost,
                AbilityType abilityType = 0, SpellType spellType = 0, int spellVal = 0,
                TargetType targetType = 0)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Manacost = manacost;
        CanAttack = false;
        IsPlaced = false;

        Abilities = new List<AbilityType>();

        if (abilityType != 0)
            Abilities.Add(abilityType);

        Spell = spellType;
        SpellTarget = targetType;
        SpellValue = spellVal;


        //Abilities.Add(AbilityType.PROVOCATION);//провокация для всех лень делать норм слипил шляпу
        

        TimesDealedDamage = 0;
    }
    
    public void GetDamage(int dmg)
    {
        if (dmg > 0)
        {
            if (Abilities.Exists(x => x == AbilityType.SHIELD))
                Abilities.Remove(AbilityType.SHIELD);
            else
                Defense -= dmg;
        }
    }

    public Card GetCopy()
    {
        Card card = this;
        card.Abilities = new List<AbilityType>(Abilities);
        return card;
    }
}

public static class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerSrc : MonoBehaviour
{
    private void Awake()
    {
        CardManager.AllCards.Add(new Card("granata", "Sprites/granata", 5, 3, 4));
        CardManager.AllCards.Add(new Card("dragon", "Sprites/dragon", 1, 1, 1));
        CardManager.AllCards.Add(new Card("energy", "Sprites/energy", 2, 5, 3));
        CardManager.AllCards.Add(new Card("kamen", "Sprites/kamen", 3, 3, 2));
        CardManager.AllCards.Add(new Card("krokodil", "Sprites/krokodil", 4, 3, 3));
        CardManager.AllCards.Add(new Card("monster", "Sprites/monster", 2, 3, 2));

        CardManager.AllCards.Add(new Card("INSTANTACTIVE", "Sprites/dragon", 1, 1, 1,
            Card.AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(new Card("DOUBLEATTACK", "Sprites/energy", 2, 5, 3,
            Card.AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("COUNTERATTACK", "Sprites/kamen", 3, 3, 2,
            Card.AbilityType.COUNTER_ATTACK));
        CardManager.AllCards.Add(new Card("REGENERATION", "Sprites/monster", 2, 3, 2,
            Card.AbilityType.REGENERATION_EACH_TURN));
        CardManager.AllCards.Add(new Card("SHEALD", "Sprites/gnom", 2, 2, 2,
            Card.AbilityType.SHIELD));

        CardManager.AllCards.Add(new Card("HealAllFeld", "Sprites/granata", 0, 0, 4, 0,
            Card.SpellType.HEAL_ALLY_FIELD_CARDS, 2, Card.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new Card("DamageAllFeld", "Sprites/dragon", 0, 0, 4, 0,
            Card.SpellType.DAMAGE_ENEMY_FIELD_CARDS, 2, Card.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new Card("HealHero", "Sprites/energy", 0, 0, 3, 0,
            Card.SpellType.HEAL_ALLY_HERO, 2, Card.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new Card("DamageHero", "Sprites/kamen", 0, 0, 3, 0,
            Card.SpellType.DAMAGE_ENEMY_HERO, 2, Card.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new Card("HealCard", "Sprites/krokodil", 0, 0, 2, 0,
            Card.SpellType.HEAL_ALLY_CARD, 2, Card.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new Card("DamageCard", "Sprites/monster", 0, 0, 2, 0,
            Card.SpellType.DAMAGE_ENEMY_CARD, 2, Card.TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new Card("DamageCard", "Sprites/monster", 0, 0, 2, 0,
            Card.SpellType.SHIELD_ON_ALLY_CARD, 0, Card.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new Card("BuffDamage", "Sprites/granata", 0, 0, 4, 0,
            Card.SpellType.BUFF_CARD_DAMAGE, 2, Card.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new Card("DebuffDamage", "Sprites/dragon", 0, 0, 4, 0,
            Card.SpellType.DEBUFF_CARD_DAMAGE, 2, Card.TargetType.ENEMY_CARD_TARGET));
    }
}
