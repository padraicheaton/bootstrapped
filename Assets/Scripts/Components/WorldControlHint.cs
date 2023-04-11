using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControlHint : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;

    private float destAlpha;
    private Transform camTransform;

    private void Start()
    {
        camTransform = ServiceLocator.instance.GetService<PlayerCamera>().transform;

        destAlpha = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetVisibility(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetVisibility(true);
    }

    private void SetVisibility(bool shown)
    {
        destAlpha = shown ? 1f : 0f;
    }

    private void Update()
    {
        cg.alpha = Mathf.Lerp(cg.alpha, destAlpha, Time.deltaTime * 10f);

        cg.transform.LookAt(camTransform);
    }
}
