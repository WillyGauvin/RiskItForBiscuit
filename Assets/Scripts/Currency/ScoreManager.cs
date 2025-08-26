using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Score is to accumulated via:
/// 
/// Frisbee caught.
/// Distance travelled before hitting water.
/// Tricks performed while airborn.
/// Colliding with beneficial obstacles during a dive.
/// 
/// </summary>

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

    [field: SerializeField] public int currentScore { get; private set; }
    // Allows for score and money to not be 1:1. Huge score = dopamine, but not infinite money.
    [SerializeField, Range(0, 1)] float scoreMoneyConversion = 0.1f;

    [field: SerializeField] public float currentMoney { get; private set; }

    private void Awake()
    {
        ResetScore();
        ResetMoney();
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

    #region Score and Value Resets

    /// <summary>
    /// Reset current score back to default.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
    }

    /// <summary>
    /// Reset current money back to default.
    /// </summary>
    public void ResetMoney()
    {
        currentMoney = 0.0f;
    }

    /// <summary>
    /// Add a value into score.
    /// </summary>
    /// <param name="value">Score to add. Unsigned integer to ensure score cannot be negative.</param>
    public void AddToScore(uint value)
    {
        currentScore += (int)value;
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
    }

    /// <summary>
    /// Converts earned score into money to spend using a conversion value.
    /// </summary>
    public void ConvertScoreToMoney()
    {
        if (currentScore <= 0) { return; }

        currentMoney = currentScore * scoreMoneyConversion;

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
    }

    #endregion
}
