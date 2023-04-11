using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Settings")]
    [SerializeField] private List<GameObject> objsToSpawn = new List<GameObject>();
    [SerializeField] private float trickleSpawnDelay;
    [SerializeField] private float swarmSpawnDelay;
    [SerializeField] private int swarmEnemyCount;
    [SerializeField] private float delayBetweenSwarms;

    [Header("Difficulty Settings")]
    [SerializeField] private float swarmDelayAdditiveReduction;
    [SerializeField] private int swarmCountAdditiveIncrease;

    private void Start()
    {
        StartCoroutine(TrickleSpawnRoutine());
        StartCoroutine(SwarmSpawnRoutine());
    }

    private IEnumerator TrickleSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(trickleSpawnDelay);

            SpawnEnemy();
        }
    }

    private IEnumerator SwarmSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenSwarms);

            for (int i = 0; i < swarmEnemyCount; i++)
            {
                yield return new WaitForSeconds(swarmSpawnDelay);

                SpawnEnemy();
            }

            // Increase difficulty each swarm
            swarmEnemyCount += swarmCountAdditiveIncrease;
            delayBetweenSwarms -= swarmDelayAdditiveReduction;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPoint = transform.position;

        if (spawnPoints.Count > 0)
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;

        Instantiate(objsToSpawn[Random.Range(0, objsToSpawn.Count)], spawnPoint, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Transform p in spawnPoints)
            Gizmos.DrawSphere(p.position, 1f);
    }
}
