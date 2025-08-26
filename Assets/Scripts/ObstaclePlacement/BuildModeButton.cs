using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
public class BuildModeButton : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private CinemachineCamera buildCamera;
    [SerializeField] private GameObject obstacles;

    bool isInBuildMode = false;

    private void Start()
    {
        UpdateBuildMode();
    }
    public void ToggleBuildMode()
    {
        buildCamera.gameObject.SetActive((isInBuildMode) ? false : true);
        isInBuildMode = !isInBuildMode;
        UpdateBuildMode();
        EventSystem.current.SetSelectedGameObject(null);

    }

    private void UpdateBuildMode()
    {
        ColorBlock colorBlock = new ColorBlock();
        colorBlock.normalColor = (isInBuildMode) ? Color.green : Color.red;
        colorBlock.colorMultiplier = 1.0f;
        colorBlock.fadeDuration = 0.1f;

        button.colors = colorBlock;

        buttonText.color = (isInBuildMode) ? Color.white : Color.white;
        obstacles.SetActive((isInBuildMode) ? true : false);

        if (isInBuildMode)
        {
            placementSystem.EnterBuildMode();
        }
        else
        {
            placementSystem.ExitBuildMode();
        }
    }

}
