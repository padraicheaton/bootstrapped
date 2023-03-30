using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
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

            Instantiate(objsToSpawn[Random.Range(0, objsToSpawn.Count)], transform.position, Quaternion.identity);
        }
    }
}
