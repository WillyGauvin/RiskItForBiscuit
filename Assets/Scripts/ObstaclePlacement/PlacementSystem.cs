using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [SerializeField] private ObstacleInputManager inputManager;

    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO dataBase;

    [SerializeField] private GameObject gridVisualization;

    public GridData obstacleData;

    [SerializeField] private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private bool pendingClick = false;
    private Vector2 pendingClickPosition;

    private void Start()
    {
        StopPlacement();
        LoadObstacles();
    }

    private void Update()
    {
        if (pendingClick)
        {
            pendingClick = false;
            PlaceStructure();
        }

        if (buildingState == null) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        buildingState = new PlacementState(ID, grid, preview, dataBase, obstacleData, objectPlacer);

        PlayerController.instance.OnClicked += QuePlacement;
        PlayerController.instance.OnExit += StopPlacement;
    }


    private void StopPlacement()
    {
        if(buildingState == null) return;

        buildingState.EndState();

        PlayerController.instance.OnClicked -= QuePlacement;
        PlayerController.instance.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
    public void StartRemoving()
    {

        StopPlacement();
        buildingState = new RemovingState(grid, preview, obstacleData, objectPlacer);

        PlayerController.instance.OnClicked += QuePlacement;
        PlayerController.instance.OnExit += StopPlacement;

    }

    //Queing the placement to delay for one frame because inputManager.isPointerOverUI() queries from the previous frame, not the one we are on.
    private void QuePlacement()
    {
        pendingClick = true;
    }

    private void PlaceStructure()
    {
        if (inputManager.isPointerOverUI())
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceError);
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);

        if (buildingState is PlacementState)
        {
            StopPlacement();
        }
    }


    public void EnterBuildMode()
    {
        gridVisualization.SetActive(true);
    }

    public void ExitBuildMode()
    {
        gridVisualization.SetActive(false);
        StopPlacement();
    }

    private void LoadObstacles()
    {
        GridData tempObstacleData = new();
        if (obstacleData.placedObjects.Count > 0)
        {
            foreach(KeyValuePair<Vector3Int,PlacementData> placedObject in obstacleData.placedObjects)
            {
                int index = dataBase.objectsData.FindIndex(data => data.ID == placedObject.Value.ID);
                if (tempObstacleData.CanPlaceObstacleAt(placedObject.Key, dataBase.objectsData[index].Size))
                {
                    int ObjectIndex = objectPlacer.PlaceObject(dataBase.objectsData[index].Prefab, grid.CellToWorld(placedObject.Key));
                    tempObstacleData.AddObjectAt(placedObject.Key, dataBase.objectsData[index].Size, placedObject.Value.ID, ObjectIndex);
                }
            }
        }
    }
}
