using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsPickup : MonoBehaviour
{
    [SerializeField] private float pickupRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private SphereCollider coll;
    private Rigidbody rb;

    private Transform playerTransform;
    private Vector3 rotateDir;

    private void Start()
    {
        coll = gameObject.AddComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        coll.radius = pickupRange;
        coll.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.isKinematic = true;
            playerTransform = other.transform;

            rotateDir = Random.onUnitSphere;
        }
    }

    private void Update()
    {
        if (playerTransform)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * moveSpeed);
            transform.eulerAngles += rotateDir * Time.deltaTime * rotateSpeed;

            if (Vector3.Distance(playerTransform.position, transform.position) < 1f)
            {
                Collect();

                Destroy(gameObject);
            }
        }
    }

    protected abstract void Collect();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
