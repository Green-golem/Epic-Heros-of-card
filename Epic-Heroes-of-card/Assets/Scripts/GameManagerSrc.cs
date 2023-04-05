using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;
                     

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 10; i++)
            list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);
        return list;
    }
}



public class GameManagerSrc : MonoBehaviour
{
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand,
                     EnemyField, PlayerField;
    public GameObject CardPref;
    int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeTxt;
    public Button EndTurnBtn;

    public int PlayerMana = 10, EnemyMana = 10;
    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt;

    public int PlayerHP, EnemyHP;
    public TextMeshProUGUI PlayerHPTxt, EnemyHPTxt;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public AttakedHero EnemyHero, PlayerHero;

    public List<CardInfoSrc> PlayerHandCards = new List<CardInfoSrc>(),
                             PlayerFieldCards = new List<CardInfoSrc>(),
                             EnemyHandCards = new List<CardInfoSrc>(),
                             EnemyFieldCards = new List<CardInfoSrc>();


    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
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

    void StartGame()
    {
        Turn = 0;
        EndTurnBtn.interactable = true;

        CurrentGame = new Game();

        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);

        PlayerMana = EnemyMana = 10;
        PlayerHP = EnemyHP = 30;

        ShowHP();
        ShowMana();

        ResultGO.SetActive(false);

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

        Card card = deck[0];

        GameObject cardGO = Instantiate(CardPref, hand, false);

        if (hand == EnemyHand)
        {
            cardGO.GetComponent<CardInfoSrc>().HideCardInfo(card);
            EnemyHandCards.Add(cardGO.GetComponent<CardInfoSrc>());
        }
        else
        {
            cardGO.GetComponent<CardInfoSrc>().ShowCardInfo(card, true);
            PlayerHandCards.Add(cardGO.GetComponent<CardInfoSrc>());
            cardGO.GetComponent<AttakedCard>().enabled = false;
        }

        deck.RemoveAt(0);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();

        foreach (var card in PlayerFieldCards)
            card.DeHighlightCard();

        CheckCardsForAvaliability();

        if (IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {
                card.SelfCard.ChangeAttackState(true);
                card.HighlightCard();
            }
                

            while(TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }

            ChangeTurn();
        }
        else
        {
            foreach (var card in EnemyFieldCards)
                card.SelfCard.ChangeAttackState(true);

             StartCoroutine(EnemyTurn(EnemyHandCards));

        }
    }

    IEnumerator EnemyTurn(List<CardInfoSrc> cards)
    {
        yield return new WaitForSeconds(1);

        int count = cards.Count == 1 ? 1 :
                    Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 5 || 
                EnemyMana == 0 ||
                EnemyHandCards.Count == 0)
                break;

            List<CardInfoSrc> cardsList = cards.FindAll(x => EnemyMana >= x.SelfCard.Manacost);

            if (cardsList.Count == 0)
                break;

            cardsList[0].GetComponent<CardMovementSrc>().MoveToField(EnemyField);

            ReduceMana(false, cardsList[0].SelfCard.Manacost);

            yield return new WaitForSeconds(.51f);

            cardsList[0].ShowCardInfo(cardsList[0].SelfCard, false);
            cardsList[0].transform.SetParent(EnemyField);

            EnemyFieldCards.Add(cardsList[0]);
            EnemyHandCards.Remove(cardsList[0]);
        }

        yield return new WaitForSeconds(.51f);

        foreach (var activeCard in EnemyFieldCards.FindAll(x => x.SelfCard.CanAttack))
        {
            if (Random.Range(0, 2) == 0 &&
                PlayerFieldCards.Count > 0)
            {

                var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

                Debug.Log(activeCard.SelfCard.Name + " (" + activeCard.SelfCard.Defense + "---> " +
                    enemy.SelfCard.Name + " (" + enemy.SelfCard.Attack + ";" + enemy.SelfCard.Defense + ")");

                activeCard.SelfCard.ChangeAttackState(false);

                activeCard.GetComponent<CardMovementSrc>().MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                CardsFight(enemy, activeCard);

            }
            else
            {
                Debug.Log(activeCard.SelfCard.Name + " (" + activeCard.SelfCard.Attack + ") Attacked hero");

                activeCard.SelfCard.ChangeAttackState(false);

                activeCard.GetComponent<CardMovementSrc>().MoveToTarget(PlayerHero.transform);
                yield return new WaitForSeconds(.75f);

                DamageHero(activeCard, false);
            }

            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(1);
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;

        EndTurnBtn.interactable = IsPlayerTurn;

        if (IsPlayerTurn)
        {
            GiveNewCards();

            PlayerMana = EnemyMana = 10;
            ShowMana();
        }

        StartCoroutine(TurnFunc());
    }

    void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }

    public void CardsFight(CardInfoSrc playerCard, CardInfoSrc enemyCard)
    {
        playerCard.SelfCard.GetDamage(enemyCard.SelfCard.Attack);
        enemyCard.SelfCard.GetDamage(playerCard.SelfCard.Attack);

        if (!playerCard.SelfCard.IsAlive)
            DestroyCard(playerCard);
        else
            playerCard.RefreshData();

        if (!enemyCard.SelfCard.IsAlive)
            DestroyCard(enemyCard);
        else
            enemyCard.RefreshData();
    }

    void DestroyCard(CardInfoSrc card)
    {
        card.GetComponent<CardMovementSrc>().OnEndDrag(null);

        if (EnemyFieldCards.Exists(x => x == card))
            EnemyFieldCards.Remove(card);

        if (PlayerFieldCards.Exists(x => x == card))
            PlayerFieldCards.Remove(card);

        Destroy(card.gameObject);
    }

    void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }

    void ShowHP()
    {
        PlayerHPTxt.text = PlayerHP.ToString();
        EnemyHPTxt.text = EnemyHP.ToString();
    }

    public void ReduceMana(bool playerMana, int manacost)
    {
        if (playerMana)
            PlayerMana = Mathf.Clamp(PlayerMana - manacost, 0, int.MaxValue);
        else
            EnemyMana = Mathf.Clamp(EnemyMana - manacost, 0, int.MaxValue);

        ShowMana();
    }

    public void DamageHero(CardInfoSrc card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
            EnemyHP = Mathf.Clamp(EnemyHP - card.SelfCard.Attack, 0, int.MaxValue);
        else
            PlayerHP = Mathf.Clamp(PlayerHP - card.SelfCard.Attack, 0, int.MaxValue);

        ShowHP();
        card.DeHighlightCard();
        CheckForResult();
    }

    void CheckForResult()
    {
        if (EnemyHP == 0 || PlayerHP == 0)
        {
            ResultGO.SetActive(true);
            StopAllCoroutines();

            if (EnemyHP == 0)
                ResultTxt.text = "YOU WIN";
            else
                ResultTxt.text = "YOU LOSE";
                
        }
    }

    public void CheckCardsForAvaliability()
    {
        foreach (var card in PlayerHandCards)
            card.CheckForAvailability(PlayerMana);
    }

    public void HighlightTargets(bool highlight)
    {
        foreach (var card in EnemyFieldCards)
            card.HighlightAsTarget(highlight);

        EnemyHero.HighlightAsTarget(highlight); 
    }

}
