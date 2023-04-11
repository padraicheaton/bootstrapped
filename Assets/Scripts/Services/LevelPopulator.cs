using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPopulator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cachePrefab;
    [SerializeField] private GameObject refillStationPrefab;

    [Header("Settings")]
    [SerializeField] private Vector2Int levelDimensions;
    [SerializeField] private bool drawLevelDimensions;
    [SerializeField] private int pointIncrement;
    [SerializeField] private int numCaches;
    [SerializeField] private int numRefillStations;

    private List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        CalculatePoints();

        PopulateLevel();
    }

    private void CalculatePoints()
    {
        for (int x = -levelDimensions.x / 2 + pointIncrement; x < levelDimensions.x / 2; x += pointIncrement)
        {
            for (int z = -levelDimensions.y / 2 + pointIncrement; z < levelDimensions.y / 2; z += pointIncrement)
            {
                Vector3 rayOrigin = new Vector3(x, 1000, z);

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit))
                {
                    points.Add(hit.point);
                }
            }
        }
    }

    private void PopulateLevel()
    {
        // Spawn Caches
        SpawnObject(cachePrefab, numCaches);
    }

    public void SpawnRefillStations()
    {
        SpawnObject(refillStationPrefab, numRefillStations);
    }

    private void SpawnObject(GameObject obj, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int choice = Random.Range(0, points.Count);

            Instantiate(obj, points[choice], Quaternion.identity);

            points.RemoveAt(choice);
        }
    }

    private void OnDrawGizmos()
    {
        if (drawLevelDimensions)
        {
            Gizmos.DrawCube(transform.position, new Vector3(levelDimensions.x, 1f, levelDimensions.y));
        }

        foreach (Vector3 p in points)
            Gizmos.DrawSphere(p, 0.25f);
    }
}
