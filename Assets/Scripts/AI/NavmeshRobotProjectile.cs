using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshRobotProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private float dmg;

    public void Fire(float damage, float speed, Transform target)
    {
        rb = GetComponent<Rigidbody>();
        dmg = damage;

        transform.LookAt(target);

        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<HealthComponent>(out HealthComponent hc))
            {
                hc.TakeDamage(dmg);
            }
        }

        Destroy(gameObject);
    }
}
