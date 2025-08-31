using FMODUnity;
using UnityEngine;

public class FlyingFans : MonoBehaviour
{
    [Header("Hover")]
    [SerializeField] float maxHeight = 10.0f;
    [SerializeField] float speed = 1.0f;

    float startHeight;

    private StudioEventEmitter emitter;
    private void Start()
    {
        startHeight = transform.position.y;

        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.fan, this.gameObject);
        emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = startHeight - Mathf.Sin(Time.time * speed) * maxHeight;

        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }
}
