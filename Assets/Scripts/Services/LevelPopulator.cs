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
    [SerializeField] private float rayOriginHeight = 45f;

    private List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        CalculatePoints();

        PopulateLevel();
    }

    private void CalculatePoints()
    {
        float offset = (float)pointIncrement / 2f;

        for (float x = -levelDimensions.x / 2 + offset; x < levelDimensions.x / 2 - offset; x += pointIncrement)
        {
            for (float z = -levelDimensions.y / 2 + offset; z < levelDimensions.y / 2 - offset; z += pointIncrement)
            {
                Vector3 rayOrigin = new Vector3(x, rayOriginHeight, z);

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
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

    public List<Vector3> PopPoints(int count)
    {
        List<Vector3> p = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            int choice = Random.Range(0, points.Count);

            p.Add(points[choice]);
        }

        return p;
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
            Gizmos.DrawSphere(p, 0.51f);

        Gizmos.DrawWireSphere(transform.position + Vector3.up * rayOriginHeight, 0.5f);
    }
}
