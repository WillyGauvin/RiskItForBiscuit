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
            price.text = "PURCHASED";
            purchaseButton.interactable = false;
        }
    }

    public void Buy()
    {
        if (!myUpgrade.isUnlocked)
        {
            myUpgrade.isUnlocked = true;
            UpgradeManager.Instance.AddUpgrade(myUpgrade);

            UpdateUI();
        }
    }
}
