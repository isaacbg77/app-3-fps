using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int totalHealth = 100;

    protected int currentHealth;
    public int CurrentHealth => currentHealth;

    protected void Awake()
    {
        currentHealth = totalHealth;
    }

    public void IncreaseHealth(int amount)
    {
        if (amount < 0) return;

        currentHealth += amount;
        if (currentHealth > totalHealth)
            currentHealth = totalHealth;
    }

    public void DecreaseHealth(int amount)
    {
        if (amount < 0) return;
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HandleDeath();
        }
    }

    protected abstract void HandleDeath();
}
