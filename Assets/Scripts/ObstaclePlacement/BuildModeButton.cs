using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
public class BuildModeButton : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private CinemachineCamera buildCamera;

    bool isInBuildMode = false;

    public void ToggleBuildMode()
    {
        buildCamera.gameObject.SetActive((isInBuildMode) ? false : true);
        isInBuildMode = !isInBuildMode;
        UpdateButton();
    }

    private void UpdateButton()
    {
       
    }

}
