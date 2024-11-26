using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHostilePatrol : AIHostile
{
    [SerializeField, Range(1, 100)] protected float patrolRange;
    [SerializeField, Range(0, 20)] protected float waitTime = 5f;

    protected bool canChooseNextDestination = true;

    protected override void HandleIdle()
    {
        base.HandleIdle();

        if (canChooseNextDestination)
        {
            canChooseNextDestination = false;
            StartCoroutine(ChooseNextDestination());
        }
    }

    protected override void HandleRun()
    {
        if (CanSeeTarget(transform, target, fovAngle, fovRange))
        {
            StartCoroutine(TargetWithDelay());
        }
        else if (navAgent.remainingDistance <= navAgent.stoppingDistance + 0.1f)
        {
            ChangeState(AIState.Idle);
        }
    }

    protected IEnumerator ChooseNextDestination()
    {
        yield return new WaitForSeconds(waitTime);

        if (state != AIState.Chase)
        {
            if (GetNextRandomPosition(transform.position, patrolRange, out Vector3 result))
            {
                navAgent.SetDestination(result);
            }
            ChangeState(AIState.Run);
        }

        canChooseNextDestination = true;
    }
}
