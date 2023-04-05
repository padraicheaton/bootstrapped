using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Settings")]
    [SerializeField] private List<GameObject> objsToSpawn = new List<GameObject>();
    [SerializeField] private float delayBetweenSpawning;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenSpawning);

            Vector3 spawnPoint = transform.position;

            if (spawnPoints.Count > 0)
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;

            Instantiate(objsToSpawn[Random.Range(0, objsToSpawn.Count)], spawnPoint, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Transform p in spawnPoints)
            Gizmos.DrawSphere(p.position, 1f);
    }
}
