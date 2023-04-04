using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHelper : MonoBehaviour
{
    public void OnPlatformProjectileDestroyed(GameObject prefab, float lifetime)
    {
        GameObject platform = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        platform.layer = LayerMask.NameToLayer("Ground");

        platform.GetComponentInChildren<MeshRenderer>().material.color = Color.grey;

        Destroy(platform, lifetime);
    }
}
