using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartInteractable : Interactable
{
    [SerializeField] private AudioClip waveStartAudio;
    private AudioSource audioSource;

    public override string GetName()
    {
        return "Inject Virus";
    }

    public override void OnInteracted()
    {
        ServiceLocator.instance.GetService<Spawner>().BeginSwarm();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;

            audioSource.clip = waveStartAudio;
        }

        if (waveStartAudio != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
