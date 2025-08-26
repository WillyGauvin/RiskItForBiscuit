using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
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
        if(obstacleData.CanPlaceObstacleAt(gridPosition, Vector2Int.one))
        {
            return;
        }

        gameObjectIndex = obstacleData.GetRepresentationIndex(gridPosition);
        if (gameObjectIndex == -1) return;

        obstacleData.RemoveObjectAt(gridPosition);
        objectPlacer.RemoveObjectAt(gameObjectIndex);

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = !obstacleData.CanPlaceObstacleAt(gridPosition, Vector2Int.one);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
