using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildTutorial : MonoBehaviour
{
    private static BuildTutorial _instance;

    public static BuildTutorial instance
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

    public Image mask;
    public Image overlay;
    public TextMeshProUGUI text;
    public List<GameMask> sentences;
    [SerializeField] PlayerInput input;

    int index;

    void OnEnable()
    {
        _instance = this;
    }

    public void StartTutorial()
    {
        ObstacleManager.buildTutorialNeeded = false;
        mask.enabled = true;
        overlay.enabled = true;
        text.enabled = true;
        input.SwitchCurrentActionMap("Tutorial");

        text.text = sentences[index].sentence;
        mask.transform.localScale = sentences[index].maskSize;
        mask.transform.localPosition = sentences[index].maskPosition;
    }

        public void OnClick()
    {
        if (index < sentences.Count - 1)
        {
            index += 1;
            text.text = sentences[index].sentence;
            mask.transform.localScale = sentences[index].maskSize;
            mask.transform.localPosition = sentences[index].maskPosition;
        }
        else
        {
            input.SwitchCurrentActionMap("Game");
            this.gameObject.SetActive(false);
        }
    }
}
