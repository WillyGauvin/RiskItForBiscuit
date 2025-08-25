using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }

    public uint currentScore { get; private set; }
    [SerializeField, Range(0, 1)] float scoreMoneyConversion = 0.1f;

    public int currentMoney { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Score Manager in the scene.");
        }
        instance = this;

        ResetScore();
        ResetMoney();
    }

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
        currentMoney = 0;
    }

    /// <summary>
    /// Add a value into score.
    /// </summary>
    /// <param name="value">Unsigned integer to ensure score cannot be negative.</param>
    public void AddToScore(uint value)
    {
        currentScore += value;
    }

    /// <summary>
    /// Converts earned score into money to spend using a conversion value.
    /// </summary>
    public void ConvertScoreToMoney()
    {
        if (currentScore <= 0) { return; }

        currentMoney = (int)(currentScore * scoreMoneyConversion);

        ResetScore();
    }

    /// <summary>
    /// Spend your money in the shop.
    /// </summary>
    /// <param name="price">Price of item/upgrade purchased.</param>
    public void SpendMoney(uint price)
    {
        if (price <= 0) { return; }

        currentMoney -= (int)price;
    }
}
