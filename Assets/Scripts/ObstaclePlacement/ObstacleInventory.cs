using UnityEngine;
using System.Collections.Generic;
public class ObstacleInventory : MonoBehaviour
{
    [SerializeField] ObjectsDatabaseSO objectDatabase;

    [SerializeField] GameObject ButtonParent;
    [SerializeField] ObstacleButton ButtonPrefab;

    private List<ObstacleButton> obstacleButtons = new List<ObstacleButton>();

    private void Start()
    {
        InitializeList(ObstacleManager.Instance.GetInventoryData());

        ObstacleManager.Instance.OnInventoryChanged += InventoryChanged;
    }

    private void OnDestroy()
    {
        if (ObstacleManager.Instance != null)
        {
            ObstacleManager.Instance.OnInventoryChanged -= InventoryChanged;
        }
    }

    void InitializeList(Dictionary<int, int> inventory)
    {
        foreach (KeyValuePair<int, int> obstacle in inventory)
        {
            int index = objectDatabase.objectsData.FindIndex(data => data.ID == obstacle.Key);
            ObstacleButton button = Instantiate(ButtonPrefab, ButtonParent.transform);
            button.Init(obstacle.Key, objectDatabase.objectsData[index].Icon, obstacle.Value);

            obstacleButtons.Add(button);
        }
    }

    public void InventoryChanged(int ID, int newCount)
    {
        foreach(ObstacleButton button in obstacleButtons)
        {
            if (button.ID == ID)
            {
                button.ChangeCount(newCount);
            }
        }
    }




}
