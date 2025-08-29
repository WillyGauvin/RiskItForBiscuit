using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MinigameFrisbee : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] RectTransform frisbeeTransform;

    Vector3 target;

    [SerializeField] float speed;

    [SerializeField] BoxCollider2D startingBox;
    [SerializeField] BoxCollider2D endingBox;

    [SerializeField] LayerMask mask;

    [SerializeField] GameObject exclamationPrefab;

    GameObject exclamation;

    Vector3 startPos;

    private void Awake()
    {
        startPos = frisbeeTransform.localPosition;
    }

    private void OnEnable() {
        frisbeeTransform.localPosition = startPos;
    }

    Vector3 FindPointInBox(BoxCollider2D box)
    {
        Bounds bounds = box.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(randomX, randomY, randomZ);
    }

    private float FindAngle(Vector3 angleTarget, Vector3 startPosition)
    {
        Vector2 direction = (angleTarget - startPosition).normalized;
        Vector2 fromDirection = frisbeeTransform.up;
        float angle = Vector2.Angle(fromDirection, direction);
        return angle;
    }

    IEnumerator ThrowFrisbee()
    {
        //yield return new WaitForSeconds(Random.Range(1f, 3f));
        yield return new WaitForSeconds(1.5f);
        exclamation.gameObject.SetActive(false);
        rb2d.AddForce(frisbeeTransform.right * speed, ForceMode2D.Impulse);
    }

    public void SetFrisbee()
    {
        StopAllCoroutines();
        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        frisbeeTransform.rotation = Quaternion.identity;

        if (exclamation != null)
        {
            Destroy(exclamation);
        }
        exclamation = null;

        frisbeeTransform.position = FindPointInBox(startingBox);

        target = FindPointInBox(endingBox);

        float angle = FindAngle(target, frisbeeTransform.position);
        frisbeeTransform.rotation = Quaternion.Euler(0, 0, angle + 90);

        RaycastHit2D hit = Physics2D.Raycast(frisbeeTransform.position, frisbeeTransform.right, 100f, mask);
        if (hit.point != Vector2.zero)
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.z -= 35f;
            exclamation = Instantiate(exclamationPrefab, spawnPosition, Quaternion.Euler(0, 0, angle - 90f));
            exclamation.transform.position -= exclamation.transform.right;
        }

        //Throw frisbee
        StartCoroutine(ThrowFrisbee());
    }
}
