using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BoughtItemStart : MonoBehaviour
{

    private static BoughtItemStart _instance;

    public static BoughtItemStart instance
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
        // if (boughItemDone && didTutorial == false)
        // {
        //     didTutorial = true;
        //     StartTutorial();
        // }
    }

    public void StartTutorial()
    {
        mask.enabled = true;
        overlay.enabled = true;
        text.enabled = true;
        input.SwitchCurrentActionMap("Tutorial");
        Dog.instance.isInTutorial = true;
        LevelLoader.tutorialActive = false;

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
            Dog.instance.isInTutorial = false;
            input.SwitchCurrentActionMap("Game");
            this.gameObject.SetActive(false);
        }
    }
}
