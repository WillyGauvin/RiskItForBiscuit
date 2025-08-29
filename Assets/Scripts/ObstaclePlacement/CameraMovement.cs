using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 50f;
    public Vector2 edgeSize = new Vector2(500, 200); // pixels from screen edge to trigger

    [Header("Constraints")]
    public Vector2 xLimits = new Vector2(-50f, 50f);
    public Vector2 yLimits = new Vector2(-50f, 50f);

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 pos = transform.position;
        Vector3 mousePos = PlayerController.instance.mousePosition;

        // Edge scrolling checks
        if (mousePos.x >= Screen.width - edgeSize.x)
            pos.x += scrollSpeed * Time.deltaTime;
        else if (mousePos.x <= edgeSize.x)
            pos.x -= scrollSpeed * Time.deltaTime;

        if (mousePos.y >= Screen.height - edgeSize.y)
        {
            pos.y += scrollSpeed * Time.deltaTime;
        }
        
        else if (mousePos.y <= edgeSize.y)
        {
            pos.y -= scrollSpeed * Time.deltaTime;
        }

        // Clamp position
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.y = Mathf.Clamp(pos.y, yLimits.x, yLimits.y);

        transform.position = pos;
    }
}
