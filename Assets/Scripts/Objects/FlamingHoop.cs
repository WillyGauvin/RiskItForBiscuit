using System.Collections;
using UnityEngine;

public class FlamingHoop : MonoBehaviour
{
    bool hasCollided = false;

    [SerializeField] float launchPower = 20.0f;
    [SerializeField] float fireCooldown = 10.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasCollided) return;

            hasCollided = true;

            ScoreManager.instance.FlameHoopHit();
        }
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(4.0f);
        hasCollided = false;
    }
}
