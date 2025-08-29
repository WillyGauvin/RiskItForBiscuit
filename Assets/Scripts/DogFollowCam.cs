using Unity.Cinemachine;
using UnityEngine;

public class DogFollowCam : MonoBehaviour
{
    public Rigidbody2D target;                     // The moving object
    public CinemachineCamera vcam;
    public float baseFOV = 60f;
    public float maxFOV = 90f;
    public float maxSpeed = 20f;
    public float smoothSpeed = 5f;

    void Update()
    {
        float speed = target.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / maxSpeed);

        float targetFOV = Mathf.Lerp(baseFOV, maxFOV, t);
        vcam.Lens.FieldOfView = Mathf.Lerp(vcam.Lens.FieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}
