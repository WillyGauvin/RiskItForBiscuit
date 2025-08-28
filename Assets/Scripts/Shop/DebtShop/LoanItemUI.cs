using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;

public class LoanItemUI : MonoBehaviour
{
    public Loan myLoan;

    [SerializeField] private Image slider;
    [SerializeField] TextMeshProUGUI loanAmount;
    [SerializeField] Button priceTag;

    [SerializeField] private RectTransform priceTagParent;

    public void Init(Loan loan)
    {
        priceTag.onClick.AddListener(OnPriceTagClicked);

        myLoan = loan;

        UpdateUI();
    }

    private void OnDestroy()
    {
        priceTag.onClick.RemoveAllListeners();
    }

    private void UpdateUI()
    {
        slider.fillAmount = myLoan.balance / DebtShop.Instance.gameOverLoanAmount;
        loanAmount.text = myLoan.balance.ToString();

        float height = slider.rectTransform.rect.height;

        float yOffset = slider.fillAmount * height;

        Vector3 newPos = slider.rectTransform.localPosition + new Vector3(0, -slider.rectTransform.rect.height * slider.rectTransform.pivot.y + yOffset, 0);

        priceTagParent.localPosition = newPos;

        slider.color = Color.Lerp(Color.green, Color.red, myLoan.balance / DebtShop.Instance.gameOverLoanAmount);
    }

    void OnPriceTagClicked()
    {
        PayOffLoanUI.instance.ShowLoanStats(this);
    }

    public void PayOff(float amount)
    {
        myLoan.PayOff(amount);
        UpdateUI();
    }

}
