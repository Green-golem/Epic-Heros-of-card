using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost;
    public bool CanAttack;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }


    public Card(string name, string logoPath, int attack, int defense, int manacost)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Manacost = manacost;
        CanAttack = false;
    }

    public void ChangeAttackState(bool can)
    {
        CanAttack = can;
    }
    
    public void GetDamage(int dmg)
    {
        Defense -= dmg;
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
    }
}
