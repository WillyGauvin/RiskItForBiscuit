using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int scoreToGive = 250;

    [Header("Hover")]
    [SerializeField] float maxHeight = 10.0f;
    [SerializeField] float speed = 1.0f;

    float startHeight;

    bool hasCollided = false;

    private void Start()
    {
        startHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = startHeight + Mathf.Sin(Time.time * speed) * maxHeight;

        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hasCollided) { return; }
            hasCollided = true;

            AwardScore();

            Destroy(gameObject);
        }
    }

    void AwardScore()
    {
        ScoreManager.instance.AddToScore((uint)scoreToGive);
    }
}
