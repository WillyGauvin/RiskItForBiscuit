using UnityEngine;

public class RemovingState : IBuildingState
{
    Grid grid;
    PreviewSystem previewSystem;
    GridData obstacleData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData obstacleData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.obstacleData = obstacleData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (obstacleData.CanPlaceObstacleAt(gridPosition, Vector2Int.one))
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceError);
            return;
        }

        // Get the placement data from ANY occupied cell
        if (!obstacleData.placedObjects.TryGetValue(gridPosition, out PlacementData data))
            return;

        int objectID = data.ID;

        // Return to inventory
        ObstacleManager.Instance.AddObstacleToInventory(objectID);

        // Remove from logical grid
        obstacleData.RemoveObjectAt(gridPosition);

        // Remove visual object – use the anchor (first cell)
        Vector3Int anchorCell = data.occupiedPositions[0];
        objectPlacer.RemoveObjectAt(anchorCell);

        // Update preview
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = !obstacleData.CanPlaceObstacleAt(gridPosition, Vector2Int.one);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
