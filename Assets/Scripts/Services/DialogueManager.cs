using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // This script will be used for flavour, teaching the player and advice

    [Header("References")]
    [SerializeField] private CanvasGroup dialogueBoxCG;
    [SerializeField] private TextMeshProUGUI dialogueTxt;
    [SerializeField] private AudioSource dialogueVoiceLineSrc;

    [Header("Settings")]
    [SerializeField] private float secondaryDelay;

    private void Start()
    {
        dialogueBoxCG.alpha = 0f;
    }

    public void DisplayDialogue(string text, AudioClip clip = null)
    {
        StopAllCoroutines();

        StartCoroutine(DisplayDialogueRoutine(text, clip));
    }

    private IEnumerator DisplayDialogueRoutine(string text, AudioClip clip)
    {
        dialogueTxt.text = "";

        // Fade in CG
        while (dialogueBoxCG.alpha < 1f)
        {
            dialogueBoxCG.alpha += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Set text
        StartCoroutine(TextTypewriter(text));

        // Set and play audio clip
        if (clip)
        {
            dialogueVoiceLineSrc.clip = clip;
            dialogueVoiceLineSrc.Play();
        }

        // Delay whilst either playing or for set duration based on text
        if (clip)
            yield return new WaitWhile(() => dialogueVoiceLineSrc.isPlaying);
        else
        {
            int wordCount = text.Split(" ").Length;

            // As it takes roughly 1/2 a second to say a word, delay would equal word count * .5

            float delay = (float)wordCount * 0.6f;

            yield return new WaitForSeconds(delay);
        }

        // Secondary delay
        yield return new WaitForSeconds(secondaryDelay);

        // Fade out CG
        while (dialogueBoxCG.alpha > 0f)
        {
            dialogueBoxCG.alpha -= Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator TextTypewriter(string text)
    {
        dialogueTxt.text = "";

        while (dialogueTxt.text.Length < text.Length)
        {
            dialogueTxt.text += text[dialogueTxt.text.Length];

            yield return new WaitForSeconds(0.01f);
        }
    }
}
