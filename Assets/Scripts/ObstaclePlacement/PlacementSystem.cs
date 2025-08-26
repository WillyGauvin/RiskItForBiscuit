using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Cinemachine;
using UnityEngine.EventSystems;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private ObstacleInputManager inputManager;

    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO dataBase;

    [SerializeField] private GameObject gridVisualization;

    private GridData obstacleData;

    [SerializeField] private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private bool pendingClick = false;
    private Vector2 pendingClickPosition;

    private void Start()
    {
        StopPlacement();

        obstacleData = new GridData();
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
        if (inputManager.isPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);

        StopPlacement();
    }


    public void EnterBuildMode()
    {
        gridVisualization.SetActive(true);
    }

    public void ExitBuildMode()
    {
        gridVisualization.SetActive(false);
    }
}
