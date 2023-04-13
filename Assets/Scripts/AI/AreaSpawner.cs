using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objsToSpawn;
    [SerializeField] private int numObjsToSpawn;
    [SerializeField] private bool oneshot;
    [SerializeField] private Vector3 spawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        for (int i = 0; i < numObjsToSpawn; i++)
        {
            Instantiate(objsToSpawn[Random.Range(0, objsToSpawn.Length)], transform.position + spawnPos + Random.onUnitSphere * 2f, Quaternion.identity);
        }

        if (oneshot)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + spawnPos, 2f);
    }
}
