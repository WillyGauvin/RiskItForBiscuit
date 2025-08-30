using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ScoreObjectPlacer : MonoBehaviour
{
    private static ScoreObjectPlacer _instance;
    public static ScoreObjectPlacer instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing one in the scene
                _instance = FindFirstObjectByType<ScoreObjectPlacer>();
            }
            return _instance;
        }
    }

    [SerializeField] private Grid grid;
    [SerializeField] private GameObject plane;
    [SerializeField] private ScoreObject scoreObjectPrefab;

    private int numberOfObjectsToPlace = 50;

    public void IncreaseNumberOfObject(float multiplier)
    {
        numberOfObjectsToPlace = (int)((float)numberOfObjectsToPlace * multiplier);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2Int gridSize = GetGridSize();

        List<Vector3> availableSpaces = new List<Vector3>();

        for (int y = 1; y < gridSize.y - 1; y += 3)
        {
            for (int x = 0; x < gridSize.x ; x++)
            {
                Vector3 newLocation = grid.gameObject.transform.position + new Vector3(grid.cellSize.x * x + grid.cellSize.x * 0.5f, grid.cellSize.y * y + grid.cellSize.y * 0.5f, 0.0f);
                availableSpaces.Add(newLocation);
            }
        }
        Debug.Log("Available Spaces:" + availableSpaces.Count);

        for (int i = 0; i < numberOfObjectsToPlace; i++)
        {
            int space = Random.Range(0, availableSpaces.Count);

            Vector3 position = availableSpaces[space];
            availableSpaces.RemoveAt(space);

            ScoreObject newObject = Instantiate(scoreObjectPrefab, position, Quaternion.identity);
            newObject.Init(5.0f, 1.0f);
        }
    }

    public Vector2Int GetGridSize()
    {
        // Get world size of plane (taking scale into account)
        Renderer renderer = plane.GetComponent<Renderer>();
        Vector3 worldSize = renderer.bounds.size;

        // Divide world size by grid cell size to get number of tiles
        Vector2 cellSize = new Vector2(grid.cellSize.x, grid.cellSize.y);

        int width = Mathf.RoundToInt(worldSize.x / cellSize.x);
        int height = Mathf.RoundToInt(worldSize.y / cellSize.y); // Z axis in 3D is "up" for plane

        return new Vector2Int(width, height);
    }
}
