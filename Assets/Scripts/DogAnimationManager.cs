using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


public class DogAnimationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private List<Rigidbody2D> rbs;

    private List<Bones> bones;

    [SerializeField] Animator animator;

    void OnEnable()
    {
        rbs = GetComponentsInChildren<Rigidbody2D>().ToList();
        rbs.Add(GetComponent<Rigidbody2D>());

        bones = GetComponentsInChildren<Bones>().ToList();

        Reset();
    }

    public void ActivateRigidbody()
    {
        foreach (Rigidbody2D rb in rbs)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        animator.enabled = false;
    }

    public void Reset()
    {
        foreach (Bones bone in bones)
        {
            bone.Reset2();
        }
        foreach (Rigidbody2D rb in rbs)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        animator.enabled = true;
        MinigameManager.canPlayGame = true;
    }
}
