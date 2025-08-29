using System.Collections;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    enum HoopType
    {
        Regular,
        Flame
    }

    bool hasCollided = false;

    [SerializeField] HoopType typeOfHoop = HoopType.Regular;

    [SerializeField] float hoopCooldown = 2.0f;

    [SerializeField] Renderer hoopRenderer;
    [SerializeField] Color triggered;
    Color baseColor;

    private void Start()
    {
        if (hoopRenderer) { baseColor = hoopRenderer.material.color; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasCollided) { return; }

            StartCoroutine(TriggerHoop());
        }
    }

    IEnumerator TriggerHoop()
    {
        hasCollided = true;

        if (hoopRenderer) { hoopRenderer.material.color = triggered; }

        switch (typeOfHoop)
        {
            case HoopType.Flame:
                ScoreManager.instance.FlameHoopPerformed();
                break;

            case HoopType.Regular:
            default:
                ScoreManager.instance.HoopPerformed();
                break;
        }

        yield return new WaitForSeconds(hoopCooldown);

        if (hoopRenderer) { hoopRenderer.material.color = baseColor; }

        hasCollided = false;
    }
}
