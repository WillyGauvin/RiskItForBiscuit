using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData obstacleData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData obstacleData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.obstacleData = obstacleData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!CheckPlacementValidity(gridPosition, selectedObjectIndex))
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceError);
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        obstacleData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);

        ObstacleManager.Instance.RemoveObstacleFromInventory(database.objectsData[selectedObjectIndex].ID);
    }

    private bool CheckPlacementValidity(Vector3Int pos, int objectIndex)
    {
        return 
            (
            obstacleData.CanPlaceObstacleAt(pos, database.objectsData[selectedObjectIndex].Size) 
            &&
            CanPlaceOnRow(pos.z)
            );
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

    public bool CanPlaceOnRow(int row)
    {
        // If there are explicit allowed rows, only those are valid
        if (database.objectsData[selectedObjectIndex].allowedRows != null && database.objectsData[selectedObjectIndex].allowedRows.Length > 0)
        {
            foreach (int r in database.objectsData[selectedObjectIndex].allowedRows)
                if (r == row) return true;
            return false;
        }

        // Otherwise, block if in the blocked list
        if (database.objectsData[selectedObjectIndex].blockedRows != null && database.objectsData[selectedObjectIndex].blockedRows.Length > 0)
        {
            foreach (int r in database.objectsData[selectedObjectIndex].blockedRows)
                if (r == row) return false;
        }

        // Default: valid
        return true;
    }
}
