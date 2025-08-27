using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public TMP_Text text;

    [SerializeField] float duration = 1.5f;
    [SerializeField] float distanceToMove = 10.0f;

    Vector3 startPos;
    Vector3 endPos;
    float timer = 0.0f;

    private Vector3 focusPosition;
    private Vector3 offset;
    private Vector3 endOffset;
    public bool disappear;

    void Update()
    {
        if (disappear)
        {
            // While active, update timer.
            timer += Time.deltaTime;

            float fadeTime = duration / 2.0f;

            if (timer > fadeTime)
            {
                text.color = Color.Lerp(text.color, Color.clear, (timer - fadeTime) / (duration - fadeTime));
            }

            // timer works as Time.deltaTime in these Lerps.
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.Sin(timer / duration));
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Mathf.Sin(timer / duration));
        }
        else
        {
            transform.position = GetOffsetPosition();
            startPos = transform.position;
            endPos = startPos + endOffset;
        }
    }

    Vector3 GetOffsetPosition()
    {
        Vector3 direction = (focusPosition - Vector3.up).normalized;
        Vector3 rightDirection = Vector3.Cross(Vector3.up, direction).normalized;
        return focusPosition + offset + rightDirection * 2f;
    }

    public void SetupIndicator(Vector3 spawnPosition, Collider collider)
    {
        // Direction the text will move towards. Clamped between -90 and 90 degrees to always have it moving upwards.
        float moveDir = UnityEngine.Random.rotation.eulerAngles.z;
        moveDir = Mathf.Clamp(moveDir, -90.0f, 90.0f);

        focusPosition = spawnPosition;
        offset = (Vector3.up * (collider.bounds.size.y + 1f));

        transform.position = GetOffsetPosition();
        startPos = transform.position;

        // Set an end position from the random distance and angle.
        endOffset = (Quaternion.Euler(0.0f, 0.0f, moveDir) * new Vector3(distanceToMove, distanceToMove, 0.0f));
        endPos = startPos + endOffset;
    }

    public void SetText(string textToDisplay)
    {
        text.text = textToDisplay;

        StartCoroutine(RemoveAfterDelay());
    }

    private IEnumerator RemoveAfterDelay()
    {
        yield return new WaitForSeconds(duration);

        disappear = true;

        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }
}
