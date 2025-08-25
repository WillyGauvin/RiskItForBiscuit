using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }

    public uint currentScore { get; private set; }
    // Allows for score and money to not be 1:1. Huge score = dopamine, but not infinite money.
    [SerializeField, Range(0, 1)] float scoreMoneyConversion = 0.1f;

    public float currentMoney { get; private set; }
    public float debtInterest {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Score Manager in the scene.");
        }
        instance = this;

        ResetScore();
        ResetMoney();
        SetDebtInterest();
    }

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
    /// Set the debt interest.
    /// Can be changed day-to-day if necessary.
    /// </summary>
    /// <param name="interest">Interest to apply to negative funds.</param>
    public void SetDebtInterest(float interest = 1.13f)
    {
        debtInterest = interest;
    }

    /// <summary>
    /// Add a value into score.
    /// </summary>
    /// <param name="value">Score to add. Unsigned integer to ensure score cannot be negative.</param>
    public void AddToScore(uint value)
    {
        currentScore += value;
    }

    #endregion

    #region Money

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
        if (price <= 0) { return; }

        currentMoney -= price;
    }

    /// <summary>
    /// Accumulate a player debt by interest rate.
    /// To be applied at the end of every day if money is in the negatives.
    /// </summary>
    public void AccumulateDebt()
    {
        if (currentMoney >= 0) { return; }

        currentMoney *= debtInterest;
    }

    #endregion
}
