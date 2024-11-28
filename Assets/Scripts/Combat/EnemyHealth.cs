using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    [SerializeField] private PhysicMaterial deathMaterial;
    [SerializeField, Range(0, 100)] private float deathForceStrength = 5f;

    protected override void HandleDeath()
    {
        // Update game manager stat
        GameManager.Instance.IncrementKills();

        // Disable some components
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<AIHostile>().enabled = false;
        GetComponent<AIInteractor>().enabled = false;
        GetComponentInChildren<Animator>().enabled = false;

        // Add physics material to collider with low friction
        GetComponent<Collider>().material = deathMaterial;

        // Apply impulse force for funny effect
        Rigidbody body = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        body.constraints = RigidbodyConstraints.FreezeRotationY;
        body.AddForce(transform.TransformDirection(Vector3.back) * deathForceStrength, ForceMode.Impulse);

        enabled = false;
    }
}
