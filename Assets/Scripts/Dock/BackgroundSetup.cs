using UnityEngine;

public class BackgroundSetup : MonoBehaviour
{
    public Vector3 direction = Vector3.right;

    [Header("House")]
    public GameObject housePrefab;
    public Transform houseParent;
    public int houseCount;

    [Header("Stands")]
    public GameObject standPrefab;
    public Transform standParent;
    public int standCount;

    [Header("Fans")]
    public GameObject fanPrefab;
    public Transform fanParent;
    public int fanCount;


    public void Start()
    {
        Spawn(housePrefab, houseParent, houseCount);
        Spawn(standPrefab, standParent, standCount);
        Spawn(fanPrefab, fanParent, fanCount);
    }

    private void Spawn(GameObject prefab, Transform parent, int count)
    {
        if (prefab == null) return;
        if (parent == null) parent = transform;

        // First, spawn one plane to measure its scaled size
        GameObject firstPlane = Instantiate(prefab, parent.position, prefab.transform.rotation, parent);
        float step = firstPlane.GetComponentInChildren<Renderer>().bounds.size.x; // width with scale
        Vector3 dir = direction.normalized;

        // Spawn the rest
        for (int i = 1; i < count; i++)
        {
            Vector3 pos = parent.position + dir * step * i;
            Instantiate(prefab, pos, prefab.transform.rotation, parent);
        }
    }
}
