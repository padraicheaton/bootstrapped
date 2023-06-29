using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class BtnAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip onClickClip;
    private float universalUIVolume = 0.5f;

    private Button btnComponent;
    private EventTrigger eventTrigger;
    private EventTrigger.Entry onPointerDownEvent;
    private AudioSource audioSource;

    private void Start()
    {
        btnComponent = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        eventTrigger = gameObject.AddComponent<EventTrigger>();

        onPointerDownEvent = new EventTrigger.Entry();
        onPointerDownEvent.eventID = EventTriggerType.PointerDown;
        onPointerDownEvent.callback.AddListener((e) =>
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        });
        eventTrigger.triggers.Add(onPointerDownEvent);

        audioSource.clip = onClickClip;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = universalUIVolume;
    }
}
