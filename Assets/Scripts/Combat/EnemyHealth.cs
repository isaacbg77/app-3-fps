using UnityEngine;

public class EnemyHealth : Health
{
    protected override void HandleDeath()
    {
        GameManager.Instance.IncrementKills();
        Destroy(gameObject);
    }
}
