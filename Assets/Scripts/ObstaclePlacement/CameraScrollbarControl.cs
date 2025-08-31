using UnityEngine;
using UnityEngine.UI;

public class CameraScrollbarControl : MonoBehaviour
{
    [SerializeField] private Scrollbar horizontalScrollbar;
    [SerializeField] private Transform objectToMove;

    [Header("Camera Constraints")]
    public Vector2 xLimits = new Vector2(-50f, 50f);

    [Header("Smooth Settings")]
    public float smoothSpeed = 5f;

    private float targetX;

    void Start()
    {
        if (horizontalScrollbar != null)
        {
            // Initialize scrollbar value based on object's current position
            targetX = objectToMove.position.x;
            horizontalScrollbar.value = Mathf.InverseLerp(xLimits.x, xLimits.y, targetX);

            // Add listener
            horizontalScrollbar.onValueChanged.AddListener(OnScrollbarChanged);
        }
    }

    void Update()
    {
        // Smoothly move object to target X
        Vector3 pos = objectToMove.position;
        pos.x = Mathf.Lerp(pos.x, targetX, smoothSpeed * Time.deltaTime);
        objectToMove.position = pos;
    }

    void OnScrollbarChanged(float value)
    {
        // Map normalized scrollbar value (0-1) to X limits
        targetX = Mathf.Lerp(xLimits.x, xLimits.y, value);
    }
}
