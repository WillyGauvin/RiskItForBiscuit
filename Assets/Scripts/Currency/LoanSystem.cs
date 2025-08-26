using UnityEngine;

public class LoanSystem : MonoBehaviour
{
    private static LoanSystem thisInstance;

    public static LoanSystem instance
    {
        get
        {
            if (!thisInstance)
            {
                thisInstance = GameManager.instance.LoanSystem;
            }

            return thisInstance;
        }

        private set => thisInstance = value;
    }

    [SerializeField] float debtRemaining = 20000;
    public float DebtRemaining => debtRemaining;

    [SerializeField] float debtInterest = 1.045f; // 4.5% interest
    public float DebtInterest => debtInterest;

    /// <summary>
    /// Adds a given amount to your total debt.
    /// </summary>
    /// <param name="amount">Amount to add to debt.</param>
    public void AddToDebt(uint amount)
    {
        debtRemaining += amount;
    }
    
    /// <summary>
    /// Removes a given amount from your debt.
    /// </summary>
    /// <param name="amount">Amount to subtract from debt.</param>
    public void RemoveFromDebt(uint amount) 
    { 
        debtRemaining -= amount;
    }

    /// <summary>
    /// Applies weekly interest to your debt.
    /// </summary>
    public void ApplyDebtInterest()
    {
        debtRemaining *= debtInterest;
    }
}
