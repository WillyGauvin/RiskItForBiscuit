using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopTemplate : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ShopUI shopUIRef;
    [SerializeField] public string ShopName;
    [SerializeField] public List<ItemTemplate> listOfItems;

    void Start()
    {
        if (shopUIRef == null)
        {
            Debug.LogError("Add shopUI to Scene");
        }
        else
        {            //add this shop to the shopUI's list of shops
            shopUIRef.listOfItems.Add(this);
        }
    }

    //open the shop UI using this shop's data
    public void OpenShop()
    {
        if (shopUIRef != null)
        {
            shopUIRef.gameObject.SetActive(true); //  Show UI
            shopUIRef.SetCurrentShop(this);
        }
    }

    //Close the shop UI
    public void CloseShop()
    {
        if (shopUIRef != null)
        {
            shopUIRef.CloseShop();
        }
    }

    //ull never guess what "OnClick" does
    public void OnPointerClick(PointerEventData eventData)
    {
        //if the shop is clicked while its menu is open, close shop
        if (shopUIRef.activeShop == this)
            CloseShop();
        //  Opens the shop when clicked
        else
            OpenShop();
    }
}
