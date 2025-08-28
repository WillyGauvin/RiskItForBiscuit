using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu]
public class DebtDataSO : ScriptableObject
{
    public List<Loan> loans = new List<Loan>();

    public void RemoveLoan(Loan loan)
    {
        if (!loans.Remove(loan))
        {
            Debug.LogError("Loan was not found");
        }
    }
}

[Serializable]
public class Loan
{
    [field: SerializeField] public float balance;

    [field: SerializeField] public float dailyInterest;

    public Loan(float balance, float dailyInterest)
    {
        this.balance = balance;
        this.dailyInterest = dailyInterest;
    }

    public float GetBalanceWithInterest()
    {
        float newBalance = balance + balance * dailyInterest;
        newBalance = Mathf.Round(newBalance * 100) / 100f;

        return newBalance;
    }

    public void ApplyInterest()
    {
        balance = GetBalanceWithInterest();
    }

    public void PayOff(float amount)
    {
        balance -= amount;
        balance = Mathf.Max(balance, 0);
    }
}
