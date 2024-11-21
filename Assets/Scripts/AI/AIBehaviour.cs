using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState { Idle, Chase, Run }

public abstract class AIBehaviour : MonoBehaviour
{
    protected NavMeshAgent navAgent;
    protected AIState state = AIState.Idle;

    protected void Awake()
    {
        if (TryGetComponent(out NavMeshAgent navAgent))
        {
            this.navAgent = navAgent;
        }
        else
            Debug.LogError("Missing NavMeshAgent component!");
    }

    protected void Update()
    {
        HandleAIBehaviour();
    }

    protected abstract void HandleAIBehaviour();

    // -------------------------------------------------------------------------------------------

    public void ChangeState(AIState newState)
    {
        state = newState;
    }
    
    protected static bool GetNextRandomPosition(Vector3 start, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPosition = start + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected static bool CanSeeTarget(Transform start, Transform target, float angle, float range)
    {
        Vector3 dir = (target.position - start.position).normalized;
        float angleBetween = Vector3.Angle(start.forward, dir);
        string layerName = LayerMask.LayerToName(target.gameObject.layer);
        bool hitTarget = Physics.Raycast(start.position, dir, range, LayerMask.GetMask(layerName));

        return (angleBetween <= angle) && hitTarget;
    }
}
