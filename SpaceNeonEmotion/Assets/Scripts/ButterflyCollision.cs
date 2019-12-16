using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyCollision : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Butterfly"))
        {
            if (!audioSource.isPlaying)
                SoundManager.instance.PlaySound(audioSource, other.gameObject, false, 0);
        }
    }
}
