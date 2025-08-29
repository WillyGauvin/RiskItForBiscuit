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
            if (hasCollided) { return; }

            StartCoroutine(Launch());
        }
    }

    Vector3 force = Vector3.zero;
    IEnumerator Launch()
    {
        hasCollided = true;

        var dog = GameManager.instance.Player.GetComponent<Dog>();
        if (dog == null) { Debug.LogError("No dog found!"); yield break; }

        dog.Body.linearVelocity = new Vector2(0.0f, 0.0f);

        force = Vector2.up * launchPower;

        dog.Body.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(fireCooldown);

        hasCollided = false;
    }
}
