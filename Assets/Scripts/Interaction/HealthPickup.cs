using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
        {
            health.IncreaseHealth(healthAmount);
            health.UpdateUI();
            gameObject.SetActive(false);
        }
    }
}
