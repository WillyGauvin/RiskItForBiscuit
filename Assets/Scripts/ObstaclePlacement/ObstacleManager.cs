using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance { get; private set; }

    [SerializeField] private ObstacleDataSO obstacleDataAsset;
    private static ObstacleDataSO runtimeData;

    [SerializeField] private ObjectsDatabaseSO objectDatabase;

    [SerializeField] private PlacementSystem placementSystem;

    public event Action<int, int> OnInventoryChanged;

    public static bool buildTutorialNeeded = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (runtimeData == null)
        {
            // First time: make the clone
            runtimeData = Instantiate(obstacleDataAsset);
            runtimeData.gridData = new GridData();

            foreach(ObjectsData data in objectDatabase.objectsData)
            {
                runtimeData.placeableObstacles.Add(data.ID, 1);
            }
        }
        if (placementSystem != null)
        {
            placementSystem.obstacleData = runtimeData.gridData;
        }
    }

    public ObstacleDataSO Data => runtimeData;

    public void AddObstacleToInventory(int ID)
    {
        runtimeData.placeableObstacles[ID]++;
        OnInventoryChanged?.Invoke(ID, runtimeData.placeableObstacles[ID]);
    }

    public void RemoveObstacleFromInventory(int ID)
    {
        if (runtimeData.placeableObstacles[ID] <= 0)
        {
            Debug.LogError($"Already 0 or less obstacles of ID {ID}");
        }
        else
        {
            runtimeData.placeableObstacles[ID]--;
            OnInventoryChanged?.Invoke(ID, runtimeData.placeableObstacles[ID]);
        }
    }

    public Dictionary<int, int> GetInventoryData()
    {
        return runtimeData.placeableObstacles;
    }



}
