using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TrainerItemUI : MonoBehaviour
{
    [SerializeField] Button purchaseButton;
    [SerializeField] Toggle equipButton;

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI trainerName;
    [SerializeField] TextMeshProUGUI perkDescription;

    [SerializeField] GameObject PermanentUpgradeParent;
    [SerializeField] UpgradeItemUI UpgradeItemUIPrefab;

    public DogTrainer myTrainer;

    public void Init(DogTrainer trainer, bool isTrainerEquipped)
    {
        myTrainer = trainer;

        if (myTrainer.isUnlocked)
        {
            equipButton.onValueChanged.AddListener(OnActivateToggle);
            if (isTrainerEquipped)
            {
                equipButton.isOn = true;
            }

        }
        else
        {
            purchaseButton.onClick.AddListener(OnButtonClick);
        }

        icon.sprite = myTrainer.icon;
        trainerName.text = myTrainer.name;

        foreach (UpgradeDataSO upgrade in myTrainer.permanentUpgrades)
        {
            UpgradeItemUI item = Instantiate(UpgradeItemUIPrefab, PermanentUpgradeParent.transform);
            item.Init(upgrade);
        }

        price.text = myTrainer.price.ToString();
        perkDescription.text = myTrainer.activatedUpgrade.description;

        UpdateUI();
    }
    public void OnDestroy()
    {
        purchaseButton.onClick.RemoveAllListeners();
        equipButton.onValueChanged.RemoveAllListeners();
    }

    private void OnButtonClick()
    {
        //Check if we can purchase item


        //Call currency manager to remove price of item
        PurchaseTrainer();


        //UpdateUI
        UpdateUI();
    }

    private void PurchaseTrainer()
    {
        if (!myTrainer.isUnlocked)
        {
            myTrainer.isUnlocked = true;

            purchaseButton.onClick.RemoveAllListeners();
            equipButton.onValueChanged.AddListener(OnActivateToggle);

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //Check if we can afford this item
        if (myTrainer.isUnlocked)
        {
            purchaseButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
        }
    }

    private void OnActivateToggle(bool isActive)
    {
        TrainerShop.Instance.SetTrainer((isActive) ? myTrainer : null);
        UpdateUI();
    }

    public void ToggleOff()
    {
        equipButton.isOn = false;
    }
}
