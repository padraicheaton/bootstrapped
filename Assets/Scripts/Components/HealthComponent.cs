using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnDeath = false;

    [Header("Stats")]
    [SerializeField] private float damageTakenMultiplier = 1f;
    [SerializeField] public float maximumHealth;
    [SerializeField] public float currentHealth;

    private bool deathTriggered = false;

    public UnityAction<float> onDamageTaken, onHealthReceived;
    public UnityAction onDeath;

    private void Start()
    {
        Reset();

        if (destroyOnDeath)
            onDeath += () => Destroy(gameObject);
    }

    public void SetHealth(float amount)
    {
        maximumHealth = currentHealth = amount;
    }

    public void Reset()
    {
        currentHealth = maximumHealth;

        deathTriggered = false;
    }

    public void TakeDamage(float amount)
    {
        amount *= damageTakenMultiplier;

        currentHealth -= amount;

        if (onDamageTaken != null)
            onDamageTaken.Invoke(amount);

        if (currentHealth <= 0 && !deathTriggered)
        {
            deathTriggered = true;

            if (onDeath != null)
                onDeath.Invoke();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (onHealthReceived != null)
            onHealthReceived.Invoke(amount);

        if (currentHealth > maximumHealth)
            currentHealth = maximumHealth;
    }
}