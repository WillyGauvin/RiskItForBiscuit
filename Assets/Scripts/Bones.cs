using UnityEngine;

public class Bones : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Quaternion rotation;

    void Awake()
    {
        rotation = transform.localRotation;
    }

    public void Reset2()
    {
        transform.localRotation = rotation;
    }
}
