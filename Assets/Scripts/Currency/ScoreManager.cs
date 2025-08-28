using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Score is to accumulated via:
/// 
/// Distance travelled before hitting water.
/// Tricks performed while airborn.
/// Colliding with beneficial obstacles during a dive.
/// 
/// </summary>
/// 
public enum ScoreStats
{
    Flip,
    Hoop,
    Frisbee,
    EarnedScore,
    Max
}

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager thisInstance;

    public static ScoreManager instance
    {
        get
        {
            if (!thisInstance)
            {
                thisInstance = GameManager.instance.ScoreManager;
            }

            return thisInstance;
        }

        private set => thisInstance = value;
    }

    // Score
    [Header("Score")]
    [SerializeField] float reductionMultiplier = 0.5f;

    [field: SerializeField] public int numFlips { get; private set; }
    const int scoreForFlip = 250;
    [field: SerializeField] public int numHoops { get; private set; }
    const int scoreForHoop = 250;
    [field: SerializeField] public bool wasFrisbeeCaught { get; private set; }

    [field: SerializeField] public int earnedScoreForJump { get; private set; }

    [field: SerializeField] public int totalScore { get; private set; }
    // Allows for score and money to not be 1:1. Huge score = dopamine, but not infinite money.
    [SerializeField, Range(0, 1)] float scoreMoneyConversion = 0.1f;

    // Money
    [Header("Money")]
    [field: SerializeField] public float currentMoney { get; private set; }
    const float startingFunds = 50.0f;

    [Header("Events")]
    [SerializeField] public UnityEvent UpdateTotalScore = new UnityEvent();
    [SerializeField] public UnityEvent UpdateMoney = new UnityEvent();

    private void Awake()
    {
        ResetScore();
        ResetMoney();

        currentMoney = startingFunds;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            ResetScore();
            ResetMoney();
        }

        if (Keyboard.current.minusKey.wasPressedThisFrame)
        {
            AddToScore(500);
        }

        if (Keyboard.current.equalsKey.wasPressedThisFrame)
        {
            AddMoney(500);
        }
    }
#endif

    #region Value Resets

    /// <summary>
    /// Reset the tracked stats for an individual dive.
    /// </summary>
    public void ResetDive()
    {
        wasFrisbeeCaught = false;
        numFlips = 0;
        numHoops = 0;
        earnedScoreForJump = 0;
    }

    /// <summary>
    /// Reset total score back to default.
    /// </summary>
    void ResetScore()
    {
        ResetDive();
        totalScore = 0;
    }

    /// <summary>
    /// Reset current money back to default.
    /// </summary>
    void ResetMoney()
    {
        currentMoney = 0.0f;
        UpdateMoney?.Invoke();
    }

    #endregion

    #region Score

    /// <summary>
    /// Add a value into earned score for that jump.
    /// </summary>
    /// <param name="value">Score to add. Unsigned integer to ensure score cannot be negative.</param>
    public void AddToScore(uint value)
    {
        earnedScoreForJump += (int)value;
    }

    /// <summary>
    /// Flip performed during a dive.
    /// </summary>
    public void FlipPerformed()
    {
        numFlips++;
        earnedScoreForJump += scoreForFlip;
    }

    /// <summary>
    /// Hoop jumped through during a dive.
    /// </summary>
    public void HoopPerformed()
    {
        numHoops++;
        earnedScoreForJump += scoreForHoop;
    }

    /// <summary>
    /// Frisbee caught during a dive.
    /// </summary>
    public void SetFrisbeeCaught()
    {
        wasFrisbeeCaught = true;
    }

    /// <summary>
    /// Add the points earned during a jump into the total points for the day.
    /// Multiply by the penalty if frisbee was not caught.
    /// </summary>
    public void TotalPointsForJump()
    {
        // Keep all points if frisbee was caught- else, point penalty.
        if (!wasFrisbeeCaught)
        {
            // Half score
            earnedScoreForJump = (int)(earnedScoreForJump * reductionMultiplier);
        }
        totalScore += earnedScoreForJump;
        UpdateTotalScore?.Invoke();
    }

    #endregion

    #region Money

    /// <summary>
    /// Adds an amount to your current funds.
    /// </summary>
    /// <param name="value">Amount of money to add.</param>
    public void AddMoney(uint value)
    {
        currentMoney += value;

        UpdateMoney?.Invoke();
    }

    /// <summary>
    /// Converts earned score into money to spend using a conversion value.
    /// </summary>
    public void ConvertScoreToMoney()
    {
        if (totalScore <= 0) { return; }

        currentMoney = totalScore * scoreMoneyConversion;

        UpdateTotalScore?.Invoke();

        ResetScore();
    }

    /// <summary>
    /// Spend your money in the shop.
    /// </summary>
    /// <param name="price">Price of item/upgrade purchased.</param>
    public void SpendMoney(uint price)
    {
        if (price <= 0 ||
            currentMoney - price < 0) { return; }

        currentMoney -= price;

        UpdateMoney?.Invoke();
    }

    public bool CanAfford(uint price)
    {
        return (currentMoney - price > 0);
    }

    #endregion
}
