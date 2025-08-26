using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObstacleDataSO : ScriptableObject
{
    public GridData gridData;
    public Dictionary<int, int> placeableObstacles = new Dictionary<int, int>();
}
