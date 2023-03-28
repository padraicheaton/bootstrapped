using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPopup : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 rayCastPos;

    private bool isShown;
    private CanvasGroup cg;
    private TextMeshProUGUI text;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        cameraTransform = ServiceLocator.instance.GetService<PlayerCamera>().transform;

        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetVisibleState(bool shown)
    {
        isShown = shown;

        if (!shown)
            rayCastPos += Vector3.down * 5f;
    }

    public void SetData(Vector3 pos, string displayName)
    {
        rayCastPos = pos;
        text.text = displayName;
    }

    private void Update()
    {
        cg.alpha = Mathf.Lerp(cg.alpha, isShown ? 1f : 0f, Time.deltaTime * 10f);

        if (isShown)
        {
            transform.position = Vector3.Lerp(transform.position, rayCastPos, Time.deltaTime * 15f);

            Vector3 targetDirection = transform.position - cameraTransform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime, 0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
