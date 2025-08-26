using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Runtime.ExceptionServices;
using System;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePostions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach (Vector3Int pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new System.Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePostions(Vector3Int gridPos, Vector2Int objectSize)
    {
        List<Vector3Int> returnValues = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnValues.Add(gridPos + new Vector3Int(x, 0, y));
            }
        }
        return returnValues;
    }

    public bool CanPlaceObstacleAt(Vector3Int gridPos, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePostions(gridPos, objectSize);
        foreach(Vector3Int pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos)) return false;
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if(!placedObjects.ContainsKey(gridPosition))
        {
            return -1;
        }
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach(Vector3Int pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
