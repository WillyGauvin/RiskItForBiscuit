using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> placedObstacles = new();

    public void PlaceObject(GameObject prefab, Vector3Int gridPos, Vector3 worldPos)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceObject);

        GameObject obstacle = Instantiate(prefab, worldPos, prefab.transform.rotation);

        placedObstacles[gridPos] = obstacle;
    }

    public void RemoveObjectAt(Vector3Int gridPos)
    {
        if (!placedObstacles.TryGetValue(gridPos, out GameObject obj) || obj == null)
        {
            Debug.Log("No obstacle at grid position " + gridPos);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.build_PlaceError);
            return;
        }

        AudioManager.instance.PlayOneShot(FMODEvents.instance.build_RemoveBuilding);

        Debug.Log("Destroyed: " + obj.name);
        Destroy(obj);
        placedObstacles.Remove(gridPos);
    }
}
