using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationBlink : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(.4f);
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.enabled = !sprite.enabled;
            }
        }
        

    }
}
