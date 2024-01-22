using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHelper : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;

    public void OnPlatformProjectileDestroyed(GameObject prefab, float lifetime)
    {
        GameObject platform = Instantiate(platformPrefab, prefab.transform.position, Quaternion.identity);

        platform.transform.localScale = prefab.transform.localScale;

        Destroy(platform, lifetime);
    }
}
