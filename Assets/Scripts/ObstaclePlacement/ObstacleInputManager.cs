using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObstacleInputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField] private LayerMask placementLayerMask;

    public bool isPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }



    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = PlayerController.instance.mousePosition;

        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 300, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    


}
