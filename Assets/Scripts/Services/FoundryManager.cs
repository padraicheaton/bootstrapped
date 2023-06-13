using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject foundryPrefab;
    [SerializeField] private Transform[] foundryPoints;
    private int currentPointIndex = -1;

    private Foundry foundryRef;
    private Outline[] foundryOutline;

    private void Start()
    {
        GameObject foundryObject = Instantiate(foundryPrefab, transform);
        foundryRef = foundryObject.GetComponent<Foundry>();
        foundryOutline = foundryObject.GetComponentsInChildren<Outline>();

        OnWaveEnd();

        ServiceLocator.instance.GetService<Spawner>().onSwarmEnd += OnWaveEnd;
        ServiceLocator.instance.GetService<Spawner>().onSwarmBegin += OnWaveStart;
    }

    private void OnWaveStart()
    {
        foundryRef.IsInteractable = false;
        SetOutline(false);
        foundryRef.transform.position = Vector3.down * 100f;
    }

    private void OnWaveEnd()
    {
        foundryRef.IsInteractable = true;
        SetOutline(true);
        foundryRef.transform.position = GetNextFoundryPosition();
        foundryRef.transform.rotation = GetFoundryRotation();
    }

    private void SetOutline(bool enabled)
    {
        foreach (Outline o in foundryOutline)
            o.enabled = enabled;
    }

    private Vector3 GetNextFoundryPosition()
    {
        currentPointIndex++;

        if (currentPointIndex >= foundryPoints.Length)
            currentPointIndex = 0;

        return foundryPoints[currentPointIndex].position;
    }

    private Quaternion GetFoundryRotation()
    {
        return foundryPoints[currentPointIndex].rotation;
    }

    private void OnDrawGizmos()
    {
        foreach (Transform t in foundryPoints)
            Gizmos.DrawWireCube(t.position + Vector3.up / 2f, Vector3.one);
    }
}
