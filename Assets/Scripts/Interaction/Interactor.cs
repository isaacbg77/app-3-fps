using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    [SerializeField, Range(0, 50)] protected float range;

    protected void DoInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
        {
            if (hit.transform.gameObject.TryGetComponent(out Interactable i))
            {
                i.Interact();
            }
        }
    }
}
