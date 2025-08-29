using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopTutorial : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> tutorialSentences;

    int tutorialStep = 0;

    [SerializeField] Image image;

    bool tutorialActive = false;

    PlayerInput input;
    public void StartTutorial()
    {
        tutorialActive = true;
        input = GetComponent<PlayerInput>();
        image.enabled = true;
        input.SwitchCurrentActionMap("Tutorial");
        tutorialSentences[tutorialStep].enabled = true;
    }

    public void OnClick()
    {
        if (tutorialActive)
        {
            if (tutorialStep < tutorialSentences.Count - 1)
            {
                tutorialSentences[tutorialStep].enabled = false;
                tutorialStep += 1;
                tutorialSentences[tutorialStep].enabled = true;
            }
            else
            {
                input.SwitchCurrentActionMap("Menu");
                this.gameObject.SetActive(false);
            }
        }
    }
}
