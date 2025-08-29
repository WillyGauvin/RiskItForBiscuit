using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedObstacles = new();

    public int PlaceObject(GameObject prefab, Vector3 pos)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceObject);

        GameObject obstacle = Instantiate(prefab);

        obstacle.transform.position = pos;

        placedObstacles.Add(obstacle);

        return placedObstacles.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedObstacles.Count <= gameObjectIndex || placedObstacles[gameObjectIndex] == null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceError);
            return;
        }
        AudioManager.instance.PlayOneShot(FMODEvents.instance.build_RemoveBuilding);

        Destroy(placedObstacles[gameObjectIndex]);
        placedObstacles[gameObjectIndex] = null;
    }
}
