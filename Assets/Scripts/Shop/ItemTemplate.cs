using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTemplate : MonoBehaviour
{
    [Header("Item Data (for display)")]
    [SerializeField] private string ItemName; //The Name of the item
    [SerializeField] private float CostOfItem; //The Current Price of the Item to buy;
    [SerializeField] private float PriceIncreaseFactor = 0.2f; //Factor each price is increased after succefully buying/upgrading
    [SerializeField] private int NumOfUpgrades; //Max Level of item
    [SerializeField] private int CurrentUpgradeLevel; //Current Level of item
    [SerializeField] private bool HasBeenPurchased; //bool to check if the item has been bought and can be update if avaible

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI buttonTextRef; //a reference to the UI's button text
    [SerializeField] private TextMeshProUGUI itemNameTextRef; // a reference to the UI's Card name text
    [SerializeField] private Button buyButton;   // drag in Inspector on prefab

    private ItemTemplate sourceItem; // the real data source

    private void Start()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyPressed);
    }
    
    public void BindTo(ItemTemplate realItem)
    {
        sourceItem = realItem;

        // Copy values into display
        ItemName = realItem.ItemName;
        CostOfItem = realItem.CostOfItem;
        PriceIncreaseFactor = realItem.PriceIncreaseFactor;
        NumOfUpgrades = realItem.NumOfUpgrades;
        CurrentUpgradeLevel = realItem.CurrentUpgradeLevel;
        HasBeenPurchased = realItem.HasBeenPurchased;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!HasBeenPurchased)
        {
            buttonTextRef.text = $"Buy For: {CostOfItem:F0}";
        }
        else if (CurrentUpgradeLevel < NumOfUpgrades)
        {
            buttonTextRef.text = $"Upgrade ({CurrentUpgradeLevel}/{NumOfUpgrades}) - {CostOfItem:F0}";
        }
        else
        {
            buttonTextRef.text = "Max Level Reached";
        }

        itemNameTextRef.text = ItemName;
    }

    private void OnBuyPressed()
    {
        if (sourceItem != null)
        {
            // Try to buy on the REAL item
            bool success = sourceItem.BuyItem();

            // Update my display to reflect latest data
            if (success)
            {
                // sync values from source after purchase/upgrade
                BindTo(sourceItem);
            }
        }
    }


    public bool BuyItem()
    {
        if (!HasBeenPurchased)
        {
            if (ScoreManager.instance.currentMoney >= CostOfItem)
            {
                ScoreManager.instance.SpendMoney((uint)CostOfItem);
                HasBeenPurchased = true;

                if (NumOfUpgrades > 0)
                    CostOfItem += CostOfItem * PriceIncreaseFactor;

                UpdateUI();
                return true;
            }
            return false;
        }
        else
        {
            if (NumOfUpgrades > 0 && CurrentUpgradeLevel < NumOfUpgrades)
            {
                if (ScoreManager.instance.currentMoney >= CostOfItem)
                {
                    ScoreManager.instance.SpendMoney((uint)CostOfItem);
                    CurrentUpgradeLevel++;
                    CostOfItem += CostOfItem * PriceIncreaseFactor;

                    UpdateUI();
                    return true;
                }
                return false;
            }
            return false;
        }
    }

    // Expose ItemName so ShopUI can show it in the hierarchy if needed
    public string GetItemName() => ItemName;
}
