using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TextMeshProUGUI PlayerMana, EnemyMana;
    public TextMeshProUGUI PlayerHP, EnemyHP;

    public GameObject ResultGo;
    public TextMeshProUGUI ResultTxt;

    public TextMeshProUGUI TurnTime;
    public Button EndTurnBtn;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        
    }



    public void StartGame()
    {
        EndTurnBtn.interactable = true;
        ResultGo.SetActive(false);
        UpdateHPAndMana();
    }

    public void UpdateHPAndMana()
    {
        PlayerMana.text = GameManagerSrc.Instance.CurrentGame.Player.Mana.ToString();
        EnemyMana.text = GameManagerSrc.Instance.CurrentGame.Enemy.Mana.ToString();
        PlayerHP.text = GameManagerSrc.Instance.CurrentGame.Player.HP.ToString();
        EnemyHP.text = GameManagerSrc.Instance.CurrentGame.Enemy.HP.ToString();
    }
    public void ShowResult()
    {

        ResultGo.SetActive(true);
        if (GameManagerSrc.Instance.CurrentGame.Enemy.HP == 0 || GameManagerSrc.Instance.CurrentGame.EnemyDeck.Count == 0)
            ResultTxt.text = "WIN";
        else
            ResultTxt.text = "Lose";
    }
    public void UpdateTurnTime(int time)
    {
        TurnTime.text=time.ToString();
    }

    public void DisableTurnBtn()
    {
        EndTurnBtn.interactable = GameManagerSrc.Instance.IsPlayerTurn;
    }

    
}
