using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play(); // Oyun baþlarken çalsýn
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
