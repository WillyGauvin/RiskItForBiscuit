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
    [field: SerializeField] public int balance;

    [field: SerializeField] public float dailyInterest;

    public Loan(int balance, float dailyInterest)
    {
        this.balance = balance;
        this.dailyInterest = dailyInterest;
    }

    public int GetBalanceWithInterest()
    {
        int newBalance = balance + (int)(balance * dailyInterest);

        return newBalance;
    }

    public void ApplyInterest()
    {
        balance = GetBalanceWithInterest();
        Debug.Log("new Balance: " + balance);
    }

    public void PayOff(int amount)
    {
        balance -= amount;
        balance = Mathf.Max(balance, 0);
    }
}
