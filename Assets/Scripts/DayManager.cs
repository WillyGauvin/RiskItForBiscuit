using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager instance;

    public uint currentDay {get; private set;}
    const int totalDays = 30;

    public uint numDivesRemaining { get; private set; }
    const int numDivesPerDay = 3;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Day Night Manager in the scene.");
        }
        instance = this;

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
            // Debt was not paid on time. Game Over.
        }

        numDivesRemaining = numDivesPerDay;
    }

    /// <summary>
    /// Reduces dives remaining. 
    /// If no more dives remain, end the day.
    /// </summary>
    public void DivePerformed()
    {
        numDivesRemaining--;

        if (numDivesRemaining <= 0)
        {
            EndCurrentDay();
        }
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
