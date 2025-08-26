using UnityEngine;
using UnityEngine.InputSystem;

public class DebtSystem : MonoBehaviour
{
    private static DebtSystem thisInstance;

    public static DebtSystem instance
    {
        get
        {
            if (!thisInstance)
            {
                thisInstance = GameManager.instance.DebtSystem;
            }

            return thisInstance;
        }

        private set => thisInstance = value;
    }

    [SerializeField] float debtRemaining = 20000;
    public float DebtRemaining => debtRemaining;

    [SerializeField] float debtInterest = 1.045f; // 4.5% interest
    public float DebtInterest => debtInterest;

#if UNITY_EDITOR
    private void Update()
    {
        if (Keyboard.current.backslashKey.wasPressedThisFrame)
        {
            ApplyDebtInterest();
        }

        if (Keyboard.current.semicolonKey.wasPressedThisFrame)
        {
            RemoveFromDebt(500);
        }

        if (Keyboard.current.quoteKey.wasPressedThisFrame)
        {
            AddToDebt(500);
        }
    }
#endif

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

        if (debtRemaining <= 0)
        {
            Debug.Log("You are winner! Bye bye debt");
        }
    }

    /// <summary>
    /// Applies interest rate to your debt.
    /// </summary>
    public void ApplyDebtInterest()
    {
        debtRemaining *= debtInterest;
    }
}
