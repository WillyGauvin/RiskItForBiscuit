using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu]
public class DebtDataSO : ScriptableObject
{
    public List<Loan> loans = new List<Loan>();
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

    public void ApplyInterest()
    {
        balance *= dailyInterest;
        balance = Mathf.Round(balance * 100) / 100f;
    }

    public void PayOff(float amount)
    {
        balance -= amount;
    }
}
