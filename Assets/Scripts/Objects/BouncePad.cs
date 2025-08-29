using System.Collections;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    #region Member Variables

    [Header("Bounce Properties")]
    [SerializeField] float launchPower = 4.0f;
    [SerializeField] Vector2 launchDirection = Vector2.up;
    [SerializeField, Range(0, 100)] float cooldownTime = 1.0f;
    [SerializeField] bool isBouncing = false;

    [Header("Object Types")]
    [SerializeField] bool cancelXVelocity = false;
    [SerializeField] bool cancelYVelocity = true;

    #endregion

    private void Start()
    {
        launchDirection.Normalize();
    }

    #region OnTrigger and Launch Function

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isBouncing)
            {
                StartCoroutine(Launch());
            }
        }
    }

    Vector3 force = Vector3.zero;
    IEnumerator Launch()
    {
        isBouncing = true;

        var dog = GameManager.instance.Player.GetComponent<Dog>();
        if (dog == null) { Debug.LogError("No dog found!"); yield break; }

        dog.Body.linearVelocity = new Vector2(
            cancelXVelocity ? 0.0f : dog.Body.linearVelocity.x,
            cancelYVelocity ? 0.0f : dog.Body.linearVelocity.y);

        force = launchDirection * launchPower;

        dog.Body.AddForce(force, ForceMode2D.Impulse);

        AudioManager.instance.PlayOneShot(FMODEvents.instance.bouncePad);

        yield return new WaitForSeconds(cooldownTime);

        isBouncing = false;
    }

    #endregion
}
