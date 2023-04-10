using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card
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

    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost;
    public bool CanAttack;
    public bool IsPlaced;

    public List<AbilityType> Abilities;

    public bool IsSpell;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
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

    public Card(string name, string logoPath, int attack, int defense, int manacost,AbilityType abilityType=0)
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


        if(!IsSpell)
            Abilities.Add(AbilityType.PROVOCATION);//провокация для всех лень делать норм слипил шляпу
        

        TimesDealedDamage = 0;
    }
    

    public Card(Card card)
    {
        Name = card.Name;
        Logo = card.Logo;
        Attack = card.Attack;
        Defense = card.Defense;
        Manacost= card.Manacost;
        CanAttack = false;
        IsPlaced = false;

        Abilities = new List<AbilityType>(card.Abilities);

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
        return new Card(this);
    }
}
public class SpellCard : Card
{
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
        //PROVOCATION_ON_ALLY_CARD,     //не реализовывать у нас провокация и так у всех карт сразу есть
        BUFF_CARD_DAMAGE,
        DEBUFF_CARD_DAMAGE
    }
    public enum TargetType
    {
        NO_TARGET,
        ALLY_CARD_TARGET,
        ENEMY_CARD_TARGET
    }
    public SpellType Spell;
    public TargetType SpellTarget;
    public int SpellValue;
    public SpellCard(string name, string logoPath, int manacost, SpellType spellType = 0, int spellValue = 0, TargetType targetType= 0) : base(name, logoPath, 0, 0, manacost)
    {
        IsSpell = true;

        Spell = spellType;
        SpellTarget = targetType;
        SpellValue = spellValue;
    }
    public SpellCard(SpellCard card) : base(card)
    {
        IsSpell = true;

        Spell = card.Spell;
        SpellTarget = card.SpellTarget;
        SpellValue = card.SpellValue;
    }

    public new SpellCard GetCopy()
    {
        return new SpellCard(this);
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
        CardManager.AllCards.Add(new Card("Dragon", "Sprites/Dragon", 5, 2, 3, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("AngryHamster", "Sprites/AngryHamster", 1, 2, 1, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("Priest", "Sprites/Priest", 2, 5, 3, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("Druid", "Sprites/Druid", 2, 1, 1, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("Damn warrior", "Sprites/DamnWarrior", 3, 2, 2, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("Warrior", "Sprites/Warrior", 2, 3, 2, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("Murat", "Sprites/murat", 10, 10, 10, Card.AbilityType.NO_ABILITY));
        CardManager.AllCards.Add(new Card("Taksist", "Sprites/taksist", 10, 10, 10, Card.AbilityType.NO_ABILITY));


        CardManager.AllCards.Add(new Card("King", "Sprites/King", 3, 1, 2,
            Card.AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(new Card("Archer", "Sprites/Archer", 2, 3, 3,
            Card.AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("Scout", "Sprites/Scout", 3, 3, 2,
            Card.AbilityType.COUNTER_ATTACK));
        CardManager.AllCards.Add(new Card("DamnPirate", "Sprites/DamnPirate", 2, 3, 4,
            Card.AbilityType.REGENERATION_EACH_TURN));
        CardManager.AllCards.Add(new Card("Knight", "Sprites/Knight", 2, 2, 2,
            Card.AbilityType.SHIELD));
        CardManager.AllCards.Add(new Card("GhostShip", "Sprites/GhostShip", 2, 5, 4,
            Card.AbilityType.SHIELD));
        CardManager.AllCards.Add(new Card("StarMagicial", "Sprites/StarMagicial", 2, 2, 3,
            Card.AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("Gaslighter", "Sprites/Gaslighter", 1, 2, 3,
            Card.AbilityType.REGENERATION_EACH_TURN));
        CardManager.AllCards.Add(new Card("DesertKnight", "Sprites/DesertKnight", 1, 2, 3,
            Card.AbilityType.COUNTER_ATTACK));



        CardManager.AllCards.Add(new SpellCard("DamageAll", "Sprites/Damage", 4, 
            SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS, 2, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("HealHero", "Sprites/Heal", 3,
            SpellCard.SpellType.HEAL_ALLY_HERO, 2, SpellCard.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("Shield", "Sprites/Shield", 2, 
            SpellCard.SpellType.SHIELD_ON_ALLY_CARD, 0, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("DamageBuff", "Sprites/DamageBuff", 4, 
            SpellCard.SpellType.BUFF_CARD_DAMAGE, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("DamageDebuff", "Sprites/DamageDebuff", 4, 
            SpellCard.SpellType.DEBUFF_CARD_DAMAGE, 2, SpellCard.TargetType.ENEMY_CARD_TARGET));



        //CardManager.AllCards.Add(new SpellCard("DamageHero", "Sprites/kamen", 3, 
        //    SpellCard.SpellType.DAMAGE_ENEMY_HERO, 2, SpellCard.TargetType.NO_TARGET));
        //CardManager.AllCards.Add(new SpellCard("HealCard", "Sprites/krokodil", 2, 
        //    SpellCard.SpellType.HEAL_ALLY_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
        //CardManager.AllCards.Add(new SpellCard("DamageCard", "Sprites/monster", 2, 
        //    SpellCard.SpellType.DAMAGE_ENEMY_CARD, 2, SpellCard.TargetType.ENEMY_CARD_TARGET));
        //CardManager.AllCards.Add(new SpellCard("HealAllFeld", "Sprites/granata", 4, 
        //    SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS, 2, SpellCard.TargetType.NO_TARGET));
    }
}
