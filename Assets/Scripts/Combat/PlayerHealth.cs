using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private void Start()
    {
        UIManager.Instance.UpdatePlayerHealth(CurrentHealth);
    }

    protected override void HandleDeath()
    {
        Debug.Log("Im Dead!");
    }
}
