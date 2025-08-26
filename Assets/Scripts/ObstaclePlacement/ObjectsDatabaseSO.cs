using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectsData> objectsData;
}

[Serializable]
public class ObjectsData
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public int ID { get; private set; }

    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field : SerializeField] public GameObject Prefab { get; private set; }

    [field : SerializeField] public Image Icon { get; private set; }

    [Tooltip("List of rows this object is allowed on. Leave empty to allow all.")]
    public int[] allowedRows;

    [Tooltip("List of rows this object is NOT allowed on. Leave empty for none.")]
    public int[] blockedRows;

}


