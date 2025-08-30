using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TakeOutLoanUI : MonoBehaviour
{
    [Header("NewLoan")]
    [SerializeField] Slider takeOutLoanSlider;
    [SerializeField] TextMeshProUGUI takeOutLoanText;
    [SerializeField] TextMeshProUGUI takeOutInterestText;
    [SerializeField] Button takeOutLoanButton;
    [SerializeField] TextMeshProUGUI loanButtonText;
    private int loanAmount = 0;
    private float interest = 0.0f;
    private int minLoan = 300;
    private int maxLoan = 1500;
    private float minInterest = 0.025f;
    private float maxInterest = 0.10f;
    private int maxAmountOfLoans = 7;

    private void Start()
    {
        takeOutLoanSlider.onValueChanged.AddListener(UpdateNewLoanUI);
        takeOutLoanButton.onClick.AddListener(TakeOutLoan);
    }

    private void OnDestroy()
    {
        takeOutLoanSlider.onValueChanged.RemoveAllListeners();
        takeOutLoanButton.onClick.RemoveAllListeners();
    }

    private void UpdateUI()
    {
        if (DebtShop.Instance.GetTotalLoans() >= maxAmountOfLoans)
        {
            loanButtonText.text = "Max Loans";
            takeOutLoanButton.interactable = false;
        }
        else
        {
            loanButtonText.text = "Take Out Loan";
            takeOutLoanButton.interactable = true;
        }
    }

    public void UpdateNewLoanUI(float percentage)
    {
        loanAmount = (int)Mathf.Lerp(minLoan, maxLoan, percentage);

        takeOutLoanText.text = "$ " + loanAmount.ToString();

        interest = Mathf.Round(CalculateInterestRate(loanAmount) * 1000) / 1000;

        takeOutInterestText.text = (interest * 100).ToString() + "%";
    }

    public float CalculateInterestRate(float loanAmount)
    {
        // Clamp loan within range
        loanAmount = Mathf.Clamp(loanAmount, minLoan, maxLoan);

        // Normalize (0 = minLoan, 1 = maxLoan)
        float t = (loanAmount - minLoan) / (maxLoan - minLoan);

        // Inverse lerp: bigger loan = closer to minInterest
        float interestRate = Mathf.Lerp(minInterest, maxInterest, t);

        return interestRate;
    }

    public void TakeOutLoan()
    {
        if (DebtShop.Instance.GetTotalLoans() >= maxAmountOfLoans)
        {
            return;
        }

        DebtShop.Instance.AddLoan(loanAmount, interest);

        ScoreManager.instance.AddMoney(loanAmount);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_buy_trainer3);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_takeLoan);


        UpdateUI();
    }

}
