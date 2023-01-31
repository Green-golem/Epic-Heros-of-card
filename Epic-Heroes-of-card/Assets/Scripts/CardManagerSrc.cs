using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense;

    public Card(string name, string logoPath, int attack, int defense)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;

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
        CardManager.AllCards.Add(new Card("granata", "Resouces/Sprites/Card/granata", 5, 3));
        CardManager.AllCards.Add(new Card("svadba", "Resouces/Sprites/Card/svadba", 1, 1));
        CardManager.AllCards.Add(new Card("paladin", "Resouces/Sprites/Card/paladin", 2, 5));
        CardManager.AllCards.Add(new Card("gnomi", "Resouces/Sprites/Card/gnomi", 3, 3));
        CardManager.AllCards.Add(new Card("jrica_voin", "Resouces/Sprites/Card/jrica_voin", 4, 3));
        CardManager.AllCards.Add(new Card("jrica_mag", "Resouces/Sprites/Card/jrica_mag", 2, 3));
    }
}
