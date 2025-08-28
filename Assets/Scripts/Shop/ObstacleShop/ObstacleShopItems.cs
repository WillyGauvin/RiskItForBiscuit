using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu]
public class ObstacleShopItems : ScriptableObject
{
    public List<ObstacleItem> obstacleItems;
}

[Serializable]
public class ObstacleItem
{
    [field: SerializeField] public int ItemID { get; private set; }

    [field: SerializeField] public float price { get; private set; }

    [Tooltip("Percentage price will increase by. 0 meaning 0% increase 1 meaning 100% increase")]
    [field: SerializeField] public float priceIncreasePerPurchase { get; private set; }

    public void IncreasePrice()
    {
        price = price + price * priceIncreasePerPurchase;
    }
}
