using UnityEngine;
using UnityEngine.UI;
public class Popup : MonoBehaviour
{
    public Animator popUp;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void Show()
    {
        if (popUp != null && popUp.runtimeAnimatorController != null)
        {
            popUp.SetTrigger("Start");
        }
        else
        {
            Debug.LogWarning("Animator or AnimatorController is missing on " + gameObject.name);
        }
    }
    public void Hide()
    {
        popUp.SetTrigger("End");
    }
}
