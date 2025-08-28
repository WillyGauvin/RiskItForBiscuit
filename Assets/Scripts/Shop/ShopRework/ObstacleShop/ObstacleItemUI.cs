using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ObstacleItemUI : MonoBehaviour
{
    [SerializeField] Button purchaseButton;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI obstacleName;

    [SerializeField] ObjectsDatabaseSO obstacleDataBase;

    private ObstacleItem myItem;

    private int databaseIndex;

    public void Init(ObstacleItem obstacleItem)
    {
        myItem = obstacleItem;

        databaseIndex = obstacleDataBase.objectsData.FindIndex(data => data.ID == myItem.ItemID);

        purchaseButton.onClick.AddListener(OnButtonClick);

        icon = obstacleDataBase.objectsData[databaseIndex].Icon;
        obstacleName.text = obstacleDataBase.objectsData[databaseIndex].Name;

        UpdateUI();
    }
    public void OnDestroy()
    {
        purchaseButton.onClick.RemoveAllListeners();
    }

    private void OnButtonClick()
    {
        //Check if we can purchase item
        if (ScoreManager.instance.currentMoney < myItem.price) { Debug.Log("Not Enough Cash."); return; }

        //Add to inventory
        ObstacleManager.Instance.AddObstacleToInventory(myItem.ItemID);

        //Call currency manager to remove price of item
        ScoreManager.instance.SpendMoney((uint)myItem.price);

        //Increase price of item
        myItem.IncreasePrice();

        //UpdateUI
        UpdateUI();
    }

    private void UpdateUI()
    {
        //Check if we can afford this item

        price.text = myItem.price.ToString();
    }
}
