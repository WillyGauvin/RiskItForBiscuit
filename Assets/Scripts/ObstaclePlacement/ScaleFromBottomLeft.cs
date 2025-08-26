using UnityEngine;

public class ScaleFromBottomLeft : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform plane;

    public void SetScale(Vector2 scale)
    {
        // Apply scale (Unity plane is 10x10 by default, so adjust if needed)
        plane.localScale = new Vector3(scale.x, 1, scale.y);

        // Because pivot is center, we move it by half the scale in world units
        Vector3 offset = new Vector3(scale.x * grid.cellSize.x / 2f, 0, scale.y * grid.cellSize.y / 2f);
        plane.localPosition = offset;
    }
}
