using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] public List<ShopTemplate> listOfItems;   // All shops
    [SerializeField] public ShopTemplate activeShop;          // Currently open shop
    [SerializeField] public GameObject itemListUI;            // Panel with GridLayoutGroup (assign the child container)
    [SerializeField] private GameObject itemCardPrefab;       // Prefab for UI item (drag in Inspector)
    [SerializeField] private TextMeshProUGUI shopNameRef;     // Name of the shop

    void Start()
    {
        gameObject.SetActive(false); // hide by default
    }

    //Called when a new shop is selected/clicked to be the current shop
    public void SetCurrentShop(ShopTemplate item)
    {
        if (item != activeShop)
        {
            //Close previous ui
            CloseShop();
            activeShop = item;

            //Make UI visible
            gameObject.SetActive(true);
            //Set The Shops Name
            if (shopNameRef != null)
                shopNameRef.text = item.ShopName;
            //Refresh the items shown
            UpdateShop();
        }
    }

    public void CloseShop()
    {
        activeShop = null;
        gameObject.SetActive(false); // hide UI
    }

    public void UpdateShop()
    {
        // Basic safety checks
        if (itemListUI == null)
        {
            Debug.LogError($"ShopUI ({name}): itemListUI is not assigned.");
            return;
        }

        // Prevent accidentally clearing the shop root itself
        if (itemListUI == this.gameObject)
        {
            Debug.LogError($"ShopUI ({name}): itemListUI points to the ShopUI root. Assign the child 'itemList' container instead. Aborting Clear.");
            return;
        }

        Transform container = itemListUI.transform;

        // Destroy direct children safely (iterate backwards to prevent error)
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            Destroy(child.gameObject);
        }

        // If no active shop, nothing more to do
        if (activeShop == null) return;

        // Populate with items from the active shop
        foreach (ItemTemplate item in activeShop.listOfItems)
        {
            GameObject card = Instantiate(itemCardPrefab, container, false); // 'false' keeps local transform
            ItemTemplate cardTemplate = card.GetComponent<ItemTemplate>();
            if (cardTemplate == null)
            {
                Debug.LogWarning("Item card prefab missing ItemTemplate component.");
                continue;
            }

            // Initialize the card with the source item's data and refresh its UI
            cardTemplate.InitializeFrom(item);

            // Update GameObject name for clarity in hierarchy
            card.name = $"Card - {item.GetItemName()}";
        }

        // Make layout update immediately so GridLayoutGroup arranges the new children now
        RectTransform rt = container as RectTransform;
        if (rt != null) UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
    }
}
