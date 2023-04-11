using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game
{
    public Player Player, Enemy;
    public List<Card> EnemyDeck, PlayerDeck;
                     

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();

        Player = new Player();
        Enemy = new Player();
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 60; i++) // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        {
            var card = CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)];

            if(card.IsSpell)
                list.Add(((SpellCard)card).GetCopy());
            else
                list.Add(card.GetCopy());
        }
            return list;
    }
    
}



public class GameManagerSrc : MonoBehaviour
{
    public static GameManagerSrc Instance;

    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand,
                     EnemyField, PlayerField;

    public GameObject CardPref;
    int Turn, TurnTime = 30;


    public AttakedHero EnemyHero, PlayerHero;
    public AI EnemyAI;

    public List<CardController> PlayerHandCards = new List<CardController>(),
                             PlayerFieldCards = new List<CardController>(),
                             EnemyHandCards = new List<CardController>(),
                             EnemyFieldCards = new List<CardController>();


    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        StartGame();
    }

    public void RestartGame()
    {
        StopAllCoroutines();

        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);

        PlayerHandCards.Clear();
        PlayerFieldCards.Clear();
        EnemyHandCards.Clear();
        EnemyFieldCards.Clear();

        StartGame();
    }

    public void ExitGame(int scena)
    {
       
        SceneManager.LoadScene(scena);
        
    }

    public void StartGame()
    {
        Turn = 0;
       

        CurrentGame = new Game();

        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);


        UIController.Instance.StartGame();

        StartCoroutine(TurnFunc());
    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 4)
            GiveCardToHand(deck, hand);
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;

        CreateCardPref(deck[0], hand);

       

        

        deck.RemoveAt(0);
    }

    void CreateCardPref(Card card, Transform hand)
    {
        GameObject cardGO = Instantiate(CardPref, hand, false);
        CardController cardC = cardGO.GetComponent<CardController>();

        cardC.Init(card, hand == PlayerHand);

        if (cardC.IsPlayerCard)
            PlayerHandCards.Add(cardC);
        else
            EnemyHandCards.Add(cardC);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        UIController.Instance.UpdateTurnTime(TurnTime);

        foreach (var card in PlayerFieldCards)
            card.Info.HighlightCard(false);

        CheckCardsForManaAvaliability();

        if (IsPlayerTurn)
        {
            if (CurrentGame.PlayerDeck.Count == 0 || CurrentGame.EnemyDeck.Count == 0)
            {
                UIController.Instance.ShowResult();
                StopAllCoroutines();
            }
            else
            {
                foreach (var card in PlayerFieldCards)
                {
                    card.Card.CanAttack = true;
                    card.Info.HighlightCard(true);
                    card.Ability.OnNewTurn();
                }
                

                while(TurnTime-- > 0)
                {
                    UIController.Instance.UpdateTurnTime(TurnTime);
                    yield return new WaitForSeconds(1);
                }

                ChangeTurn();
            }


            
        }
        else
        {
            if (CurrentGame.PlayerDeck.Count == 0 || CurrentGame.EnemyDeck.Count == 0)
            {
                UIController.Instance.ShowResult();
                StopAllCoroutines();
            }
            else
            {
                foreach (var card in EnemyFieldCards)
                {
                    card.Card.CanAttack = true;
                    card.Ability.OnNewTurn();
                }

                EnemyAI.MakeTurn();

                while (TurnTime-- > 0)
                {
                    UIController.Instance.UpdateTurnTime(TurnTime);
                    yield return new WaitForSeconds(1);
                }

                ChangeTurn();
            }
        }
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;

        UIController.Instance.DisableTurnBtn();

        if (IsPlayerTurn)
        {
            GiveNewCards();

            CurrentGame.Player.IncreaseManapool();
            CurrentGame.Player.RestoreroundMana();

            UIController.Instance.UpdateHPAndMana();
        }
        else
        {
            CurrentGame.Enemy.IncreaseManapool();
            CurrentGame.Enemy.RestoreroundMana();
        }

        StartCoroutine(TurnFunc());
    }

    void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }

    public void CardsFight(CardController attacker, CardController defender)
    {
        defender.Card.GetDamage(defender.Card.Attack);
        attacker.OnDamageDeal();
        defender.OnTakeDamage(attacker);

        attacker.Card.GetDamage(defender.Card.Attack);
        attacker.OnTakeDamage();

        attacker.CheckForAlive();
        defender.CheckForAlive();
    }


    public void ReduceMana(bool playerMana, int manacost)
    {
        if (playerMana)
            CurrentGame.Player.Mana -= manacost;
        
        else
            CurrentGame.Enemy.Mana -= manacost;

        UIController.Instance.UpdateHPAndMana();
    }

    public void DamageHero(CardController card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
            CurrentGame.Enemy.GetDamage(card.Card.Attack);
        else
            CurrentGame.Player.GetDamage(card.Card.Attack);

        UIController.Instance.UpdateHPAndMana();
        card.OnDamageDeal();
        CheckForResult();
    }

    public void CheckForResult()
    {
        if (CurrentGame.Enemy.HP == 0 || CurrentGame.Player.HP == 0)
        {
            UIController.Instance.ShowResult();
            StopAllCoroutines();
        }
    }

    public void CheckCardsForManaAvaliability()
    {
        foreach (var card in PlayerHandCards)
            card.Info.HighlightManaAvaliability(CurrentGame.Player.Mana);
    }

    public void HighlightTargets(CardController attacker, bool highlight)
    {
        List<CardController> targets = new List<CardController>();

        if (attacker.Card.IsSpell)
        {
            var spellCard = (SpellCard)attacker.Card;

            if (spellCard.SpellTarget == SpellCard.TargetType.NO_TARGET)
                targets = new List<CardController>();
            else if (spellCard.SpellTarget == SpellCard.TargetType.ALLY_CARD_TARGET)
                targets = PlayerFieldCards;
            else
                targets = EnemyFieldCards;
        }
        else
        {
            if (EnemyFieldCards.Exists(x => x.Card.IsProvocation))
                targets = EnemyFieldCards.FindAll(x => x.Card.IsProvocation);
            else
            {
                targets = EnemyFieldCards;
                
                if (!attacker.Card.IsSpell)
                    EnemyHero.HighlightAsTarget(highlight);
            }
        }

        foreach (var card in targets)
        {
            if (attacker.Card.IsSpell)
                card.Info.HighlightAsSpellTarget(highlight);
            else
                card.Info.HighlightAsTarget(highlight);

        }
    }
}
