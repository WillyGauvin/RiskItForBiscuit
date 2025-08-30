using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class MinigameMask
{
    public Vector2 maskPosition;
    public Vector2 maskSize;
    public string sentence;
    public Sprite maskSprite;
}

public class MinigameTutorialManager : MonoBehaviour
{
    private static MinigameTutorialManager _instance;

    public static MinigameTutorialManager instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing one in the scene
                Debug.Log("Not active");
            }
            return _instance;
        }
    }

    [SerializeField] Image overlay;
    [SerializeField] Image mask;
    [SerializeField] PlayerInput input;
    [SerializeField] List<MinigameMask> sentences;
    [SerializeField] TextMeshProUGUI text;

    int index = 0;

    void OnEnable()
    {
        _instance = this;
    }

    public void OnClick()
    {
        if (index < sentences.Count - 1)
        {
            index += 1;
            text.text = sentences[index].sentence;
            mask.transform.localScale = sentences[index].maskSize;
            mask.transform.localPosition = sentences[index].maskPosition;
            mask.sprite = sentences[index].maskSprite;
        }
        else
        {
            input.SwitchCurrentActionMap("Minigame");
            this.gameObject.SetActive(false);
            MinigameManager.instance.RestartMinigame();
        }
    }

    public void StartTutorial()
    {
        mask.enabled = true;
        overlay.enabled = true;
        text.enabled = true;
        input.SwitchCurrentActionMap("Tutorial");

        text.text = sentences[index].sentence;
        mask.transform.localScale = sentences[index].maskSize;
        mask.transform.localPosition = sentences[index].maskPosition;
        mask.sprite = sentences[index].maskSprite;
    }
}
