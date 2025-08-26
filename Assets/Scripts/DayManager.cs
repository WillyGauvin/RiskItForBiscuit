using UnityEngine;

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

    public uint currentDay {get; private set;}
    const int totalDays = 30;

    public uint numDivesRemaining { get; private set; }
    const int numDivesPerDay = 3;

    private void Awake()
    {
        currentDay = 0;
    }

    private void Start()
    {
        StartNewDay();
    }

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

        // Apply weekly debt interest.
        if ((currentDay % 7) == 0)
        {
            LoanSystem.instance.ApplyDebtInterest();
        }

        numDivesRemaining = numDivesPerDay;
    }

    /// <summary>
    /// Reduces dives remaining. 
    /// If no more dives remain, end the day.
    /// </summary>
    public bool DivePerformed()
    {
        numDivesRemaining--;

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
