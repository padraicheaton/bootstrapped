using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objsToSpawn;
    [SerializeField] private int numObjsToSpawn;
    [SerializeField] private bool oneshot;
    [SerializeField] private Vector3 spawnPos;

    private bool isSpawning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isSpawning)
            StartCoroutine(PsuedoSwarmRoutine());
    }

    private IEnumerator PsuedoSwarmRoutine()
    {
        isSpawning = true;

        ServiceLocator.instance.GetService<SoundController>().SetSwarmBackgroundMusic(true);

        List<GameObject> swarmEnemies = new List<GameObject>();

        for (int i = 0; i < numObjsToSpawn; i++)
        {
            swarmEnemies.Add(Instantiate(objsToSpawn[Random.Range(0, objsToSpawn.Length)], transform.position + spawnPos + Random.onUnitSphere * 2f, Quaternion.identity));
        }

        while (swarmEnemies.Count > 0)
        {
            yield return new WaitForEndOfFrame();

            swarmEnemies.RemoveAll(enemy => enemy == null);
        }

        ServiceLocator.instance.GetService<SoundController>().SetSwarmBackgroundMusic(false);

        isSpawning = false;

        if (oneshot)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + spawnPos, 2f);
    }
}
