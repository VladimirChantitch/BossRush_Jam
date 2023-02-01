using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();  
    }

    private void Explode()
    {
        particleSystem.Play();
        audioSource.PlayOneShot(audioSource.clip);
    }
}
