using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DayManager : MonoBehaviour
{
    private static DayManager thisInstance;

    public static DayManager instance
    {
        get
        {
            if (!thisInstance)
            {
                thisInstance = GameManager.instance.DayManager;
            }

            return thisInstance;
        }

        private set => thisInstance = value;
    }

    [field: SerializeField] public uint currentDay {get; private set;}
    const int totalDays = 30;

    [field: SerializeField] public uint numDivesRemaining { get; private set; }
    const int numDivesPerDay = 3;

    [SerializeField] public UnityEvent UpdateDayCount = new UnityEvent();
    [SerializeField] public UnityEvent UpdateDivesCount = new UnityEvent();

    private void Awake()
    {
        currentDay = 0;
        numDivesRemaining = numDivesPerDay;
    }

    private void Start()
    {
        StartNewDay();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
        {
            StartNewDay();
        }

        if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
        {
            EndCurrentDay();
        }
    }
#endif

    /// <summary>
    /// Begins a new day, increasing day counter and resetting the number of dives available.
    /// </summary>
    public void StartNewDay()
    {
        currentDay++;

        if (currentDay >= totalDays)
        {
            // Debt was not paid off on time. Game Over.
            Debug.Log("Debt was not paid off on time. Game Over.");
        }

        numDivesRemaining = numDivesPerDay;

        UpdateDayCount.Invoke();
    }

    /// <summary>
    /// Reduces dives remaining. 
    /// If no more dives remain, end the day.
    /// </summary>
    public bool DivePerformed()
    {
        numDivesRemaining--;

        UpdateDivesCount.Invoke();

        if (numDivesRemaining <= 0)
        {
            EndCurrentDay();
            return false;
        }
        return true;
    }

    /// <summary>
    /// Ends the current day, turning accumulated score into money for the next day.
    /// </summary>
    void EndCurrentDay()
    {
        ScoreManager.instance.ConvertScoreToMoney();

        LevelLoader.Instance.LoadNextLevel();

        // For now, just immediately start the next day.
        StartNewDay();
    }
}
