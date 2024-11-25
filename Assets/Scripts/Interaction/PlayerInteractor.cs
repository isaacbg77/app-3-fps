using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField, Range(0, 50)] private float range;

    private void Update()
    {
        if (Input.GetButtonUp("Interact"))
        {
            DoInteract();
        }
    }

    private void DoInteract()
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
