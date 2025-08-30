using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.EventSystems;
public class BuildModeButton : MonoBehaviour
{
    public static BuildModeButton instance { get; private set; }

    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private CinemachineCamera buildCamera;
    [SerializeField] private GameObject obstacles;

    public bool isInBuildMode = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.build_Open);
    }

    public void DisableBuildModeButton()
    {
        buildCamera.gameObject.SetActive(false);
        isInBuildMode = false;
        placementSystem.ExitBuildMode();
        button.interactable = false;
    }

    public void EnableBuildModeButton()
    {
        button.interactable = true;
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
