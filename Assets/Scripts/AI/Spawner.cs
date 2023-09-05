using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private List<GameObject> objsToSpawn = new List<GameObject>();
    [SerializeField] private int spawnPointCount;
    [SerializeField] private float swarmSpawnDelay;
    [SerializeField] private int swarmEnemyCount;

    [Header("Difficulty Settings")]
    [SerializeField] private int swarmCountAdditiveIncrease;

    private List<GameObject> swarmEnemies = new List<GameObject>();
    private List<Vector3> spawnPoints;

    public UnityAction onSwarmBegin;
    public UnityAction onSwarmEnd;

    private bool swarmInProgress = false;

    private void Start()
    {
        onSwarmBegin += () =>
        {
            swarmInProgress = true;

            new EventLogger.Event("Wave Started",
                                $"{swarmEnemyCount} Enemies");
        };

        onSwarmEnd += () =>
        {
            swarmInProgress = false;

            new EventLogger.Event("Wave Ended",
                                "");
        };
    }

    public void BeginSwarm()
    {
        if (!swarmInProgress)
            StartCoroutine(SwarmSpawnRoutine());
    }

    private IEnumerator SwarmSpawnRoutine()
    {
        if (onSwarmBegin != null)
            onSwarmBegin.Invoke();

        spawnPoints = ServiceLocator.instance.GetService<LevelPopulator>().PopPoints(spawnPointCount);

        ServiceLocator.instance.GetService<SoundController>().SetSwarmBackgroundMusic(true);

        for (int i = 0; i < swarmEnemyCount; i++)
        {
            yield return new WaitForSeconds(swarmSpawnDelay);

            swarmEnemies.Add(SpawnEnemy());
        }

        // Wait while there are still remaining swarm enemies
        while (swarmEnemies.Count > 0)
        {
            yield return new WaitForEndOfFrame();

            swarmEnemies.RemoveAll(enemy => enemy == null);
        }

        ServiceLocator.instance.GetService<SoundController>().SetSwarmBackgroundMusic(false);

        // Increase difficulty each swarm
        swarmEnemyCount += swarmCountAdditiveIncrease;

        if (onSwarmEnd != null)
            onSwarmEnd.Invoke();
    }

    private GameObject SpawnEnemy()
    {
        Vector3 spawnPoint = transform.position;

        if (spawnPoints.Count > 0)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 20f, NavMesh.AllAreas))
            {
                spawnPoint = hit.position;
            }
        }

        return Instantiate(objsToSpawn[Random.Range(0, objsToSpawn.Count)], spawnPoint, Quaternion.identity);
    }
}
