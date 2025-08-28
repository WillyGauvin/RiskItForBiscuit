using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeItemUI : MonoBehaviour
{
    [SerializeField] Button purchaseButton;
    [SerializeField] TextMeshProUGUI price;

    [SerializeField] TextMeshProUGUI description;

    private UpgradeDataSO myUpgrade;

    private void OnDestroy()
    {
        purchaseButton.onClick.RemoveAllListeners();
    }

    public void Init(UpgradeDataSO upgrade)
    {
        purchaseButton.onClick.AddListener(Buy);

        myUpgrade = upgrade;
        description.text = myUpgrade.description.ToString();

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!myUpgrade.isUnlocked)
        {
            price.text = myUpgrade.price.ToString();
        }
        else
        {
            price.text = "SOLD";
            purchaseButton.interactable = false;
        }
    }

    public void Buy()
    {
        if (!myUpgrade.isUnlocked)
        {
            if (ScoreManager.instance.currentMoney < myUpgrade.price) { Debug.Log("Not Enough Cash."); return; }

            myUpgrade.isUnlocked = true;

            ScoreManager.instance.SpendMoney((uint)myUpgrade.price);

            UpgradeManager.Instance.AddUpgrade(myUpgrade);

            UpdateUI();
        }
    }
}
