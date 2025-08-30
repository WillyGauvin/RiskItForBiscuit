using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class TutorialMask
{
    public Vector2 maskPosition;
    public Vector2 maskSize;
    public TextMeshProUGUI textBox;
}

public class ShopTutorial : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> tutorialSentences;
    [SerializeField] List<TutorialMask> tutorialMaskTransforms;

    int tutorialStep = 0;

    [SerializeField] Image image;
    [SerializeField] Image mask;

    bool tutorialActive = false;

    PlayerInput input;
    public void StartTutorial()
    {
        tutorialActive = true;
        input = GetComponent<PlayerInput>();
        image.enabled = true;
        mask.enabled = true;
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
                mask.transform.localPosition = tutorialMaskTransforms[tutorialStep].maskPosition;
                mask.transform.localScale = tutorialMaskTransforms[tutorialStep].maskSize;
            }
            else
            {
                input.SwitchCurrentActionMap("Menu");
                this.gameObject.SetActive(false);
            }
        }
    }
}
