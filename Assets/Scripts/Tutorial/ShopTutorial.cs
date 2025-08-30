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
    public float maskRotation;
}

public class ShopTutorial : MonoBehaviour
{
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
        //tutorialSentences[tutorialStep].enabled = true;
        tutorialMaskTransforms[tutorialStep].textBox.enabled = true;
    }

    public void OnClick()
    {
        if (tutorialActive)
        {
            if (tutorialStep < tutorialMaskTransforms.Count - 1)
            {
                tutorialMaskTransforms[tutorialStep].textBox.enabled = false;
                //tutorialSentences[tutorialStep].enabled = false;
                tutorialStep += 1;
                //tutorialSentences[tutorialStep].enabled = true;
                tutorialMaskTransforms[tutorialStep].textBox.enabled = true;
                mask.transform.localPosition = tutorialMaskTransforms[tutorialStep].maskPosition;
                mask.transform.localScale = tutorialMaskTransforms[tutorialStep].maskSize;
                mask.transform.localRotation = Quaternion.Euler(0, 0, tutorialMaskTransforms[tutorialStep].maskRotation);
            }
            else
            {
                input.SwitchCurrentActionMap("Menu");
                this.gameObject.SetActive(false);
            }
        }
    }
}
