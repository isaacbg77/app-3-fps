using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState { Idle, Chase, Run }

public abstract class AIBehaviour : MonoBehaviour
{
    protected NavMeshAgent navAgent;
    protected Animator anim;
    protected AIState state = AIState.Idle;
    protected bool isMoving = false;

    [SerializeField, Range(0, 50)] protected float stopNotifyTime = 5f;
    protected bool notified = false;
    private Coroutine notifyCoroutine;

    protected void Awake()
    {
        if (TryGetComponent(out NavMeshAgent navAgent))
        {
            this.navAgent = navAgent;
        }
        else
            Debug.LogError("Missing NavMeshAgent component!");

        anim = GetComponentInChildren<Animator>();
        if (anim == null)
            Debug.LogError("Missing Animator component!");
    }

    protected void Update()
    {
        CheckMovement();

        if (state == AIState.Idle)
        {
            HandleIdle();
        }
        else if (state == AIState.Chase)
        {
            HandleChase();
        }
        else if (state == AIState.Run)
        {
            HandleRun();
        }
    }

    // AI must implement a default idle behaviour, and can optionally implement other behaviours
    protected abstract void HandleIdle();
    protected virtual void HandleChase() { /* Chase behaviour */ }
    protected virtual void HandleRun() { /* Run away behaviour */ }

    // -------------------------------------------------------------------------------------------

    private void CheckMovement()
    {
        // Check if nav agent is moving and update animation
        Vector3 velocityXZ = new(navAgent.velocity.x, 0, navAgent.velocity.z);
        isMoving = velocityXZ.sqrMagnitude > 0;
        anim.SetBool("IsMoving", isMoving);
    }

    protected void ChangeState(AIState newState)
    {
        state = newState;
    }
    
    public AIState GetState()
    {
        return state;
    }

    public void Notify()
    {
        if (notifyCoroutine != null)
        {
            StopCoroutine(notifyCoroutine);
        }
        notifyCoroutine = StartCoroutine(NotifyAsync());
    }

    protected IEnumerator NotifyAsync()
    {
        notified = true;
        yield return new WaitForSeconds(stopNotifyTime);
        notified = false;
    }
    
    // -------------------------------------------------------------------------------------------

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

        if (Physics.Raycast(start.position, dir, out RaycastHit hit, range))
        {
            return (angleBetween <= angle) && hit.transform.gameObject == target.gameObject;
        }
        else
        {
            return false;
        }
    }
}
