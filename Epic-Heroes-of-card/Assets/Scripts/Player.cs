using UnityEngine;

public class Player 
{
    public int HP, Mana, Manapool;
    const int MAX_MANAPOOL = 10;

    public  Player()
    {
        HP = 30;
        Mana = Manapool = 2;
    }

    public void RestoreroundMana()
    {
        Mana = Manapool;
    }

    public void IncreaseManapool()
    {
        Manapool=Mathf.Clamp(Manapool + 1, 0, MAX_MANAPOOL);
    }

    public void GetDamage(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, int.MaxValue);
    }
}
