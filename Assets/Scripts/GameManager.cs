using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager thisInstance;

    public static GameManager instance
    {
        get 
        {
            if (!thisInstance)
            {
                // Spawns GameManager prefab and sets component reference.
                GameObject manager = Instantiate(Resources.Load<GameObject>("Managers/GameManager"));
            }

            return thisInstance; 
        } 

        private set => thisInstance = value;
    }

    private void Awake()
    {
        if (thisInstance == null)
        {
            thisInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        scoreManager = gameObject.GetComponent<ScoreManager>();
        dayManager = gameObject.GetComponent<DayManager>();
        debtSystem = gameObject.GetComponent<DebtSystem>();

        AudioManager.instance.ForceLoad();
    }

    public void TriggerGameOver()
    {
        isGameEnded = true;
        badEnding = true;
        Debug.Log("Debt was not paid off on time. Game Over.");
    }

    public void TriggerGameVictory()
    {
        isGameEnded = true;
    }

    public void SetPlayer(GameObject obj)
    {
        if (player == null) { player = obj; }
    }

    private bool isGameEnded = false;
    public bool IsGameEnded => isGameEnded;

    private bool badEnding = false;
    public bool IsBadEnding => badEnding;

    private GameObject player;
    public GameObject Player => player;

    // Score/Currency
    private static ScoreManager scoreManager;
    public ScoreManager ScoreManager => scoreManager;

    // Day Tracking
    private static DayManager dayManager;
    public DayManager DayManager => dayManager;

    // Loans
    private static DebtSystem debtSystem;
    public DebtSystem DebtSystem => debtSystem;
}
