using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private VoiceLine voiceLine;
    [SerializeField] private bool destroyAfterTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ServiceLocator.instance.GetService<DialogueManager>().DisplayDialogue(voiceLine.text, voiceLine.clip);

        if (destroyAfterTrigger)
            Destroy(gameObject);
    }
}
