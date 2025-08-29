using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ObstacleButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI countText;

    private float count;
    public int ID;

    public void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    public void Init(int ID, Sprite buttonImage, float count)
    {
        button.image.sprite = buttonImage;
        this.count = count;
        this.ID = ID;

        button.onClick.AddListener(OnButtonClick);
        UpdateUI();
    }

    public void ChangeCount(int newCount)
    {
        count = newCount;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        countText.text = count.ToString();

        button.interactable = (count > 0) ? true : false;
    }

    private void OnButtonClick()
    {
        PlacementSystem.Instance.StartPlacement(ID);
    }




}
