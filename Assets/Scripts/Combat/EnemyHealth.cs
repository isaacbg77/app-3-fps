using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    [SerializeField] private PhysicMaterial deathMaterial;
    [SerializeField, Range(0, 100)] private float deathForceStrength = 5f;
    [SerializeField, Range(0, 50)] private float despawnTime = 5f;

    protected override void HandleDeath()
    {
        // Update game manager stat
        GameManager.Instance.IncrementKills();

        // Disable some components
        if (TryGetComponent(out NavMeshAgent navAgent))
            navAgent.enabled = false;
        if (TryGetComponent(out AIHostile aiHostile))
            aiHostile.enabled = false;
        if (TryGetComponent(out AIInteractor aiInteractor))
            aiInteractor.enabled = false;

        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;

        // Add physics material to collider with low friction
        GetComponent<Collider>().material = deathMaterial;

        // Apply impulse force for funny effect
        Rigidbody body = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        body.constraints = RigidbodyConstraints.FreezeRotationY;
        body.AddForce(transform.TransformDirection(Vector3.back) * deathForceStrength, ForceMode.Impulse);

        enabled = false;
        StartCoroutine(DestoryWithDelay());
    }

    private IEnumerator DestoryWithDelay()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
