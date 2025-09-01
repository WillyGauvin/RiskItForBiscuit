using UnityEngine;

public class DebtShop : Shop
{
    public static DebtShop Instance { get; private set; }

    private DebtDataSO runtimeData;

    [Header("LoanData")]
    [SerializeField] public float gameOverLoanAmount = 25000.0f;

    [Header("LoanItemUI")]
    [SerializeField] Transform loanListParent;
    [SerializeField] LoanItemUI loanItemPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        runtimeData = DebtSystem.runtimeData;

        LoadShop();
    }

    private void LoadShop()
    {
        foreach(Loan loan in runtimeData.loans)
        {
            LoanItemUI loanItem = Instantiate(loanItemPrefab, loanListParent);
            loanItem.Init(loan);
        }
    }

    public void AddLoan(int loanAmount, float interest)
    {
        Loan newLoan = new Loan(loanAmount, interest);

        runtimeData.loans.Add(newLoan);
        LoanItemUI newLoanUI = Instantiate(loanItemPrefab, loanListParent);
        newLoanUI.Init(newLoan);
    }
    public void RemoveLoan(Loan loan)
    {
        runtimeData.RemoveLoan(loan);

        if (runtimeData.loans.Count <= 0)
        {
            //Call game over
            GameManager.instance.TriggerGameVictory();

            LevelLoader.Instance.LoadNextLevel();
        }
    }

    public int GetTotalLoans()
    {
        return runtimeData.loans.Count;
    }

    public void DebugAddInterest()
    {
        foreach (Loan loan in runtimeData.loans)
        {
            loan.ApplyInterest();
        }
    }
}
