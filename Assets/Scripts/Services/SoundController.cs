using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private List<SceneMusic> backgroundMusic;
    [SerializeField] private AudioClip swarmMusic;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource foregroundAudioSource;

    [Header("Settings")]
    [SerializeField] private float audioFadeSpeed;

    public void SwitchBackgroundMusic(LoadedScenes activeScene)
    {
        foreach (SceneMusic sm in backgroundMusic)
            if (sm.scene == activeScene)
            {
                StartCoroutine(FadeSwitchAudioClip(backgroundAudioSource, sm));
                break;
            }
    }

    public void SetSwarmBackgroundMusic(bool enabled)
    {
        foregroundAudioSource.clip = swarmMusic;

        foregroundAudioSource.Play();

        StartCoroutine(FadeAudioSource(foregroundAudioSource, enabled ? 1f : 0f));

        if (enabled)
            StartCoroutine(FadeAudioSource(backgroundAudioSource, 0f));
        else
            SwitchBackgroundMusic(ServiceLocator.instance.GetService<SceneController>().GetActiveScene());
    }

    private IEnumerator FadeAudioSource(AudioSource source, float destinationVolume)
    {
        while (Mathf.Abs(source.volume - destinationVolume) > 0.1f)
        {
            yield return null;

            source.volume = Mathf.Lerp(source.volume, destinationVolume, Time.deltaTime * audioFadeSpeed);
        }

        source.volume = destinationVolume;
    }

    private IEnumerator FadeSwitchAudioClip(AudioSource source, SceneMusic musicData)
    {
        yield return StartCoroutine(FadeAudioSource(source, 0f));

        source.clip = musicData.clip;
        source.Play();

        yield return StartCoroutine(FadeAudioSource(source, musicData.volume));
    }


    [System.Serializable]
    public struct SceneMusic
    {
        public AudioClip clip;
        public LoadedScenes scene;

        [Range(0f, 1f)]
        public float volume;
    }
}
