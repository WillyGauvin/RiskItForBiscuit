using DG.Tweening;
using FMODUnity;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingFans : MonoBehaviour
{
    [SerializeField] GameObject fanBlade;

    private StudioEventEmitter emitter;
    private void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.fan, this.gameObject);
        emitter.Play();

        fanBlade.transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear); // Uses degreesPerSecond instead of duration
    }
}
