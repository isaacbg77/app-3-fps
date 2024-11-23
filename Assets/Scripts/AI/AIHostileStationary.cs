using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHostileStationary : AIBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Gun gun;
    [SerializeField, Range(0, 90)] private float fovAngle = 45f;
    [SerializeField, Range(0, 100)] private float fovRange = 45f;
    
    protected override void HandleAIBehaviour()
    {
        if (state == AIState.Idle)
        {
            if (CanSeeTarget(transform, target, fovAngle, fovRange))
            {
                ChangeState(AIState.Chase);
            }
        }
        else if (state == AIState.Chase)
        {
            if (!CanSeeTarget(transform, target, fovAngle, fovRange))
            {
                ChangeState(AIState.Idle);
            }
            
            navAgent.SetDestination(target.position);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            if (navAgent.remainingDistance <= fovRange)
            {
                if (gun.ClipAmmoLeft > 0)
                {
                    gun.Fire();
                }
                else
                {
                    gun.ReloadClip(false);
                }
            }
        }
    }
}
