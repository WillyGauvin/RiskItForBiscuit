using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TrainerItemUI : MonoBehaviour
{
    [SerializeField] Button purchaseButton;
    [SerializeField] Button equipButton;

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI trainerName;
    [SerializeField] TextMeshProUGUI perkDescription;

    [SerializeField] GameObject PermanentUpgradeParent;
    [SerializeField] UpgradeItemUI UpgradeItemUIPrefab;

    private DogTrainer myTrainer;

    public void Init(DogTrainer trainer)
    {
        myTrainer = trainer;

        purchaseButton.onClick.AddListener(OnButtonClick);

        icon = myTrainer.icon;
        trainerName.text = myTrainer.name;

        foreach (UpgradeDataSO upgrade in myTrainer.permanentUpgrades)
        {
            UpgradeItemUI item = Instantiate(UpgradeItemUIPrefab, PermanentUpgradeParent.transform);
            item.Init(upgrade);
        }

        UpdateUI();
    }
    public void OnDestroy()
    {
        purchaseButton.onClick.RemoveAllListeners();
    }

    private void OnButtonClick()
    {
        //Check if we can purchase item

        //Add to inventory


        //Call currency manager to remove price of item

        //Increase price of item

        //UpdateUI
        UpdateUI();
    }

    private void UpdateUI()
    {
        //Check if we can afford this item

        price.text = myTrainer.price.ToString();
    }
}
