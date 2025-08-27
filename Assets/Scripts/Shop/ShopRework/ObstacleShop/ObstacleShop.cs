using UnityEngine;

public class ObstacleShop : Shop
{
    [SerializeField] private ObstacleShopItems obstacleItemsAsset;
    private static ObstacleShopItems runtimeData;

    [SerializeField] private ObstacleItemUI shopItemPrefab;
    [SerializeField] private GameObject shopContent;

    private void Awake()
    {

        if (runtimeData == null)
        {
            // First time: make the clone
            runtimeData = Instantiate(obstacleItemsAsset);
        }

        LoadShop();
    }

    private void LoadShop()
    {
        foreach(ObstacleItem obstacle in runtimeData.obstacleItems)
        {
            ObstacleItemUI item = Instantiate(shopItemPrefab, shopContent.transform);
            item.Init(obstacle);
        }
    }
}
