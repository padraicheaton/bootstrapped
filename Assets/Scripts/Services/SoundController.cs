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
        StopAllCoroutines();

        StartCoroutine(FadeAudioSource(foregroundAudioSource, enabled ? GetMaxVolumeMultiplier() : 0f));

        if (enabled)
        {
            foregroundAudioSource.clip = swarmMusic;

            foregroundAudioSource.Play();

            StartCoroutine(FadeAudioSource(backgroundAudioSource, 0f));
        }
        else
            SwitchBackgroundMusic(ServiceLocator.instance.GetService<SceneController>().GetActiveScene());
    }

    private IEnumerator FadeAudioSource(AudioSource source, float destinationVolume)
    {
        while (Mathf.Abs(source.volume - destinationVolume) > 0.01f)
        {
            yield return null;

            source.volume = Mathf.Lerp(source.volume, destinationVolume, Time.unscaledDeltaTime * audioFadeSpeed);
        }

        source.volume = destinationVolume;
    }

    private IEnumerator FadeSwitchAudioClip(AudioSource source, SceneMusic musicData)
    {
        yield return StartCoroutine(FadeAudioSource(source, 0f));

        source.clip = musicData.clip;
        source.Play();

        yield return StartCoroutine(FadeAudioSource(source, musicData.volume * GetMaxVolumeMultiplier()));
    }

    private float GetMaxVolumeMultiplier()
    {
        return SaveStateController.GetData<float>(SaveStateController.masterVolumeValueKey) / 100f;
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
