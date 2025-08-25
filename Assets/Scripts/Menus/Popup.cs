using UnityEngine;
using UnityEngine.UI;
public class Popup : MonoBehaviour
{
    public Animator popUp;

    public void Show()
    {
        popUp.SetTrigger("Start");
    }
    public void Hide()
    {
        popUp.SetTrigger("End");
    }
}
