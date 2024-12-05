using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHostile : AIBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected Gun gun;
    [SerializeField, Range(0, 90)] protected float fovAngle = 45f;
    [SerializeField, Range(0, 100)] protected float fovRange = 45f;
    [SerializeField, Range(0, 10)] protected float maxReactionTime = 2f;
    
    protected override void HandleIdle()
    {
        if (CanSeeTarget(transform, target, fovAngle, fovRange))
        {
            StartCoroutine(TargetWithDelay());
        }
    }

    protected override void HandleChase()
    {
        if (!CanSeeTarget(transform, target, fovAngle, fovRange) && !notified)
        {
            ChangeState(AIState.Idle);
        }
        
        navAgent.SetDestination(target.position);
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        if (navAgent.remainingDistance <= fovRange && !isMoving)
        {
            if (gun.ClipAmmoLeft > 0 && !gun.Reloading)
            {
                gun.Fire();
                anim.Play("Fire");
            }
            else
            {
                gun.ReloadClip(false);
            }
        }
    }

    protected IEnumerator TargetWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(0, maxReactionTime));
        ChangeState(AIState.Chase);
    }
}
