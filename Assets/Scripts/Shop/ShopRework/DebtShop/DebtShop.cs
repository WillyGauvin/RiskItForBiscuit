using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UI;

public class DebtShop : Shop
{
    public static DebtShop Instance { get; private set; }

    [SerializeField] private DebtDataSO debtDataAsset;
    private static DebtDataSO runtimeData;

    [SerializeField] public float gameOverLoanAmount = 25000.0f;


    [SerializeField] Transform loanListParent;
    //[SerializeField] LoanItemUI loanItemPrefab;

    [SerializeField] TextMeshProUGUI tomorrowsDebt;
    [SerializeField] TextMeshProUGUI interesetText;


    [SerializeField] Slider LoanSlider;
    [SerializeField] TextMeshProUGUI takeOutLoanText;
    [SerializeField] TextMeshProUGUI takeOutInterestText;
    private float loanAmount = 0.0f;
    private float interest = 0.0f;
    private float minLoan = 100.0f;
    private float maxLoan = 5000.0f;
    private float minInterest = 0.05f;
    private float maxInterest = 0.30f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (runtimeData == null)
        {
            // First time: make the clone
            runtimeData = Instantiate(debtDataAsset);
        }

        LoadShop();

        LoanSlider.onValueChanged.AddListener(UpdateLoanAmount);
    }

    private void OnDestroy()
    {
        LoanSlider.onValueChanged.RemoveAllListeners();
    }

    private void LoadShop()
    {

    }

    public void TakeOutLoan()
    {
        Loan newLoan = new Loan(loanAmount, interest);
        runtimeData.loans.Add(newLoan);
    }

    public void UpdateLoanAmount(float percentage)
    {
        loanAmount = minLoan + (percentage / maxLoan) * (maxLoan - minLoan);

        loanAmount = Mathf.Round(maxLoan);
        takeOutLoanText.text = loanAmount.ToString();

        interest = Mathf.Round(CalculateInterestRate(loanAmount));

        takeOutInterestText.text = interest.ToString();
    }

    public float CalculateInterestRate( float loanAmount)
    {
        // Clamp loan within range
        loanAmount = Mathf.Clamp(loanAmount, minLoan, maxLoan);

        // Normalize (0 = minLoan, 1 = maxLoan)
        float t = (loanAmount - minLoan) / (maxLoan - minLoan);

        // Inverse lerp: bigger loan = closer to minInterest
        float interestRate = Mathf.Lerp(maxInterest, minInterest, t);

        return interestRate;
    }
}
