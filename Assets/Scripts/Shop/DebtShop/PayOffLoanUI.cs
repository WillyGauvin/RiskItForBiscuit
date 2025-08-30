using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PayOffLoanUI : MonoBehaviour
{
    public static PayOffLoanUI instance { get; private set; }

    [Header("ShownLoanInfo")]
    [SerializeField] TextMeshProUGUI tomorrowsDebt;
    [SerializeField] TextMeshProUGUI interestText;

    [SerializeField] Slider payoffLoanSlider;
    [SerializeField] TextMeshProUGUI payoffLoanAmount;
    [SerializeField] Button payOffLoanButton;
    [SerializeField] TextMeshProUGUI payoffLoanText;

    private LoanItemUI currentShownLoan;
    private int payOffAmount = 0;
    private float minPayOffAmount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Other loanUI found");
        }
        ScoreManager.instance.UpdateMoney.AddListener(UpdateUI);
        UpdateUI();
    }
    void Start()
    {
        payoffLoanSlider.onValueChanged.AddListener(OnSliderChanged);
        payOffLoanButton.onClick.AddListener(PayOffLoan);
    }

    private void OnDestroy()
    {
        payoffLoanSlider.onValueChanged.RemoveAllListeners();
        payOffLoanButton.onClick.RemoveAllListeners();
        ScoreManager.instance.UpdateMoney.RemoveListener(UpdateUI);

    }

    private void OnSliderChanged(float percent)
    {
        if (currentShownLoan != null)
        {
            payOffAmount = (int)Mathf.Lerp(minPayOffAmount, currentShownLoan.myLoan.balance, percent);

            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        if (currentShownLoan != null)
        {
            tomorrowsDebt.text = "$ " + currentShownLoan.myLoan.GetBalanceWithInterest().ToString();
            interestText.text = (currentShownLoan.myLoan.dailyInterest * 100).ToString() + "%";
            payoffLoanAmount.text = "$ " + payOffAmount.ToString();
        }
        else
        {
            tomorrowsDebt.text = "--";
            interestText.text = "--";
            payoffLoanAmount.text = "--";
        }

        if (ScoreManager.instance.CanAfford((uint)payOffAmount))
        {
            payOffLoanButton.interactable = true;
            payoffLoanText.text = "Pay Off Loan";
            payoffLoanAmount.color = Color.white;
        }
        else
        {
            payOffLoanButton.interactable = false;
            payoffLoanText.text = "Insufficient Funds";
            payoffLoanAmount.color = Color.red;
        }
    }

    private void PayOffLoan()
    {
        if (currentShownLoan != null)
        {
            if (ScoreManager.instance.CanAfford((uint)payOffAmount))
            {

                ScoreManager.instance.SpendMoney((int)payOffAmount);
                currentShownLoan.PayOff(payOffAmount);

                if (currentShownLoan.myLoan.balance <= 0.0f)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_paidOffLoan);
                    DebtShop.Instance.RemoveLoan(currentShownLoan.myLoan);
                    Destroy(currentShownLoan.gameObject);
                    currentShownLoan = null;
                }
                else
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.shop_payLoan);

                }
                UpdateUI();
            }
        }
    }

    public void ShowLoanStats(LoanItemUI loan)
    {
        currentShownLoan = loan;
        payoffLoanSlider.value = 0.0f;
        UpdateUI();
    }
}
