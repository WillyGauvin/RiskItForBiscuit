using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeItemUI : MonoBehaviour
{
    [SerializeField] Button purchaseButton;
    [SerializeField] TextMeshProUGUI price;

    [SerializeField] TextMeshProUGUI description;

    [SerializeField] Image padlock;

    private UpgradeDataSO myUpgrade;
    private DogTrainer myTrainer;

    private void OnDestroy()
    {
        purchaseButton.onClick.RemoveAllListeners();
        ScoreManager.instance.UpdateMoney.RemoveListener(UpdateUI);
    }

    public void Init(UpgradeDataSO upgrade, DogTrainer trainer)
    {
        myTrainer = trainer;
        purchaseButton.onClick.AddListener(Buy);

        myUpgrade = upgrade;
        description.text = myUpgrade.description.ToString();

        UpdateUI();

        ScoreManager.instance.UpdateMoney.AddListener(UpdateUI);
    }

    public void UpdateUI()
    {
        if (!myUpgrade.isUnlocked)
        {
            if (!myTrainer.isUnlocked)
            {
                padlock.gameObject.SetActive(true);
                price.text = "";
                purchaseButton.interactable = false;

            }
            else
            {
                padlock.gameObject.SetActive(false);

                price.text = myUpgrade.price.ToString();

                if (ScoreManager.instance.CanAfford((uint)myUpgrade.price))
                {
                    purchaseButton.interactable = true;
                }
                else
                {
                    purchaseButton.interactable = false;
                }
            }
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
            if (ScoreManager.instance.CanAfford((uint)myUpgrade.price))
            {
                myUpgrade.isUnlocked = true;
                UpgradeManager.Instance.AddUpgrade(myUpgrade);
                ScoreManager.instance.SpendMoney((uint)myUpgrade.price);

                UpdateUI();
            }
        }
    }
}
