using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;

public class LoanItemUI : MonoBehaviour
{
    private Loan myLoan;

    [SerializeField] private Image slider;
    [SerializeField] TextMeshProUGUI loanAmount;
    [SerializeField] Button priceTag;

    public void Init(Loan loan)
    {
        myLoan = loan;

        UpdateUI();
    }

    private void UpdateUI()
    {
        slider.fillAmount = myLoan.balance / DebtShop.Instance.gameOverLoanAmount;
        loanAmount.text = myLoan.balance.ToString();
    }
}
