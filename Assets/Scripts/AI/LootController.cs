using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();

    [Header("Settings")]
    [SerializeField][Range(0f, 1f)] private float itemDropChance;

    public void OnEnemyDefeated(Vector3 position, int numItemsToDrop = 1)
    {
        for (int i = 0; i < numItemsToDrop; i++)
        {
            if (Random.value < itemDropChance)
            {
                int choice = Random.Range(0, items.Count);

                Instantiate(items[choice], position, Quaternion.identity);
            }
        }
    }

    public void AddItemDropChance(float additiveChance)
    {
        itemDropChance += additiveChance;
    }
}
