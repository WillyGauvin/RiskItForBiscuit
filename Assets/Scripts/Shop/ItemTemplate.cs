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
    [SerializeField] private bool HasBeenPurchased; //bool to check if the item has been bought and can be update if available
    [SerializeField] private bool canAddDebt; //borrow
    [SerializeField] private bool canRemoveDebt; //pay back
    [SerializeField] private bool isObstacle; //pay back
    [SerializeField] private int obstacleID; //pay back

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI buttonTextRef; //a reference to the UI's button text
    [SerializeField] private TextMeshProUGUI itemNameTextRef; // a reference to the UI's Card name text
    [SerializeField] private Button buyButton;   // drag in Inspector on prefab

    // runtime link back to the source item that this card was initialized from
    private ItemTemplate sourceRef;

    private void Awake()
    {
        // Try to auto-find UI refs if they weren't assigned on the prefab to reduce broken prefabs.
        if (buttonTextRef == null)
            buttonTextRef = GetComponentInChildren<TextMeshProUGUI>(true); // this will pick first TMP child; OK as fallback

        if (itemNameTextRef == null)
        {
            // find TMP that is not the button text (best-effort)
            var tmps = GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var t in tmps)
            {
                if (t != buttonTextRef)
                {
                    itemNameTextRef = t;
                    break;
                }
            }
        }

        if (buyButton == null)
            buyButton = GetComponentInChildren<Button>(true);
    }

    private void Start()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyPressed);
        else
            Debug.LogWarning($"{name}: buyButton not assigned and not found in children. Clicks won't be handled.");

        // make sure visible UI is correct
        UpdateUI();
    }

    public void InitializeFrom(ItemTemplate source)
    {
        if (source == null)
        {
            Debug.LogWarning($"{name}: InitializeFrom called with null source.");
            return;
        }

        // copy the item data values
        ItemName = source.ItemName;
        CostOfItem = source.CostOfItem;
        PriceIncreaseFactor = source.PriceIncreaseFactor;
        NumOfUpgrades = source.NumOfUpgrades;
        CurrentUpgradeLevel = source.CurrentUpgradeLevel;
        HasBeenPurchased = source.HasBeenPurchased;
        canAddDebt = source.canAddDebt;
        canRemoveDebt = source.canRemoveDebt;

        // store source reference so we can write back when changes happen
        sourceRef = source;

        // refresh visuals
        UpdateUI();
    }

    private void UpdateUI()
    {
        // safe null checks
        if (buttonTextRef == null || itemNameTextRef == null)
        {
            if (buttonTextRef == null) Debug.LogWarning($"{name}: buttonTextRef is not assigned or found.");
            if (itemNameTextRef == null) Debug.LogWarning($"{name}: itemNameTextRef is not assigned or found.");
            return;
        }

        if (!canAddDebt && !canRemoveDebt)
        {
            if (!HasBeenPurchased)
            {
                buttonTextRef.text = $"Buy For: {CostOfItem:F0}";
                if (isObstacle)
                {
                    ObstacleManager.Instance.AddObstacleToInventory(obstacleID);
                }
            }
            else if (CurrentUpgradeLevel < NumOfUpgrades)
            {
                buttonTextRef.text = $"Upgrade ({CurrentUpgradeLevel}/{NumOfUpgrades}) - {CostOfItem:F0}";
                if (isObstacle)
                {
                    ObstacleManager.Instance.AddObstacleToInventory(obstacleID);
                }
            }
            else
            {
                buttonTextRef.text = "Max Level Reached";
            }
            itemNameTextRef.text = ItemName;
        }
        else
        {
            if (canAddDebt)
            {
                buttonTextRef.text = $"Borrow {CostOfItem:F0}";
                itemNameTextRef.text = "Borrow More Money";
            }
            else if (canRemoveDebt)
            {
                buttonTextRef.text = $"Pay {CostOfItem:F0}";
                itemNameTextRef.text = "Pay Back Debt";
            }
        }
    }

    private void OnBuyPressed()
    {
        bool ok = BuyItem();
        if (ok)
        {
            // push changes back to source so refreshing shop keeps the new values
            SyncBackToSource();

            UpdateUI();

        }
        else
        {
            // helpful log for debugging why buy failed
            Debug.Log($"{name}: BuyItem returned false. Check console for more info.");
        }
    }

    private void SyncBackToSource()
    {
        if (sourceRef == null) return;

        // copy the runtime values back to the source object so future re-instantiations reflect them
        sourceRef.ItemName = this.ItemName;
        sourceRef.CostOfItem = this.CostOfItem;
        sourceRef.PriceIncreaseFactor = this.PriceIncreaseFactor;
        sourceRef.NumOfUpgrades = this.NumOfUpgrades;
        sourceRef.CurrentUpgradeLevel = this.CurrentUpgradeLevel;
        sourceRef.HasBeenPurchased = this.HasBeenPurchased;
        sourceRef.canAddDebt = this.canAddDebt;
        sourceRef.canRemoveDebt = this.canRemoveDebt;
    }

    public bool BuyItem()
    {
        // check singleton existence up-front and log helpful messages
        if (ScoreManager.instance == null)
        {
            Debug.LogError($"{name}: ScoreManager.instance is null. Can't process purchases.");
            return false;
        }

        if (!canAddDebt && !canRemoveDebt)
        {
            if (!HasBeenPurchased)
            {
                if (ScoreManager.instance.currentMoney >= CostOfItem)
                {
                    uint amount = (uint)Mathf.RoundToInt(CostOfItem);
                    ScoreManager.instance.SpendMoney(amount);
                    HasBeenPurchased = true;

                    if (NumOfUpgrades > 0)
                        CostOfItem += CostOfItem * PriceIncreaseFactor;

                    Debug.Log($"{name}: Purchased for {amount}. New cost: {CostOfItem:F2}");
                    return true;
                }

                Debug.Log($"{name}: Not enough money to buy. Needed {CostOfItem:F2}, have {ScoreManager.instance.currentMoney:F2}");
                return false;
            }
            else
            {
                if (NumOfUpgrades > 0 && CurrentUpgradeLevel < NumOfUpgrades)
                {
                    if (ScoreManager.instance.currentMoney >= CostOfItem)
                    {
                        uint amount = (uint)Mathf.RoundToInt(CostOfItem);
                        ScoreManager.instance.SpendMoney(amount);
                        CurrentUpgradeLevel++;
                        CostOfItem += CostOfItem * PriceIncreaseFactor;
                        Debug.Log($"{name}: Upgraded to {CurrentUpgradeLevel}. Spent {amount}. New cost: {CostOfItem:F2}");
                        return true;
                    }

                    Debug.Log($"{name}: Not enough money to upgrade. Needed {CostOfItem:F2}, have {ScoreManager.instance.currentMoney:F2}");
                    return false;
                }

                Debug.Log($"{name}: Cannot upgrade - either no upgrades available or already at max.");
                return false;
            }
        }
        else
        {
            if (DebtSystem.instance == null)
            {
                Debug.LogError($"{name}: DebtSystem.instance is null. Can't process debt actions.");
                return false;
            }

            uint amount = (uint)Mathf.RoundToInt(CostOfItem);

            if (canAddDebt)
            {
                DebtSystem.instance.AddToDebt(amount);
                ScoreManager.instance.AddMoney(amount);
                Debug.Log($"{name}: Borrowed {amount} (debt increased). New Debt:{DebtSystem.instance.DebtRemaining:F2} and New Money Amout:{ScoreManager.instance.currentMoney:F2}");
                return true;
            }
            else if (canRemoveDebt)
            {
                if (ScoreManager.instance.currentMoney >= CostOfItem)
                {
                    DebtSystem.instance.RemoveFromDebt(amount);
                    ScoreManager.instance.SpendMoney(amount);
                    Debug.Log($"{name}: Paid back {amount} of debt.");
                    return true;
                }

                Debug.Log($"{name}: Not enough money to pay debt. Needed {CostOfItem:F2}, have {ScoreManager.instance.currentMoney:F2}");
                return false;
            }
            return false;
        }
    }

    // Expose ItemName so ShopUI can show it in the hierarchy if needed
    public string GetItemName() => ItemName;
}
