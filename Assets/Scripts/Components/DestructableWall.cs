using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour
{
    private void Start()
    {
        GetComponent<HealthComponent>().onDeath += () =>
        {
            GetComponent<Collider>().enabled = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                Rigidbody rb = transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                rb.AddForce(Random.onUnitSphere * 25f, ForceMode.Impulse);
            }

            Destroy(gameObject, 3f);
        };
    }
}
