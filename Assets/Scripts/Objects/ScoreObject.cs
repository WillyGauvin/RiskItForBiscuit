using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int scoreToGive = 200;

    [Header("Hover")]
    private float maxHeight = 10.0f;
    private float speed = 1.0f;

    private float randomOffset = 0.0f;

    float startHeight;

    bool hasCollided = false;

    private void Start()
    {
        startHeight = transform.position.y;
        randomOffset = Random.Range(0, 7);
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = startHeight + Mathf.Sin(Time.time * speed + randomOffset) * maxHeight;

        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        startHeight = newPosition.y;
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.point_toeTouch);
    }

    public void Init(float height, float speed)
    {
        maxHeight = height;
        this.speed = speed;
    }
}
