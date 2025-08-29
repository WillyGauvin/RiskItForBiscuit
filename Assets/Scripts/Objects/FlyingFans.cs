using UnityEngine;

public class FlyingFans : MonoBehaviour
{
    [Header("Hover")]
    [SerializeField] float maxHeight = 10.0f;
    [SerializeField] float speed = 1.0f;

    float startHeight;

    private void Start()
    {
        startHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = startHeight + Mathf.Sin(Time.time * speed) * maxHeight;

        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }
}
